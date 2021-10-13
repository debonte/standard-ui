using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

#nullable enable

namespace StandardUI.CodeGenerator
{
    [Generator]
    internal class SourceGenerator : ISourceGenerator
    {
        private const string AttributeSource = @"
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
            foreach (string controlTypeName in rx.ControlsToGenerate)
            {
                string source = GenerateSourceFileContents(context, controlTypeName);
                context.AddSource(controlTypeName, source);
            }
        }

        private static string GenerateSourceFileContents(GeneratorExecutionContext context, string interfaceFullTypeName)
        {
            SourceGenerator.GetTypeNamesFromInterface(interfaceFullTypeName, out string interfaceTypeName, out string interfaceNamespace, out string controlTypeName);

            StringBuilder sb = new StringBuilder();
            sb.Append($@"
// This file was generated

using System;
using Microsoft.StandardUI;
using Microsoft.StandardUI.Controls;
using Microsoft.StandardUI.Media;
using Microsoft.StandardUI.Shapes;
using Microsoft.StandardUI.Wpf.Media;
using Microsoft.StandardUI.Wpf;

namespace SimpleControls.Wpf
{{
    public class {controlTypeName} : StandardControl<{interfaceTypeName}>, {interfaceTypeName}
    {{
        public {controlTypeName}()
        {{
            InitImplementation(new {interfaceNamespace}.{controlTypeName}Implementation<{interfaceTypeName}>(this));
        }}");

            INamedTypeSymbol controlSymbol = context.Compilation.GetTypeByMetadataName(interfaceFullTypeName);
            IEnumerable<INamedTypeSymbol> propertySources = new[] { controlSymbol };//.Concat(controlSymbol.AllInterfaces);

            foreach (INamedTypeSymbol typeSymbol in propertySources)
            {
                foreach (IPropertySymbol propertySymbol in typeSymbol.GetMembers().Where(member => member.Kind == SymbolKind.Property))
                {
                    string propertyName = propertySymbol.Name;
                    string interfacePropertyType = propertySymbol.Type.Name;
                    string publicPropertyType = interfacePropertyType;
                    if (publicPropertyType.Length > 1 && publicPropertyType[0] == 'I' && publicPropertyType[1] == char.ToUpper(publicPropertyType[1]))
                    {
                        publicPropertyType = publicPropertyType.Substring(1);
                    }

                    sb.Append($@"

        public static readonly System.Windows.DependencyProperty {propertyName}Property = PropertyUtils.Register(nameof({propertyName}), typeof({publicPropertyType}), typeof({controlTypeName}), null);

        public {publicPropertyType}? {propertyName}
        {{
            get => ({publicPropertyType}?)GetValue({propertyName}Property);
");

                    if (propertySymbol.SetMethod != null)
                    {
                        sb.AppendLine($@"            set => SetValue({propertyName}Property, value);");
                    }

                    sb.Append($@"        }}

        {interfacePropertyType} {propertySymbol.ContainingType.Name}.{propertyName}
        {{
            get => {propertyName};
");

                    if (propertySymbol.SetMethod != null)
                    {
                        sb.AppendLine($@"            set => { propertyName} = ({publicPropertyType}?)value;");
                    }

                    sb.Append($@"        }}");
                }
            }

            sb.Append($@"
    }}
}}");

            return sb.ToString();
        }

        private static void GetTypeNamesFromInterface(string interfaceFullTypeName, out string interfaceTypeName, out string interfaceNamespace, out string controlTypeName)
        {
            int lastDotIndex = interfaceFullTypeName.LastIndexOf('.');

            interfaceTypeName = interfaceFullTypeName.Substring(lastDotIndex + 1);
            interfaceNamespace = interfaceFullTypeName.Substring(0, lastDotIndex);
            controlTypeName = interfaceTypeName.Substring(1);
        }

        class SyntaxReceiver : ISyntaxContextReceiver
        {
            public List<string> ControlsToGenerate = new List<string>();

            public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
            {
                if (context.Node is AttributeSyntax attrib
                    && attrib.ArgumentList?.Arguments.Count == 1
                    && context.SemanticModel.GetTypeInfo(attrib).Type?.ToDisplayString() == "StandardUIControlAttribute")
                {
                    string typeName = context.SemanticModel.GetConstantValue(attrib.ArgumentList.Arguments[0].Expression).ToString();

                    ControlsToGenerate.Add(typeName);
                }
            }
        }
    }
}
