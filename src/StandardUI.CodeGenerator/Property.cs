using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace StandardUI.CodeGenerator
{
    public class Property
    {
        public Context Context { get; }
        public Interface Interface { get; }
        public PropertyDeclarationSyntax Declaration { get; }
        public bool HasSetter { get; }
        public string Name { get; }
        public TypeSyntax SourceType { get; }
        public TypeSyntax DestinationType { get; }
        public ExpressionSyntax DefaultValue { get; }

        public Property(Context context, Interface intface, PropertyDeclarationSyntax declaration)
        {
            Context = context;
            Interface = intface;
            Declaration = declaration;
            HasSetter = declaration.AccessorList?.Accessors.Any((accessor) => accessor.Kind() == SyntaxKind.SetAccessorDeclaration) ?? false;
            Name = declaration.Identifier.Text;
            SourceType = declaration.Type.WithoutTrivia();
            DestinationType = context.ToDestinationType(SourceType);
            DefaultValue = context.GetDefaultValue(declaration.AttributeLists, Name, SourceType);
        }

        public void GenerateDescriptor(Source destinationStaticMembers)
        {
            if (!(Context.OutputType is XamlOutputType xamlOutputType))
                return;

            TypeSyntax nonNullablePropertyType;
            if (DestinationType is NullableTypeSyntax nullablePropertyType)
                nonNullablePropertyType = nullablePropertyType.ElementType;
            else nonNullablePropertyType = DestinationType;

            string descriptorName = xamlOutputType.GetPropertyDescriptorName(Name);

            destinationStaticMembers.AddLine(
                $"public static readonly {xamlOutputType.DependencyPropertyClassName} {descriptorName} = PropertyUtils.Register(nameof({Name}), typeof({nonNullablePropertyType}), typeof({Interface.DestinationClassName}), {DefaultValue});");
        }

        public void GenerateMethods(Source source)
        {
            bool classPropertyTypeDiffersFromInterface = SourceType.ToString() != DestinationType.ToString();

            SyntaxTrivia xmlCommentTrivia = Declaration.GetLeadingTrivia().FirstOrDefault(t =>
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

            source.AddBlankLineIfNonempty();
            if (Context.OutputType is XamlOutputType xamlOutputType)
            {
                string descriptorName = xamlOutputType.GetPropertyDescriptorName(Name);

                source.AddLines(
                    $"public {DestinationType} {Name}",
                    "{");
                using (source.Indent())
                {
                    source.AddLine(
                        $"get => ({DestinationType}) GetValue({descriptorName});");
                    if (HasSetter)
                        source.AddLine(
                            $"set => SetValue({descriptorName}, value);");
                }
                source.AddLine(
                    "}");
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
                if (SourceType is IdentifierNameSyntax identifierName && Context.IsWrappedType(identifierName.Identifier.Text))
                {
                    string wrapperTypeName = identifierName.Identifier.Text;
                    getterValue = $"{Name}.{SourceType}";
                    setterAssignment = $"{Name} = new {DestinationType}(value)";
                }
                else
                {
                    getterValue = Name;
                    setterAssignment = $"{Name} = ({DestinationType}) value";
                }

                source.AddLines(
                    $"{SourceType} {Interface.Name}.{Name}",
                    "{");
                using (source.Indent())
                {
                    source.AddLine(
                        $"get => {getterValue};");
                    if (HasSetter)
                        source.AddLine(
                            $"set => {setterAssignment};");
                }
                source.AddLine(
                    "}");
            }
        }

        public void GenerateExtensionClassMethods(Source source)
        {
            if (!HasSetter)
                return;

            string interfaceVariableName = Interface.VariableName;

            source.AddBlankLineIfNonempty();
            source.AddLines(
                $"public static T {Name}<T>(this T {interfaceVariableName}, {SourceType} value) where T : {Interface.Name}",
                "{");
            using (source.Indent())
            {
                source.AddLines(
                    $"{interfaceVariableName}.{Name} = value;",
                    $"return {interfaceVariableName};");
            }
            source.AddLine(
                "}");
        }
    }
}
