using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace StandardUI.CodeGenerator
{
    /// <summary>
    /// Generates framework-specific implementations of StandardUI control interfaces. The
    /// consuming app specifies the control interfaces that it consumes via StandardUIControl
    /// assembly attributes.
    /// </summary>
    /// <example>
    /// [assembly: StandardUIControl("Namespace.IControlName")]
    /// </example>
    [Generator]
    internal class SourceGenerator : ISourceGenerator
    {
        private const string AttributeSource = @"// This file was generated

[System.AttributeUsage(System.AttributeTargets.Assembly, AllowMultiple=true)]
internal sealed class StandardUIControlAttribute : System.Attribute
{
    public string TypeName { get; }
    public StandardUIControlAttribute(string typeName)
    {
        TypeName = typeName;
    }
}
";

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForPostInitialization((pi) => pi.AddSource("StandardUI_AssemblyAttributes", AttributeSource));
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            SyntaxReceiver rx = (SyntaxReceiver)context.SyntaxContextReceiver!;
            foreach (string interfaceFullTypeName in rx.InterfacesToGenerate)
            {
                SourceGenerator.GenerateSourceFile(context, interfaceFullTypeName);
            }
        }

        private static void GenerateSourceFile(GeneratorExecutionContext context, string interfaceFullTypeName)
        {
            INamedTypeSymbol interfaceSymbol = context.Compilation.GetTypeByMetadataName(interfaceFullTypeName);
            if (interfaceSymbol == null)
            {
                return;
            }

            if (!SourceGenerator.TryGetTypeNamesFromInterface(interfaceFullTypeName, out string interfaceNamespace, out string controlTypeName))
            {
                return;
            }

            StringBuilder sourceCode = new StringBuilder();
            sourceCode.Append($@"// This file was generated

using Microsoft.StandardUI.Media;
using Microsoft.StandardUI.Wpf.Media;
using Microsoft.StandardUI.Wpf;

namespace SimpleControls.Wpf
{{
    public class {controlTypeName} : StandardControl, {interfaceFullTypeName}
    {{
        public {controlTypeName}()
        {{
            InitImplementation(new {interfaceNamespace}.{controlTypeName}Implementation<{interfaceFullTypeName}>(this));
        }}");

            SourceGenerator.GenerateProperties(interfaceSymbol, controlTypeName, sourceCode);

            sourceCode.Append($@"
    }}
}}");

            // Create the file
            context.AddSource(controlTypeName, sourceCode.ToString());
        }

        /// <summary>
        /// Appends the source code to implement the properties on <paramref name="interfaceSymbol"/> to <paramref name="sourceCode"/>.
        /// </summary>
        /// <example>
        /// public Brush? Fill
        /// {
        ///     get => (Brush?)GetValue(FillProperty);
        ///     set => SetValue(FillProperty, value);
        /// }
        ///
        /// IBrush IRadialGauge.Fill
        /// {
        ///     get => Fill;
        ///     set => Fill = (Brush?)value;
        /// }
        /// </example>
        /// <param name="interfaceSymbol">The interface whose properties should be implemented.</param>
        /// <param name="controlTypeName">The name of the control class that is implementing the interface.</param>
        /// <param name="sourceCode">The StringBuilder to which the source code should be written.</param>
        private static void GenerateProperties(INamedTypeSymbol interfaceSymbol, string controlTypeName, StringBuilder sourceCode)
        {
            foreach (IPropertySymbol propertySymbol in interfaceSymbol.GetMembers().Where(member => member.Kind == SymbolKind.Property))
            {
                string propertyName = propertySymbol.Name;
                string explicitInterfacePropertyType = propertySymbol.Type.Name;
                string publicPropertyType = SourceGenerator.IsStandardUIInterface(propertySymbol.Type) ? explicitInterfacePropertyType.Substring(1) : explicitInterfacePropertyType;

                // The dependency property
                sourceCode.Append($@"

        public static readonly System.Windows.DependencyProperty {propertyName}Property = PropertyUtils.Register(nameof({propertyName}), typeof({publicPropertyType}), typeof({controlTypeName}), null);
");

                // The public property implementation
                sourceCode.Append($@"
        public {publicPropertyType}? {propertyName}
        {{
            get => ({publicPropertyType}?)GetValue({propertyName}Property);");

                if (propertySymbol.SetMethod != null)
                {
                    sourceCode.Append($@"
            set => SetValue({propertyName}Property, value);");
                }

                sourceCode.Append($@"
        }}
");

                // The explicit property implementation
                sourceCode.Append($@"
        {explicitInterfacePropertyType} {propertySymbol.ContainingType.Name}.{propertyName}
        {{
            get => {propertyName};");

                if (propertySymbol.SetMethod != null)
                {
                    sourceCode.Append($@"
            set => { propertyName} = ({publicPropertyType}?)value;");
                }

                sourceCode.Append($@"
        }}");
            }
        }

        /// <summary>
        /// Returns true if the Roslyn symbol represents a Standard UI control interface, otherwise false.
        /// </summary>
        private static bool IsStandardUIInterface(ITypeSymbol type)
        {
            string typeName = type.Name;

            return typeName.Length > 1 && typeName[0] == 'I' && typeName[1] == char.ToUpper(typeName[1]);
        }

        /// <summary>
        /// Given the full name (with namespace) of an interface type, extracts various other related strings.
        /// </summary>
        /// <param name="interfaceFullTypeName">The full name (with namespace) of an interface type. For example, Contoso.Controls.IControl</param>
        /// <param name="interfaceNamespace">The interface's namespace. For example, Contoso.Controls</param>
        /// <param name="controlTypeName">The default name of the class implementing the interface (by convention). For example, Control</param>
        /// <returns>True if the output strings were successfully determined, otherwise false.</returns>
        private static bool TryGetTypeNamesFromInterface(string interfaceFullTypeName, out string interfaceNamespace, out string controlTypeName)
        {
            int lastDotIndex = interfaceFullTypeName.LastIndexOf('.');
            if (lastDotIndex < 3)
            {
                interfaceNamespace = null;
                controlTypeName = null;
                return false;
            }

            controlTypeName = interfaceFullTypeName.Substring(lastDotIndex + 2);
            interfaceNamespace = interfaceFullTypeName.Substring(0, lastDotIndex);
            return true;
        }

        /// <summary>
        /// Roslyn calls this class when code changes are made. If we detect a change to the StandardUIControlAttribute set
        /// we will generate a matching set of source files.
        /// </summary>
        class SyntaxReceiver : ISyntaxContextReceiver
        {
            public List<string> InterfacesToGenerate = new List<string>();

            public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
            {
                if (context.Node is AttributeSyntax attrib
                    && attrib.ArgumentList?.Arguments.Count == 1
                    && context.SemanticModel.GetTypeInfo(attrib).Type?.ToDisplayString() == "StandardUIControlAttribute")
                {
                    string interfaceFullTypeName = context.SemanticModel.GetConstantValue(attrib.ArgumentList.Arguments[0].Expression).ToString();

                    InterfacesToGenerate.Add(interfaceFullTypeName);
                }
            }
        }
    }
}
