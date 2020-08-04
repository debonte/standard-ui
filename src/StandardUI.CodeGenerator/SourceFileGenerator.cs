using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace StandardUI.CodeGenerator
{
	public class SourceFileGenerator
    {
        public static int IndentSize = 4;
        public const string RootNamespace = "Microsoft.StandardUI";

        private readonly Workspace _workspace;
        private readonly InterfaceDeclarationSyntax _sourceInterfaceDeclaration;
        private readonly string _rootDirectory;
        private readonly OutputType _outputType;
        private readonly string _interfaceName;
        private readonly IdentifierNameSyntax _destinationClassName;
        private readonly NameSyntax _sourceNamespaceName;
        private readonly CompilationUnitSyntax _sourceCompilationUnit;
        private readonly QualifiedNameSyntax _destinationNamespaceName;

        public SourceFileGenerator(Workspace workspace, InterfaceDeclarationSyntax sourceInterfaceDeclaration, string rootDirectory, OutputType outputType)
        {
            _workspace = workspace;
            _sourceInterfaceDeclaration = sourceInterfaceDeclaration;
            _rootDirectory = rootDirectory;
            _outputType = outputType;

            _interfaceName = _sourceInterfaceDeclaration.Identifier.Text;
            if (!_interfaceName.StartsWith("I"))
                throw new UserViewableException($"Data model interface {_interfaceName} must start with 'I'");

            _destinationClassName = IdentifierName(_interfaceName.Substring(1));

            if (!(_sourceInterfaceDeclaration.Parent is NamespaceDeclarationSyntax interfaceNamespaceDeclaration))
                throw new UserViewableException(
                    $"Parent of ${_interfaceName} interface should be namespace declaration, but it's a {_sourceInterfaceDeclaration.Parent.GetType()} node instead");
            _sourceNamespaceName = interfaceNamespaceDeclaration.Name;

            if (!(interfaceNamespaceDeclaration.Parent is CompilationUnitSyntax compilationUnit))
                throw new UserViewableException(
                    $"Parent of ${interfaceNamespaceDeclaration} namespace should be compilation unit, but it's a {interfaceNamespaceDeclaration.Parent.GetType()} node instead");
            _sourceCompilationUnit = compilationUnit;

            _destinationNamespaceName = ToDestinationNamespaceName(_sourceNamespaceName);
        }

        public void Generate()
        {
            bool hasChildrenProperty = false;

            var destinationStaticMembers = new Source();
            var destinationMembers = new Source();
            var collectionProperties = new List<PropertyDeclarationSyntax>();
            foreach (MemberDeclarationSyntax modelObjectMember in _sourceInterfaceDeclaration.Members)
            {
                if (!(modelObjectMember is PropertyDeclarationSyntax modelProperty))
                    continue;

                string propertyName = modelProperty.Identifier.Text;
                string propertyDescriptorName = propertyName + "Property";
                TypeSyntax sourcePropertyType = modelProperty.Type.WithoutTrivia();
                TypeSyntax destinationPropertyType = ToDestinationType(sourcePropertyType);
                ExpressionSyntax defaultValue = GetDefaultValue(modelProperty, destinationPropertyType);

#if false
                if (IsCollectionType(modelProperty.Type))
                    collectionProperties.Add(modelProperty);
#endif

                AddPropertyDescriptor(propertyName, propertyDescriptorName, destinationPropertyType, defaultValue, destinationStaticMembers);
                AddProperty(modelProperty, propertyName, propertyDescriptorName, sourcePropertyType, destinationPropertyType, destinationMembers);

                if (propertyName == "Children")
                    hasChildrenProperty = true;
            }

            Source? constructor = CreateConstructor(collectionProperties);

            string? destinationBaseClass = GetDestinationBaseClass();

            Source classSource = new Source();

            string dervivedFrom;
            if (destinationBaseClass == null)
                dervivedFrom = _interfaceName;
            else
                dervivedFrom = $"{destinationBaseClass}, {_interfaceName}";
            classSource.AddLine($"public class {_destinationClassName.Identifier} : {dervivedFrom}");
            classSource.AddLine("{");
            using (var indentRestorer = classSource.Indent())
			{
                classSource.AddSource(destinationStaticMembers);
                if (constructor != null)
                    classSource.AddSource(constructor);
                classSource.AddSource(destinationMembers);
            }
            classSource.AddLine("}");

            // TODO: Add this
#if false
            if (DestinationTypeHasTypeConverterAttribute())
            {
                classDeclaration =
                    classDeclaration.WithAttributeLists(
                        SingletonList(
                            AttributeList(
                                SingletonSeparatedList(
                                    Attribute(
                                            IdentifierName("TypeConverter"))
                                        .WithArgumentList(
                                            AttributeArgumentList(
                                                SingletonSeparatedList(
                                                    AttributeArgument(
                                                        TypeOfExpression(
                                                            IdentifierName($"{_destinationClassName.Identifier}TypeConverter"))))))))));
            }

            // Add the [ContentProperty("Children")] attribute, if needed
            if (hasChildrenProperty && _outputType is XamlOutputType)
                classDeclaration = classDeclaration
                    .WithAttributeLists(
                        SingletonList(
                            AttributeList(
                                SingletonSeparatedList(
                                    Attribute(
                                            IdentifierName("ContentProperty"))
                                        .WithArgumentList(
                                            AttributeArgumentList(
                                                SingletonSeparatedList(
                                                    AttributeArgument(
                                                        LiteralExpression(
                                                            SyntaxKind.StringLiteralExpression,
                                                            Literal("Children"))))))))));
#endif

#if false
            var usings = new List<UsingDirectiveSyntax>
            {
                UsingDirective(_interfaceNamespaceName)
                    .WithUsingKeyword(
                        Token(
                            TriviaList(
                                Comment($"// This file is generated from {_interfaceName}.cs. Update the source file to change its contents."),
                                CarriageReturnLineFeed),
                            SyntaxKind.UsingKeyword,
                            TriviaList())),
                UsingDirective(QualifiedName(IdentifierName("System"), IdentifierName("Windows")))
            };
            if (addCollectionsUsing)
            {
                usings.Add(UsingDirective(QualifiedName(
                    QualifiedName(IdentifierName("System"), IdentifierName("Collections")),
                    IdentifierName("Generic"))));
            }
            if (addTransformsUsing)
            {
                usings.Add(UsingDirective(QualifiedName(IdentifierName("XGraphics"), IdentifierName("Transforms"))));
                // This will be, for example, XGraphics.WPF.Transforms
                usings.Add(UsingDirective(QualifiedName(_destinationNamespaceName, IdentifierName("Transforms"))));
            }
            if (hasChildrenProperty)
            {
                usings.Add(UsingDirective(QualifiedName(
                    QualifiedName(IdentifierName("System"), IdentifierName("Windows")),
                    IdentifierName("Markup"))));
            }
#endif
            Source usingDeclarations = CreateUsingDeclarations(! destinationStaticMembers.IsEmpty);

            Source fileSource = new Source();

            fileSource.AddLine($"// This file is generated from {_interfaceName}.cs. Update the source file to change its contents.");
            fileSource.AddBlankLine();

            fileSource.AddSource(usingDeclarations);
            if (!usingDeclarations.IsEmpty)
                fileSource.AddBlankLine();

            fileSource.AddLine($"namespace {_destinationNamespaceName}");
            fileSource.AddLine("{");

            using (fileSource.Indent())
			{
                fileSource.AddSource(classSource);
			}

            fileSource.AddLine("}");

            string outputDirectory = GetOutputDirectory(_sourceNamespaceName);
            fileSource.WriteToFile(outputDirectory, _destinationClassName + ".cs");
        }

        private Source? CreateConstructor(List<PropertyDeclarationSyntax> collectionProperties)
        {
            if (collectionProperties.Count == 0)
                return null;

            Source constructor = new Source();

            constructor.AddLine($"public {_destinationClassName.Identifier}()");
            constructor.AddLine("{");

            using (constructor.Indent())
			{
                List<StatementSyntax> statements = new List<StatementSyntax>();
                foreach (PropertyDeclarationSyntax property in collectionProperties)
                {
                    string propertyName = property.Identifier.Text;
                    TypeSyntax destinationPropertyType = ToDestinationType(property.Type);

                    constructor.AddLine($"{propertyName} = new {destinationPropertyType}()");
                }
            }

            constructor.AddLine("}");

            return constructor;
        }

        private void AddPropertyDescriptor(string propertyName, string propertyDescriptorName, TypeSyntax propertyType,
            ExpressionSyntax defaultValue, Source destinationStaticMembers)
        {
            if (!(_outputType is XamlOutputType xamlOutputType))
                return;

            TypeSyntax nonNullablePropertyType;
            if (propertyType is NullableTypeSyntax nullablePropertyType)
                nonNullablePropertyType = nullablePropertyType.ElementType;
            else nonNullablePropertyType = propertyType;

            destinationStaticMembers.AddLine(
                $"public static readonly System.Windows.DependencyProperty {propertyDescriptorName} = PropertyUtils.Create(nameof({propertyName}), typeof({nonNullablePropertyType}), typeof({_destinationClassName}), {defaultValue});");
        }

        private void AddProperty(PropertyDeclarationSyntax modelProperty, string propertyName, string propertyDescriptorName,
            TypeSyntax sourcePropertyType, TypeSyntax destinationPropertyType, Source destinationMembers)
        {
            IdentifierNameSyntax propertyDescriptorIdentifier = IdentifierName(propertyDescriptorName);
            bool hasSetter = modelProperty.AccessorList?.Accessors.Any((accessor) => accessor.Kind() == SyntaxKind.SetAccessorDeclaration) ?? false;

            bool classPropertyTypeDiffersFromInterface = sourcePropertyType.ToString() != destinationPropertyType.ToString();

            SyntaxTrivia xmlCommentTrivia = modelProperty.GetLeadingTrivia().FirstOrDefault(t =>
                t.Kind() == SyntaxKind.SingleLineDocumentationCommentTrivia ||
                t.Kind() == SyntaxKind.MultiLineDocumentationCommentTrivia);
            bool includeXmlComment = classPropertyTypeDiffersFromInterface && xmlCommentTrivia.Kind() != SyntaxKind.None;

#if LATER
            SyntaxTokenList modifiers;
            if (includeXmlComment)
                modifiers = TokenList(
                    Token(
                        TriviaList(xmlCommentTrivia),
                        SyntaxKind.PublicKeyword,
                        TriviaList()));
            else
                modifiers = TokenList(Token(SyntaxKind.PublicKeyword));
#endif

            destinationMembers.AddBlankLine();
            if (_outputType is XamlOutputType)
            {
                destinationMembers.AddLine($"public {destinationPropertyType} {propertyName}");
                destinationMembers.AddLine("{");
                using (var indentRestorer = destinationMembers.Indent()) {
                    destinationMembers.AddLine($"get => ({destinationPropertyType}) GetValue({propertyName}Property);");
                    if (hasSetter)
                        destinationMembers.AddLine($"set => SetValue({propertyName}Property, value);");
                }
                destinationMembers.AddLine("}");
            }
            else
            {
#if LATER
                ExpressionSyntax defaultValue = GetDefaultValue(modelProperty, destinationPropertyType);

                propertyDeclaration = PropertyDeclaration(destinationPropertyType, propertyName)
                    .WithModifiers(modifiers)
                    .WithAccessorList(
                        AccessorList(
                            List(new[]
                            {
                                AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                                    .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
                                AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                                    .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                            })))
                    .WithInitializer(
                        EqualsValueClause(defaultValue))
                    .WithSemicolonToken(
                        Token(SyntaxKind.SemicolonToken));
#endif
            }

#if LATER
            //if (!includeXmlComment)
            propertyDeclaration = propertyDeclaration.WithLeadingTrivia(
                    TriviaList(propertyDeclaration.GetLeadingTrivia()
                        .Insert(0, CarriageReturnLineFeed)
                        .Insert(0, CarriageReturnLineFeed)));
#endif

            // If the interface property has a different type, add another property that explicitly implements it
            if (classPropertyTypeDiffersFromInterface)
            {
                string getterValue;
                string setterAssignment;
                if (sourcePropertyType is IdentifierNameSyntax identifierName && IsWrappedType(identifierName.Identifier.Text))
                {
                    string wrapperTypeName = identifierName.Identifier.Text;
                    getterValue = $"{propertyName}.{sourcePropertyType}";
                    setterAssignment = $"{propertyName} = new {destinationPropertyType}(value)";
                }
                else
                {
                    getterValue = propertyName;
                    setterAssignment = $"{propertyName} = ({destinationPropertyType}) value";
                }

                destinationMembers.AddLine($"{sourcePropertyType} {_interfaceName}.{propertyName}");
                destinationMembers.AddLine("{");
                using (var indentRestorer = destinationMembers.Indent()) {
                    destinationMembers.AddLine($"get => {getterValue};");
                    if (hasSetter)
                        destinationMembers.AddLine($"set => {setterAssignment};");
                }
                destinationMembers.AddLine("}");
            }
        }

        private Source CreateUsingDeclarations(bool hasPropertyDescriptors)
        {
            Source usingDeclarations = new Source();

            var usingNames = new Dictionary<string, NameSyntax>();

            foreach (UsingDirectiveSyntax sourceUsing in _sourceCompilationUnit.Usings)
            {
                NameSyntax sourceUsingName = sourceUsing.Name;
                AddUsing(usingNames, sourceUsingName);

                if (sourceUsingName.ToString().StartsWith("Microsoft.StandardUI."))
                    AddUsing(usingNames, ToDestinationNamespaceName(sourceUsingName));
            }

            AddUsing(usingNames, _sourceNamespaceName);

            IEnumerable<QualifiedNameSyntax> outputTypeUsings = _outputType.GetUsings(hasPropertyDescriptors, DestinationTypeHasTypeConverterAttribute());
            foreach (QualifiedNameSyntax outputTypeUsing in outputTypeUsings)
                AddUsing(usingNames, outputTypeUsing);

            foreach (var member in _sourceInterfaceDeclaration.Members)
            {
                if (!(member is PropertyDeclarationSyntax modelProperty))
                    continue;

                // Array.Empty requires System
                if (modelProperty.Type is ArrayTypeSyntax)
                    AddUsing(usingNames, IdentifierName("System"));
            }

            if (DestinationTypeHasTypeConverterAttribute())
                AddUsing(usingNames, QualifiedName(_outputType.RootNamespace, IdentifierName("Converters")));

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

        private QualifiedNameSyntax ToDestinationNamespaceName(NameSyntax sourceNamespace)
        {
            string? childNamespaceName = GetChildNamespace(sourceNamespace);

            QualifiedNameSyntax destinationNamespace = _outputType.RootNamespace;

            // Map e.g. Microsoft.StandardUI.Media source namespace => Microsoft.StandardUI.WPF.Media destination namespace
            // If the source namespace is just Microsoft.StandardUI, don't change anything here
            if (childNamespaceName != null)
                destinationNamespace = QualifiedName(destinationNamespace, IdentifierName(childNamespaceName));

            return destinationNamespace;
        }

        private TypeSyntax ToDestinationType(TypeSyntax sourceType)
        {
            if (sourceType is IdentifierNameSyntax identifierName)
                return GetIdentifierDestinationType(identifierName);
            else if (sourceType is NullableTypeSyntax nullableType &&
                     nullableType.ElementType is IdentifierNameSyntax nullableIdentifierName)
                return NullableType(GetIdentifierDestinationType(nullableIdentifierName));
            else if (sourceType is PredefinedTypeSyntax predefinedType)
                return predefinedType;
            else if (sourceType is GenericNameSyntax genericName)
                return genericName;
            else if (sourceType is NameSyntax name)
                return name;
            else if (sourceType is ArrayTypeSyntax arrayType)
                return arrayType;
            /*
                PropertyDeclaration(
                    GenericName(
                        Identifier("IEnumerable"))
                    .WithTypeArgumentList(
                        TypeArgumentList(
                            SingletonSeparatedList<TypeSyntax>(
                                IdentifierName("IGraphicsElement")))),
                    Identifier("Children"))             */
            else
                throw new UserViewableException(
                    $"Type {sourceType.GetType()} isn't supported for model object generation");
        }

        private NameSyntax GetIdentifierDestinationType(IdentifierNameSyntax identifierName)
        {
            string typeName = identifierName.Identifier.Text;
            if (IsEnumType(typeName))
                return identifierName;
            else if (IsWrappableType(typeName))
            {
                if (IsWrappedType(typeName))
                    return IdentifierName(typeName + ((XamlOutputType) _outputType).WrapperSuffix);
                else return identifierName;
            }
            else if (IsNonwrappedObjectType(typeName))
                return identifierName;
            else if (typeName.StartsWith("I"))
                return IdentifierName(typeName.Substring(1));
            else
                throw new UserViewableException(
                    $"Identifier type {typeName} isn't supported for model object generation; interface name starting with 'I' is expected");
        }

        private bool DestinationTypeHasTypeConverterAttribute()
        {
            string destinationTypeName = _destinationClassName.Identifier.Text;

            return _outputType is XamlOutputType &&
                   (destinationTypeName == "Geometry" || destinationTypeName == "Brush");
        }

        private static bool IsTransformType(TypeSyntax type)
        {
            if (type is NullableTypeSyntax nullableType)
                type = nullableType.ElementType;

            return type is IdentifierNameSyntax identifierName && identifierName.Identifier.Text.EndsWith("Transform");
        }

        private bool IsWrappableType(string typeName)
        {
            return typeName == "Color" || typeName == "Point" || typeName == "Points" || typeName == "Size" || typeName == "DataSource";
        }

        private bool IsWrappedType(string typeName)
        {
            return _outputType is XamlOutputType && IsWrappableType(typeName);
        }

        public string GetWrapperTypeName(string typeName)
        {
            return typeName + ((XamlOutputType)_outputType).WrapperSuffix;
        }

        private bool IsNonwrappedObjectType(string typeName)
        {
            return typeName == "LoadedImage" || typeName == "Exception" || typeName == "ImageDecoder";
        }

        private static bool IsEnumType(string typeName)
        {
            return typeName == "SweepDirection" || typeName == "FillRule" || typeName == "GradientSpreadMethod" ||
                   typeName == "BrushMappingMode" || typeName == "PenLineCap" || typeName == "PenLineJoin" || typeName == "LoadingStatus";
        }

        private static bool IsCollectionType(TypeSyntax type, out string elementType)
        {
            if (!(type is IdentifierNameSyntax identiferTypeName))
            {
                elementType = "";
                return false;
            }

            return IsCollectionType(identiferTypeName.Identifier.Text, out elementType);
        }

        private static bool IsCollectionType(string typeName, out string elementType)
        {
            const string collectionSuffix = "Collection";

            if (typeName.EndsWith(collectionSuffix))
            {
                elementType = typeName.Substring(0, typeName.Length - collectionSuffix.Length);
                return true;
            }
            else
            {
                elementType = "";
                return false;
            }
        }

        private ExpressionSyntax GetDefaultValue(PropertyDeclarationSyntax modelProperty, TypeSyntax destinationPropertyType)
        {
            foreach (AttributeListSyntax attributeList in modelProperty.AttributeLists)
            {
                foreach (AttributeSyntax attribute in attributeList.Attributes)
                {
                    if (attribute.Name.ToString() != "DefaultValue")
                        continue;

                    AttributeArgumentSyntax? firstArgument = attribute.ArgumentList?.Arguments.FirstOrDefault();
                    if (firstArgument == null)
                        throw new UserViewableException($"Property {modelProperty.Identifier.Text} should have an argument for the [DefaultValue] attribute");

                    ExpressionSyntax defaultExpression = firstArgument.Expression;
                    if (defaultExpression is LiteralExpressionSyntax literalExpression && literalExpression.Token.IsKind(SyntaxKind.StringLiteralToken))
                    {
                        string literalExpressionString = literalExpression.Token.ToString();
                        if (literalExpressionString == "\"0.5,0.5\"")
                        {
                            bool isWrappedType = modelProperty.Type is IdentifierNameSyntax propertyTypeName &&
                                             IsWrappedType(propertyTypeName.Identifier.Text);

                            defaultExpression =
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    IdentifierName(isWrappedType ? GetWrapperTypeName("Point") : "Point"),
                                    IdentifierName("CenterDefault"));
                        }
                        else if (literalExpressionString != "\"\"")
                            throw new UserViewableException($"Unknown string literal based default value: {literalExpressionString}");
                    }

                    return defaultExpression;
                }
            }

            TypeSyntax propertyType = modelProperty.Type;

            if (IsCollectionType(propertyType, out var _))
            {
                return
                    LiteralExpression(SyntaxKind.NullLiteralExpression);
            }
            else if (propertyType is GenericNameSyntax genericName && genericName.Identifier.Text == "IEnumerable" &&
                genericName.TypeArgumentList.Arguments.Count == 1 &&
                genericName.TypeArgumentList.Arguments[0] is IdentifierNameSyntax elementIdentifierName)
            {
                return
                    LiteralExpression(SyntaxKind.NullLiteralExpression);
            }
            else if (propertyType is IdentifierNameSyntax propertyTypeName && (propertyTypeName.Identifier.Text == "Color" ||
                                                                               propertyTypeName.Identifier.Text == "Point" ||
                                                                               propertyTypeName.Identifier.Text == "Points" ||
                                                                               propertyTypeName.Identifier.Text == "Size"))
            {
                // WithoutTrivia is needed here to remove any comment before the type, so the comment isn't written to the output
                propertyTypeName = propertyTypeName.WithoutTrivia();

                ExpressionSyntax typeExpression;
                if (IsWrappedType(propertyTypeName.Identifier.Text))
                    typeExpression = IdentifierName(GetWrapperTypeName(propertyTypeName.Identifier.Text));
                else typeExpression = propertyTypeName;

                return
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        typeExpression,
                        IdentifierName("Default"));
            }
            else if (propertyType is ArrayTypeSyntax arrayType)
            {
                return
                    InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName("Array"),
                            GenericName(
                                    Identifier("Empty"))
                                .WithTypeArgumentList(
                                    TypeArgumentList(
                                        SingletonSeparatedList<TypeSyntax>(arrayType.ElementType)))));
            }

            throw new UserViewableException($"Property {modelProperty.Identifier.Text} has no [DefaultValue] attribute nor hardcoded default");
        }

        private string? GetDestinationBaseClass()
        {
            if (IsCollectionType(_sourceInterfaceDeclaration.Identifier.Text, out string elementType))
                return $"StandardUICollection<{elementType}>";

            TypeSyntax? baseInterface = _sourceInterfaceDeclaration.BaseList?.Types.FirstOrDefault()?.Type;

            if (baseInterface == null)
            {
                if (_sourceInterfaceDeclaration.Identifier.Text == "IUIElement")
                    return _outputType.UIElementBaseClassName;
                else return _outputType.DefaultBaseClassName;
            }
            else
                return ToDestinationType(baseInterface).ToString();
        }

        private string GetOutputDirectory(NameSyntax namespaceName)
        {
            string outputDirectory = Path.Combine(_rootDirectory, "src", _outputType.ProjectBaseDirectory);
            string? childNamespace = GetChildNamespace(namespaceName);
            if (childNamespace != null)
                outputDirectory = Path.Combine(outputDirectory, childNamespace);

            return outputDirectory;
        }

        /// <summary>
        /// Return the child namespace (e.g. "Shapes", "Transforms", etc. or null if there is no child
        /// and classes should be at the root.
        /// </summary>
        /// <param name="sourceNamespace">source namespace</param>
        /// <returns>child namespace</returns>
        private static string? GetChildNamespace(NameSyntax sourceNamespace)
        {
            string sourceNamespaceString = sourceNamespace.ToString();

            if (!sourceNamespaceString.StartsWith(RootNamespace))
                throw new InvalidOperationException($"Source namespace {sourceNamespace} doesn't start with '{RootNamespace}' as expected");

            if (!sourceNamespaceString.StartsWith(RootNamespace + "."))
                return null;

            return sourceNamespaceString.Substring(sourceNamespaceString.LastIndexOf('.') + 1);
        }
    }
}
