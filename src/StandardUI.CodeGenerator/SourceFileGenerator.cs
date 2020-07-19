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

                if (IsCollectionType(modelProperty.Type, out TypeSyntax _))
                    collectionProperties.Add(modelProperty);

                AddPropertyDescriptor(propertyName, propertyDescriptorName, destinationPropertyType, defaultValue, destinationStaticMembers);
                //AddProperty(modelProperty, propertyName, propertyDescriptorName, sourcePropertyType, destinationPropertyType, destinationMembers);

                if (propertyName == "Children")
                    hasChildrenProperty = true;
            }

            Source? constructor = CreateConstructor(collectionProperties);

            TypeSyntax? baseInterface = _sourceInterfaceDeclaration.BaseList?.Types.FirstOrDefault()?.Type;
            TypeSyntax? destinationBaseClass = GetBaseClass(baseInterface);

            SyntaxNodeOrToken[] baseList;
            if (destinationBaseClass == null)
                baseList = new SyntaxNodeOrToken[] {
                    SimpleBaseType(IdentifierName(_interfaceName))
                };
            else
                baseList = new SyntaxNodeOrToken[] {
                    SimpleBaseType(destinationBaseClass), Token(SyntaxKind.CommaToken),
                    SimpleBaseType(IdentifierName(_interfaceName))
                };

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
            fileSource.AddEmptyLine();

            fileSource.AddSource(usingDeclarations);
            if (!usingDeclarations.IsEmpty)
                fileSource.AddEmptyLine();

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
                $"public static readonly DependencyProperty {propertyDescriptorName} = PropertyUtils.Create(nameof({propertyName}), typeof({nonNullablePropertyType}), typeof({_destinationClassName}), {defaultValue});");
        }

#if LATER
        private void AddProperty(PropertyDeclarationSyntax modelProperty, string propertyName, string propertyDescriptorName,
            TypeSyntax sourcePropertyType, TypeSyntax destinationPropertyType, Source destinationMembers)
        {
            IdentifierNameSyntax propertyDescriptorIdentifier = IdentifierName(propertyDescriptorName);

            bool classPropertyTypeDiffersFromInterface = sourcePropertyType.ToString() != destinationPropertyType.ToString();

            SyntaxTrivia xmlCommentTrivia = modelProperty.GetLeadingTrivia().FirstOrDefault(t =>
                t.Kind() == SyntaxKind.SingleLineDocumentationCommentTrivia ||
                t.Kind() == SyntaxKind.MultiLineDocumentationCommentTrivia);
            bool includeXmlComment = classPropertyTypeDiffersFromInterface && xmlCommentTrivia.Kind() != SyntaxKind.None;

            SyntaxTokenList modifiers;
            if (includeXmlComment)
                modifiers = TokenList(
                    Token(
                        TriviaList(xmlCommentTrivia),
                        SyntaxKind.PublicKeyword,
                        TriviaList()));
            else
                modifiers = TokenList(Token(SyntaxKind.PublicKeyword));

            PropertyDeclarationSyntax propertyDeclaration;
            if (_outputType is XamlOutputType)
            {
                propertyDeclaration = PropertyDeclaration(destinationPropertyType, propertyName)
                    .WithModifiers(modifiers)
                    .WithAccessorList(
                        AccessorList(
                            List(new[]
                            {
                                AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                                    .WithExpressionBody(
                                        ArrowExpressionClause(
                                            CastExpression(
                                                destinationPropertyType,
                                                InvocationExpression(IdentifierName("GetValue"))
                                                    .WithArgumentList(ArgumentList(
                                                        SingletonSeparatedList(
                                                            Argument(propertyDescriptorIdentifier)))))))
                                    .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
                                AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                                    .WithExpressionBody(
                                        ArrowExpressionClause(InvocationExpression(IdentifierName("SetValue"))
                                            .WithArgumentList(ArgumentList(
                                                SeparatedList<ArgumentSyntax>(
                                                    new SyntaxNodeOrToken[]
                                                    {
                                                        Argument(propertyDescriptorIdentifier),
                                                        Token(SyntaxKind.CommaToken),
                                                        Argument(IdentifierName("value"))
                                                    })))))
                                    .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                            })))
                    .NormalizeWhitespace();
            }
            else
            {
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
            }

#if LATER
            //if (!includeXmlComment)
            propertyDeclaration = propertyDeclaration.WithLeadingTrivia(
                    TriviaList(propertyDeclaration.GetLeadingTrivia()
                        .Insert(0, CarriageReturnLineFeed)
                        .Insert(0, CarriageReturnLineFeed)));
#endif

            destinationMembers.Add(propertyDeclaration);

            // If the interface property has a different type, add another property that explicitly implements it
            if (classPropertyTypeDiffersFromInterface)
            {
                ExpressionSyntax arrowRightHandSide;
                if (sourcePropertyType is IdentifierNameSyntax identifierName &&
                    IsWrappedType(identifierName.Identifier.Text))
                {
                    string wrapperTypeName = identifierName.Identifier.Text;

                    arrowRightHandSide =
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(propertyName),
                            IdentifierName($"Wrapped{wrapperTypeName}"));
                }
                else arrowRightHandSide = IdentifierName(propertyName);

                PropertyDeclarationSyntax explicitInterfaceProperty =
                    PropertyDeclaration(sourcePropertyType, propertyName)
                        .WithExplicitInterfaceSpecifier(
                            ExplicitInterfaceSpecifier(IdentifierName(_sourceInterfaceDeclaration.Identifier)))
                        .WithExpressionBody(
                            ArrowExpressionClause(arrowRightHandSide))
                        .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));

                destinationMembers.Add(explicitInterfaceProperty);
            }
        }
