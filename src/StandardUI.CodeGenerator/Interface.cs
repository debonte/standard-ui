using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace StandardUI.CodeGenerator
{
    public class Interface
    {
        public static int IndentSize = 4;
        public const string RootNamespace = "System.StandardUI";

        private readonly NameSyntax _sourceNamespaceName;
        private readonly CompilationUnitSyntax _sourceCompilationUnit;
        private readonly QualifiedNameSyntax _destinationNamespaceName;

        public Context Context { get; }
        public string DestinationClassName { get; }
        public InterfaceDeclarationSyntax Declaration { get; }
        public InterfaceDeclarationSyntax? AttachedInterfaceDeclaration { get; }
        public string Name { get; }
        public string VariableName { get; }

        public Interface(Context context, InterfaceDeclarationSyntax sourceInterfaceDeclaration, InterfaceDeclarationSyntax? sourceAttachedInterfaceDeclaration)
        {
            Context = context;
            Declaration = sourceInterfaceDeclaration;
            AttachedInterfaceDeclaration = sourceAttachedInterfaceDeclaration;

            Name = sourceInterfaceDeclaration.Identifier.Text;
            if (!Name.StartsWith("I"))
                throw new UserViewableException($"Data model interface {Name} must start with 'I'");

            DestinationClassName = Name.Substring(1);

            // Form the default variable name for the interface by dropping the "I" and lower casing the first letter(s) after (ICanvas => canvas)
            VariableName = Context.TypeNameToVariableName(Name.Substring(1));

            if (!(Declaration.Parent is NamespaceDeclarationSyntax interfaceNamespaceDeclaration))
                throw new UserViewableException(
                    $"Parent of ${Name} interface should be namespace declaration, but it's a {Declaration.Parent.GetType()} node instead");
            _sourceNamespaceName = interfaceNamespaceDeclaration.Name;

            if (!(interfaceNamespaceDeclaration.Parent is CompilationUnitSyntax compilationUnit))
                throw new UserViewableException(
                    $"Parent of ${interfaceNamespaceDeclaration} namespace should be compilation unit, but it's a {interfaceNamespaceDeclaration.Parent!.GetType()} node instead");
            _sourceCompilationUnit = compilationUnit;

            _destinationNamespaceName = Context.ToDestinationNamespaceName(_sourceNamespaceName);
        }

        public void Generate()
        {
            var properties = new List<Property>();

            var mainClassStaticFields = new Source(Context);
            var mainClassStaticMethods = new Source(Context);
            var mainClassNonstaticFields = new Source(Context);
            var mainClassNonstaticMethods = new Source(Context);

            var extensionClassMethods = new Source(Context);
            var attachedClassStaticFields = new Source(Context);
            var attachedClassMethods = new Source(Context);

            // Add the property descriptors and accessors
            foreach (MemberDeclarationSyntax modelObjectMember in Declaration.Members)
            {
                if (!(modelObjectMember is PropertyDeclarationSyntax modelProperty))
                    continue;

                string propertyName = modelProperty.Identifier.Text;
                var property = new Property(Context, this, modelProperty);
                properties.Add(property);

                property.GenerateDescriptor(mainClassStaticFields);
                property.GenerateFieldIfNeeded(mainClassNonstaticFields);
                property.GenerateMethods(mainClassNonstaticMethods);
                property.GenerateExtensionClassMethods(extensionClassMethods);
            }

            if (Context.IncludeOnVisualize(Declaration))
            {
                mainClassNonstaticMethods.AddBlankLineIfNonempty();
                mainClassNonstaticMethods.AddLine(
                    $"public override void OnVisualize(IVisualizer visualizer) => visualizer.Draw{DestinationClassName}(this);");
            }

            // Add a special case for the WPF visual tree child methods for Panel; later we'll generalize this as needed
            if (Name == "IPanel" && Context.OutputType is WpfXamlOutputType)
            {
                mainClassNonstaticMethods.AddBlankLineIfNonempty();

                mainClassNonstaticMethods.AddLine(
                    "protected override int VisualChildrenCount => _uiElementCollection.Count;");
                mainClassNonstaticMethods.AddBlankLine();
                mainClassNonstaticMethods.AddLine(
                    "protected override Windows.Media.Visual GetVisualChild(int index) => (Windows.Media.Visual) _uiElementCollection[index];");
            }

            // If there are any attached properties, add the property descriptors and accessors for them
            if (AttachedInterfaceDeclaration != null)
            {
                foreach (MemberDeclarationSyntax member in AttachedInterfaceDeclaration.Members)
                {
                    if (!(member is MethodDeclarationSyntax getterMethodDeclaration))
                        continue;

                    // We just process the Get 
                    string methodName = getterMethodDeclaration.Identifier.Text;
                    if (!methodName.StartsWith("Get"))
                    {
                        if (!methodName.StartsWith("Set"))
                            throw new UserViewableException(
                                $"Attached type method {AttachedInterfaceDeclaration.Identifier.Text}.{methodName} doesn't start with Get or Set");
                        else continue;
                    }

                    string propertyName = methodName.Substring("Get".Length);
                    string setterMethodName = "Set" + propertyName;
                    MethodDeclarationSyntax? setterMethodDeclaration = (MethodDeclarationSyntax?)AttachedInterfaceDeclaration.Members.
                        FirstOrDefault(m => m is MethodDeclarationSyntax potentialSetter && potentialSetter.Identifier.Text == setterMethodName);

                    var attachedProperty = new AttachedProperty(Context, this, AttachedInterfaceDeclaration, getterMethodDeclaration, setterMethodDeclaration);

                    attachedProperty.GenerateMainClassDescriptor(mainClassStaticFields);
                    attachedProperty.GenerateMainClassMethods(mainClassStaticMethods);
                    attachedProperty.GenerateAttachedClassMethods(attachedClassMethods);
                }
            }

            Source usingDeclarations = GenerateUsingDeclarations(!mainClassStaticFields.IsEmpty);

            string? destinationBaseClass = GetDestinationBaseClass();

            Source? constructor = GenerateConstructor(properties);

            string platformOutputDirectory = Context.GetPlatformOutputDirectory(_sourceNamespaceName);

            string mainClassDerviedFrom;
            if (destinationBaseClass == null)
                mainClassDerviedFrom = Name;
            else
                mainClassDerviedFrom = $"{destinationBaseClass}, {Name}";

            bool isPartial = Context.IsPanelSubclass(Declaration);

            Source mainClassSource = GenerateClassFile(usingDeclarations, _destinationNamespaceName, DestinationClassName, mainClassDerviedFrom, isPartial: isPartial,
                constructor: constructor, staticFields: mainClassStaticFields, staticMethods: mainClassStaticMethods, nonstaticFields: mainClassNonstaticFields,
                nonstaticMethods: mainClassNonstaticMethods);
            mainClassSource.WriteToFile(platformOutputDirectory, DestinationClassName + ".cs");

            if (AttachedInterfaceDeclaration != null)
            {
                string attachedClassName = DestinationClassName + "Attached";
                string attachedClassDerivedFrom = AttachedInterfaceDeclaration.Identifier.Text;

                attachedClassStaticFields.AddLine($"public static {attachedClassName} Instance = new {attachedClassName}();");

                Source attachedClassSource = GenerateClassFile(usingDeclarations, _destinationNamespaceName, attachedClassName, attachedClassDerivedFrom,
                    staticFields: attachedClassStaticFields, nonstaticMethods: attachedClassMethods);
                attachedClassSource.WriteToFile(platformOutputDirectory, attachedClassName + ".cs");
            }

            if (!extensionClassMethods.IsEmpty)
            {
                string extensionsClassName = DestinationClassName + "Extensions";
                Source extensionsClassSource = GenerateStaticClassFile(GenerateExtensionsClassUsingDeclarations(), _sourceNamespaceName, extensionsClassName, extensionClassMethods);
                extensionsClassSource.WriteToFile(Context.GetSharedOutputDirectory(_sourceNamespaceName), extensionsClassName + ".cs");
            }
        }

        public Source GenerateClassFile(Source usingDeclarations, NameSyntax namespaceName, string className, string derivedFrom, bool isPartial = false,
            Source? constructor = null, Source? staticFields = null, Source? staticMethods = null, Source? nonstaticFields = null, Source? nonstaticMethods = null)
        {
            Source fileSource = new Source(Context);

            GenerateFileHeader(fileSource);

            if (!usingDeclarations.IsEmpty)
            {
                fileSource.AddSource(usingDeclarations);
                fileSource.AddBlankLine();
            }

            fileSource.AddLines(
                $"namespace {namespaceName}",
                "{");

            using (fileSource.Indent())
            {
                Source classBody = new Source(Context);
                if (staticFields != null && !staticFields.IsEmpty)
                    classBody.AddSource(staticFields);
                if (staticMethods != null && !staticMethods.IsEmpty)
                {
                    classBody.AddBlankLineIfNonempty();
                    classBody.AddSource(staticMethods);
                }
                if (nonstaticFields != null && !nonstaticFields.IsEmpty)
                {
                    classBody.AddBlankLineIfNonempty();
                    classBody.AddSource(nonstaticFields);
                }
                if (constructor != null && !constructor.IsEmpty)
                {
                    classBody.AddBlankLineIfNonempty();
                    classBody.AddSource(constructor);
                }
                if (nonstaticMethods != null && !nonstaticMethods.IsEmpty)
                {
                    classBody.AddBlankLineIfNonempty();
                    classBody.AddSource(nonstaticMethods);
                }

                string modifiers = isPartial ? "public partial" : "public";
                fileSource.AddLines(
                    $"{modifiers} class {className} : {derivedFrom}",
                    "{");
                using (fileSource.Indent())
                    fileSource.AddSource(classBody);
                fileSource.AddLine(
                    "}");
            }

            fileSource.AddLine(
                "}");

            return fileSource;
        }

        public Source GenerateStaticClassFile(Source usingDeclarations, NameSyntax namespaceName, string className, Source staticMethods)
        {
            Source fileSource = new Source(Context);

            GenerateFileHeader(fileSource);

            if (!usingDeclarations.IsEmpty)
            {
                fileSource.AddSource(usingDeclarations);
                fileSource.AddBlankLine();
            }

            fileSource.AddLines(
                $"namespace {namespaceName}",
                "{");

            using (fileSource.Indent())
            {
                fileSource.AddLines(
                    $"public static class {className}",
                    "{");
                using (fileSource.Indent())
                {
                    fileSource.AddSource(
                        staticMethods);
                }
                fileSource.AddLine(
                    "}");
            }

            fileSource.AddLine(
                "}");

            return fileSource;
        }

        private void GenerateFileHeader(Source fileSource)
        {
            fileSource.AddLine($"// This file is generated from {Name}.cs. Update the source file to change its contents.");
            fileSource.AddBlankLine();
        }

        public OutputType OutputType => Context.OutputType;

        private Source? GenerateConstructor(List<Property> collectionProperties)
        {
            Source constructorBody = new Source(Context);
            foreach (Property property in collectionProperties)
                property.GenerateConstructorLinesIfNeeded(constructorBody);

            if (constructorBody.IsEmpty)
                return null;

            Source constructor = new Source(Context);
            constructor.AddLines(
                $"public {DestinationClassName}()",
                "{");
            using (constructor.Indent())
                constructor.AddSource(
                    constructorBody);
            constructor.AddLine(
                "}");

            return constructor;
        }

        private Source GenerateUsingDeclarations(bool hasPropertyDescriptors)
        {
            Source source = new Source(Context);

            var usingNames = new Dictionary<string, NameSyntax>();

            foreach (UsingDirectiveSyntax sourceUsing in _sourceCompilationUnit.Usings)
            {
                NameSyntax sourceUsingName = sourceUsing.Name;
                AddUsing(usingNames, sourceUsingName);

                if (sourceUsingName.ToString().StartsWith("System.StandardUI."))
                    AddUsing(usingNames, Context.ToDestinationNamespaceName(sourceUsingName));
            }

            AddUsing(usingNames, _sourceNamespaceName);

            IEnumerable<QualifiedNameSyntax> outputTypeUsings = OutputType.GetUsings(hasPropertyDescriptors, DestinationTypeHasTypeConverterAttribute());
            foreach (QualifiedNameSyntax outputTypeUsing in outputTypeUsings)
                AddUsing(usingNames, outputTypeUsing);

            foreach (var member in Declaration.Members)
            {
                if (!(member is PropertyDeclarationSyntax modelProperty))
                    continue;

                // Array.Empty requires System
                if (modelProperty.Type is ArrayTypeSyntax)
                    AddUsing(usingNames, IdentifierName("System"));
            }

            if (DestinationTypeHasTypeConverterAttribute())
                AddUsing(usingNames, QualifiedName(OutputType.RootNamespace, IdentifierName("Converters")));

            foreach (NameSyntax name in usingNames.Values)
            {
                source.AddLine($"using {name};");
            }

            return source;
        }

        private Source GenerateExtensionsClassUsingDeclarations()
        {
            Source source = new Source(Context);

            foreach (UsingDirectiveSyntax sourceUsing in _sourceCompilationUnit.Usings)
            {
                source.AddLine($"using {sourceUsing.Name};");
            }

            return source;
        }

        private static void AddUsing(Dictionary<string, NameSyntax> usingNames, NameSyntax name)
        {
            string usingString = name.ToString();
            if (! usingNames.ContainsKey(usingString))
                usingNames.Add(usingString, name);
        }

        private bool DestinationTypeHasTypeConverterAttribute()
        {
            return Context.OutputType is XamlOutputType &&
                   (DestinationClassName == "Geometry" || DestinationClassName == "Brush");
        }

        private string? GetDestinationBaseClass()
        {
            string? elementType = Context.IsCollectionType(Declaration.Identifier.Text);
            if (elementType != null)
                return $"StandardUICollection<{elementType}>";

            TypeSyntax? baseInterface = Declaration.BaseList?.Types.FirstOrDefault()?.Type;

            if (baseInterface == null)
                return OutputType.DefaultBaseClassName;
            else
                return Context.ToDestinationType(baseInterface).ToString();
        }
    }
}
