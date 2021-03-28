using System;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace StandardUI.CodeGenerator
{
    public class Context
    {
        public const string RootNamespace = "System.StandardUI";

        public int IndentSize { get; } = 4;
        public Workspace Workspace { get; }
        public string RootDirectory { get; }
        public OutputType OutputType { get; }

        public Context(Workspace workspace, string rootDirectory, OutputType outputType)
        {
            Workspace = workspace;
            RootDirectory = rootDirectory;
            OutputType = outputType;
        }

        public QualifiedNameSyntax ToDestinationNamespaceName(NameSyntax sourceNamespace)
        {
            string? childNamespaceName = GetChildNamespace(sourceNamespace);

            QualifiedNameSyntax destinationNamespace = OutputType.RootNamespace;

            // Map e.g. System.StandardUI.Media source namespace => System.StandardUI.WPF.Media destination namespace
            // If the source namespace is just System.StandardUI, don't change anything here
            if (childNamespaceName != null)
                destinationNamespace = QualifiedName(destinationNamespace, IdentifierName(childNamespaceName));

            return destinationNamespace;
        }

        public TypeSyntax ToDestinationType(TypeSyntax sourceType)
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

        /// <summary>
        /// Convert the type name, normally starting with an upper case character, to a variable name,
        /// lowercasing the initial character(s) per convention.
        /// 
        /// element => element
        /// Element => element
        /// AElement => aElement
        /// UIElement => uiElement
        /// </summary>
        public string TypeNameToVariableName(string typeName)
        {
            int upperCasePrefixCharCount = 0;
            int length = typeName.Length;
            for (int i = 0; i < length; ++i)
            {
                if (char.IsUpper(typeName[i]))
                    ++upperCasePrefixCharCount;
                else break;
            }

            if (upperCasePrefixCharCount == 0)
                return typeName;
            else if (upperCasePrefixCharCount == 1)
                return typeName.Substring(0, 1).ToLower() + typeName.Substring(1);
            else
                return typeName.Substring(0, upperCasePrefixCharCount - 1).ToLower() + typeName.Substring(upperCasePrefixCharCount - 1);
        }

        public NameSyntax GetIdentifierDestinationType(IdentifierNameSyntax identifierName)
        {
            string typeName = identifierName.Identifier.Text;
            if (IsEnumType(typeName))
                return identifierName;
            else if (IsWrappableType(typeName))
            {
                if (IsWrappedType(typeName))
                    return IdentifierName(typeName + ((XamlOutputType) OutputType).WrapperSuffix);
                else return identifierName;
            }
            else if (IsNonwrappedObjectType(typeName))
                return identifierName;
            else if (typeName == "IUIElement")
                return IdentifierName("StandardUIFrameworkElement");
            else if (typeName.StartsWith("I"))
                return IdentifierName(typeName.Substring(1));
            else
                throw new UserViewableException(
                    $"Identifier type {typeName} isn't supported for model object generation; interface name starting with 'I' is expected");
        }

        public static bool IsTransformType(TypeSyntax type)
        {
            if (type is NullableTypeSyntax nullableType)
                type = nullableType.ElementType;

            return type is IdentifierNameSyntax identifierName && identifierName.Identifier.Text.EndsWith("Transform");
        }

        public bool IsWrappableType(string typeName)
        {
            return typeName == "Color" || typeName == "Point" || typeName == "Points" || typeName == "Size" || typeName == "DataSource" || typeName == "FontWeight";
        }

        public static bool IsPanelSubclass(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            TypeSyntax? baseInterface = interfaceDeclaration.BaseList?.Types.FirstOrDefault()?.Type;
            return baseInterface is IdentifierNameSyntax identifieName && identifieName.Identifier.Text == "IPanel";
        }

        public static bool IncludeOnVisualize(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            TypeSyntax? baseInterface = interfaceDeclaration.BaseList?.Types.FirstOrDefault()?.Type;
            if (baseInterface is IdentifierNameSyntax identifieName && identifieName.Identifier.Text == "IShape")
                return true;

            if (interfaceDeclaration.Identifier.Text == "ITextBlock")
                return true;

            return false;
        }

        public bool IsWrappedType(string typeName)
        {
            return OutputType is XamlOutputType && IsWrappableType(typeName);
        }

        public string GetWrapperTypeName(string typeName)
        {
            return typeName + ((XamlOutputType)OutputType).WrapperSuffix;
        }

        public bool IsNonwrappedObjectType(string typeName)
        {
            return typeName == "LoadedImage" || typeName == "Exception" || typeName == "ImageDecoder" || typeName == "Thickness" || typeName == "CornerRadius";
        }

        public static bool IsEnumType(string typeName)
        {
            return typeName == "SweepDirection" || typeName == "FillRule" || typeName == "GradientSpreadMethod" ||
                   typeName == "BrushMappingMode" || typeName == "PenLineCap" || typeName == "PenLineJoin" || typeName == "LoadingStatus" ||
                   typeName == "TextAlignment" || typeName == "FontStyle" || typeName == "BackgroundSizing";
        }

        public static string? IsCollectionType(TypeSyntax type)
        {
            if (!(type is IdentifierNameSyntax identiferTypeName))
                return null;

            return IsCollectionType(identiferTypeName.Identifier.Text);
        }

        public static string? IsCollectionType(string typeName)
        {
            const string collectionSuffix = "Collection";

            if (typeName.EndsWith(collectionSuffix))
                return typeName.Substring(0, typeName.Length - collectionSuffix.Length);
            else
                return null;
        }

        public ExpressionSyntax GetDefaultValue(SyntaxList<AttributeListSyntax> attributeLists, string propertyName, TypeSyntax sourcePropertyType)
        {
            foreach (AttributeListSyntax attributeList in attributeLists)
            {
                foreach (AttributeSyntax attribute in attributeList.Attributes)
                {
                    if (attribute.Name.ToString() != "DefaultValue")
                        continue;

                    AttributeArgumentSyntax? firstArgument = attribute.ArgumentList?.Arguments.FirstOrDefault();
                    if (firstArgument == null)
                        throw new UserViewableException($"Property {propertyName} should have an argument for the [DefaultValue] attribute");

                    ExpressionSyntax defaultExpression = firstArgument.Expression;
                    if (defaultExpression is LiteralExpressionSyntax literalExpression && literalExpression.Token.IsKind(SyntaxKind.StringLiteralToken))
                    {
                        string literalExpressionString = literalExpression.Token.ToString();
                        if (literalExpressionString == "\"0.5,0.5\"")
                        {
                            bool isWrappedType = sourcePropertyType is IdentifierNameSyntax propertyTypeName &&
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

            if (IsCollectionType(sourcePropertyType) != null)
                return LiteralExpression(SyntaxKind.NullLiteralExpression);
            else if (sourcePropertyType is GenericNameSyntax genericName && genericName.Identifier.Text == "IEnumerable" &&
                genericName.TypeArgumentList.Arguments.Count == 1 &&
                genericName.TypeArgumentList.Arguments[0] is IdentifierNameSyntax elementIdentifierName)
            {
                return
                    LiteralExpression(SyntaxKind.NullLiteralExpression);
            }
            else if ( sourcePropertyType is IdentifierNameSyntax propertyTypeName &&
                (propertyTypeName.Identifier.Text == "Color" ||
                propertyTypeName.Identifier.Text == "Point" ||
                propertyTypeName.Identifier.Text == "Points" ||
                propertyTypeName.Identifier.Text == "Size" ||
                propertyTypeName.Identifier.Text == "Thickness" ||
                propertyTypeName.Identifier.Text == "CornerRadius" ||
                propertyTypeName.Identifier.Text == "FontWeight") )
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
            else if (sourcePropertyType is ArrayTypeSyntax arrayType)
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

            throw new UserViewableException($"Property {propertyName} has no [DefaultValue] attribute nor hardcoded default");
        }

        public string GetSharedOutputDirectory(NameSyntax namespaceName)
        {
            string outputDirectory = Path.Combine(RootDirectory, "src", "StandardUI", "generated");
            string? childNamespace = GetChildNamespace(namespaceName);
            if (childNamespace != null)
                outputDirectory = Path.Combine(outputDirectory, childNamespace);

            return outputDirectory;
        }

        public string GetPlatformOutputDirectory(NameSyntax namespaceName)
        {
            string outputDirectory = Path.Combine(RootDirectory, "src", OutputType.ProjectBaseDirectory, "generated");
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
        public static string? GetChildNamespace(NameSyntax sourceNamespace)
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