#endif

        private Source CreateUsingDeclarations(bool hasPropertyDescriptors)
        {
            Source usingDeclarations = new Source();

            var usingNames = new Dictionary<string, NameSyntax>();

            foreach (UsingDirectiveSyntax sourceUsing in _sourceCompilationUnit.Usings)
            {
                NameSyntax sourceUsingName = sourceUsing.Name;
                AddUsing(usingNames, sourceUsingName);

                if (sourceUsingName.ToString().StartsWith("StandardUI."))
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

        private QualifiedNameSyntax ToDestinationNamespaceName(NameSyntax sourceNamespaceName)
        {
            if (!sourceNamespaceName.ToString().StartsWith("Microsoft.StandardUI"))
                throw new InvalidOperationException($"Source namespace {sourceNamespaceName} doesn't start with 'Microsoft.StandardUI.' as expected");

            QualifiedNameSyntax destinationNamespaceName = _outputType.RootNamespace;

            // Map e.g. Microsoft.StandardUI.Media source namespace => Microsoft.StandardUI.WPF.Media destination namespace
            // If the source namespace is just Microsoft.StandardUI, don't change anything here
            if (((QualifiedNameSyntax)sourceNamespaceName).Left is QualifiedNameSyntax qualifiedName)
                destinationNamespaceName = QualifiedName(destinationNamespaceName, qualifiedName.Right);

            return destinationNamespaceName;
        }

        private TypeSyntax ToDestinationType(TypeSyntax sourceType)
        {
            if (IsCollectionType(sourceType, out TypeSyntax elementType))
            {
                TypeSyntax elementDestinationType = ToDestinationType(elementType);

                return GenericName("XGraphicsCollection")
                    .WithTypeArgumentList(
                        TypeArgumentList(
                            SingletonSeparatedList(elementDestinationType)));
            }
            else if (sourceType is IdentifierNameSyntax identifierName)
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
                    return QualifiedName(IdentifierName("Wrapper"), IdentifierName(typeName));
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

        private bool IsNonwrappedObjectType(string typeName)
        {
            return typeName == "LoadedImage" || typeName == "Exception" || typeName == "ImageDecoder";
        }

        private static bool IsEnumType(string typeName)
        {
            return typeName == "SweepDirection" || typeName == "FillRule" || typeName == "GradientSpreadMethod" ||
                   typeName == "BrushMappingMode" || typeName == "PenLineCap" || typeName == "PenLineJoin" || typeName == "LoadingStatus";
        }

        private static bool IsCollectionType(TypeSyntax type, out TypeSyntax elementType)
        {
            elementType = IdentifierName("INVALID");
            if (!(type is GenericNameSyntax genericName))
                return false;

            if (genericName.Identifier.Text != "IEnumerable")
                return false;

            if (!(genericName.TypeArgumentList.Arguments.Count == 1 &&
                  genericName.TypeArgumentList.Arguments[0] is IdentifierNameSyntax elementIdentifierName))
                throw new InvalidOperationException($"Type {genericName} doesn't have a single identifier generic argument as expected");

            elementType = elementIdentifierName;
            return true;
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

                            if (isWrappedType)
                                defaultExpression =
                                    MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            IdentifierName("Wrapper"),
                                            IdentifierName("Point")),
                                        IdentifierName("CenterDefault"));
                            else
                                defaultExpression = MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    IdentifierName("Point"),
                                    IdentifierName("CenterDefault"));
                        }
                        else throw new UserViewableException($"Unknown string literal based default value: {literalExpressionString}");
                    }

                    return defaultExpression;
                }
            }

            TypeSyntax propertyType = modelProperty.Type;

            if (propertyType is GenericNameSyntax genericName && genericName.Identifier.Text == "IEnumerable" &&
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
                    typeExpression = MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName("Wrapper"),
                        propertyTypeName);
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

            throw new UserViewableException($"Property {modelProperty.Identifier.Text} has no [lDefaultValue] attribute nor hardcoded default");
        }

        private TypeSyntax? GetBaseClass(TypeSyntax? baseInterface)
        {
            if (baseInterface == null)
                return _outputType.BaseClassName;
            else
                return ToDestinationType(baseInterface);
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
        /// <param name="namespaceName">source namespace</param>
        /// <returns>child namespace</returns>
        private static string? GetChildNamespace(NameSyntax namespaceName)
        {
            string namespaceNameString = namespaceName.ToString();

            int periodIndex = namespaceNameString.IndexOf('.');
            if (periodIndex == -1)
                return null;
            else return namespaceNameString.Substring(periodIndex + 1);
        }
    }
}
