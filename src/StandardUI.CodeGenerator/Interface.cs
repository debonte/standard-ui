using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace StandardUI.CodeGenerator
{
    public class Interface
    {
        public static int IndentSize = 4;
        public const string RootNamespace = "Microsoft.StandardUI";

        private readonly Context _context;
        private readonly NameSyntax _sourceNamespaceName;
        private readonly CompilationUnitSyntax _sourceCompilationUnit;
        private readonly QualifiedNameSyntax _destinationNamespaceName;

        public string DestinationClassName { get; }
        public InterfaceDeclarationSyntax Declaration { get; }
        public InterfaceDeclarationSyntax? AttachedInterfaceDeclaration { get; }
        public string Name { get; }

        public Interface(Context context, InterfaceDeclarationSyntax sourceInterfaceDeclaration, InterfaceDeclarationSyntax? sourceAttachedInterfaceDeclaration)
        {
            _context = context;
            Declaration = sourceInterfaceDeclaration;
            AttachedInterfaceDeclaration = sourceAttachedInterfaceDeclaration;

            Name = sourceInterfaceDeclaration.Identifier.Text;
            if (!Name.StartsWith("I"))
                throw new UserViewableException($"Data model interface {Name} must start with 'I'");

            DestinationClassName = Name.Substring(1);

            if (!(Declaration.Parent is NamespaceDeclarationSyntax interfaceNamespaceDeclaration))
                throw new UserViewableException(
                    $"Parent of ${Name} interface should be namespace declaration, but it's a {Declaration.Parent.GetType()} node instead");
            _sourceNamespaceName = interfaceNamespaceDeclaration.Name;

            if (!(interfaceNamespaceDeclaration.Parent is CompilationUnitSyntax compilationUnit))
                throw new UserViewableException(
                    $"Parent of ${interfaceNamespaceDeclaration} namespace should be compilation unit, but it's a {interfaceNamespaceDeclaration.Parent.GetType()} node instead");
            _sourceCompilationUnit = compilationUnit;

            _destinationNamespaceName = _context.ToDestinationNamespaceName(_sourceNamespaceName);
        }

        public void Generate()
        {
            bool hasChildrenProperty = false;

            var descriptors = new Source();
            var staticMethods = new Source();
            var nonstaticMethods = new Source();
            var collectionProperties = new List<PropertyDeclarationSyntax>();
            var attachedClassMethods = new Source();

            // Add the property descriptors and accessors
            foreach (MemberDeclarationSyntax modelObjectMember in Declaration.Members)
            {
                if (!(modelObjectMember is PropertyDeclarationSyntax modelProperty))
                    continue;

                string propertyName = modelProperty.Identifier.Text;
                var property = new Property(_context, this, modelProperty);

#if false
                if (IsCollectionType(modelProperty.Type))
                    collectionProperties.Add(modelProperty);
#endif

                property.AddDescriptorSource(descriptors);
                property.AddMethodsSource(nonstaticMethods);

                if (propertyName == "Children")
                    hasChildrenProperty = true;
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

                    var attachedProperty = new AttachedProperty(_context, this, AttachedInterfaceDeclaration, getterMethodDeclaration, setterMethodDeclaration);

                    attachedProperty.AddMainClassDescriptorSource(descriptors);
                    attachedProperty.AddMainClassMethodsSource(staticMethods);
                    attachedProperty.AddAttachedClassMethodsSource(attachedClassMethods);
                }
            }

            Source usingDeclarations = CreateUsingDeclarationsSource(!descriptors.IsEmpty);

            string? destinationBaseClass = GetDestinationBaseClass();

            Source? constructor = CreateConstructor(collectionProperties);

            string dervivedFrom;
            if (destinationBaseClass == null)
                dervivedFrom = Name;
            else
                dervivedFrom = $"{destinationBaseClass}, {Name}";

            GenerateClassFile(usingDeclarations, DestinationClassName, dervivedFrom, constructor, descriptors, staticMethods, nonstaticMethods);

            if (AttachedInterfaceDeclaration != null)
            {
                string attachedClassDerivedFrom = AttachedInterfaceDeclaration.Identifier.Text;
                GenerateClassFile(usingDeclarations, DestinationClassName + "Attached", attachedClassDerivedFrom, constructor: null, descriptors: null, staticMethods: null, attachedClassMethods);
            }
        }

        public void GenerateClassFile(Source usingDeclarations, string className, string derivedFrom, Source? constructor, Source? descriptors, Source? staticMethods, Source? nonstaticMethods)
        {
            Source fileSource = new Source();

            fileSource.AddLine($"// This file is generated from {Name}.cs. Update the source file to change its contents.");
            fileSource.AddBlankLine();

            fileSource.AddSource(usingDeclarations);
            if (!usingDeclarations.IsEmpty)
                fileSource.AddBlankLine();

            fileSource.AddLine($"namespace {_destinationNamespaceName}");
            fileSource.AddLine("{");

            using (fileSource.Indent())
            {
                fileSource.AddLine($"public class {className} : {derivedFrom}");
                fileSource.AddLine("{");
                using (var indentRestorer = fileSource.Indent())
                {
                    if (descriptors != null)
                        fileSource.AddSource(descriptors);
                    if (staticMethods != null)
                        fileSource.AddSource(staticMethods);
                    if (constructor != null)
                        fileSource.AddSource(constructor);
                    if (nonstaticMethods != null)
                        fileSource.AddSource(nonstaticMethods);
                }
                fileSource.AddLine("}");
            }

            fileSource.AddLine("}");

            string outputDirectory = _context.GetOutputDirectory(_sourceNamespaceName);
            fileSource.WriteToFile(outputDirectory, className + ".cs");
        }

        public OutputType OutputType => _context.OutputType;

        private Source? CreateConstructor(List<PropertyDeclarationSyntax> collectionProperties)
        {
            if (collectionProperties.Count == 0)
                return null;

            Source constructor = new Source();

            constructor.AddLine($"public {DestinationClassName}()");
            constructor.AddLine("{");

            using (constructor.Indent())
			{
                List<StatementSyntax> statements = new List<StatementSyntax>();
                foreach (PropertyDeclarationSyntax property in collectionProperties)
                {
                    string propertyName = property.Identifier.Text;
                    TypeSyntax destinationPropertyType = _context.ToDestinationType(property.Type);

                    constructor.AddLine($"{propertyName} = new {destinationPropertyType}()");
                }
            }

            constructor.AddLine("}");

            return constructor;
        }

        private Source CreateUsingDeclarationsSource(bool hasPropertyDescriptors)
        {
            Source usingDeclarations = new Source();

            var usingNames = new Dictionary<string, NameSyntax>();

            foreach (UsingDirectiveSyntax sourceUsing in _sourceCompilationUnit.Usings)
            {
                NameSyntax sourceUsingName = sourceUsing.Name;
                AddUsing(usingNames, sourceUsingName);

                if (sourceUsingName.ToString().StartsWith("Microsoft.StandardUI."))
                    AddUsing(usingNames, _context.ToDestinationNamespaceName(sourceUsingName));
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

            var usingDirectives = new List<UsingDirectiveSyntax>();
            foreach (NameSyntax name in usingNames.Values)
            {
                usingDeclarations.AddLine($"using {name};");
            }

            return usingDeclarations;
        }

        private static void AddUsing(Dictionary<string, NameSyntax> usingNames, NameSyntax name)
        {
            string usingString = name.ToString();
            if (! usingNames.ContainsKey(usingString))
                usingNames.Add(usingString, name);
        }

        private bool DestinationTypeHasTypeConverterAttribute()
        {
            return _context.OutputType is XamlOutputType &&
                   (DestinationClassName == "Geometry" || DestinationClassName == "Brush");
        }

        private string? GetDestinationBaseClass()
        {
            if (Context.IsCollectionType(Declaration.Identifier.Text, out string elementType))
                return $"StandardUICollection<{elementType}>";

            TypeSyntax? baseInterface = Declaration.BaseList?.Types.FirstOrDefault()?.Type;

            if (baseInterface == null)
            {
                if (Declaration.Identifier.Text == "IUIElement")
                    return OutputType.UIElementBaseClassName;
                else return OutputType.DefaultBaseClassName;
            }
            else
                return _context.ToDestinationType(baseInterface).ToString();
        }
    }
}
