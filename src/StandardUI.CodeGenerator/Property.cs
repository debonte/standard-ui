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
        public string Name { get; }
        public TypeSyntax SourceType { get; }
        public TypeSyntax DestinationType { get; }
        public ExpressionSyntax DefaultValue { get; }

        public Property(Context context, Interface intface, PropertyDeclarationSyntax declaration)
        {
            Context = context;
            Interface = intface;
            Declaration = declaration;
            Name = declaration.Identifier.Text;
            SourceType = declaration.Type.WithoutTrivia();
            DestinationType = context.ToDestinationType(SourceType);
            DefaultValue = context.GetDefaultValue(declaration.AttributeLists, Name, SourceType, DestinationType);
        }

        public void AddDescriptorSource(Source destinationStaticMembers)
        {
            if (!(Context.OutputType is XamlOutputType xamlOutputType))
                return;

            TypeSyntax nonNullablePropertyType;
            if (DestinationType is NullableTypeSyntax nullablePropertyType)
                nonNullablePropertyType = nullablePropertyType.ElementType;
            else nonNullablePropertyType = DestinationType;

            string descriptorName = xamlOutputType.GetPropertyDescriptorName(Name);

            destinationStaticMembers.AddLine(
                $"public static readonly {xamlOutputType.DependencyPropertyClassName} {descriptorName} = PropertyUtils.Create(nameof({Name}), typeof({nonNullablePropertyType}), typeof({DestinationType}), {DefaultValue});");
        }

        public void AddMethodsSource(Source destinationMembers)
        {
            bool hasSetter = Declaration.AccessorList?.Accessors.Any((accessor) => accessor.Kind() == SyntaxKind.SetAccessorDeclaration) ?? false;

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

            destinationMembers.AddBlankLine();
            if (Context.OutputType is XamlOutputType xamlOutputType)
            {
                string descriptorName = xamlOutputType.GetPropertyDescriptorName(Name);

                destinationMembers.AddLine($"public {DestinationType} {Name}");
                destinationMembers.AddLine("{");
                using (var indentRestorer = destinationMembers.Indent())
                {
                    destinationMembers.AddLine($"get => ({DestinationType}) GetValue({descriptorName});");
                    if (hasSetter)
                        destinationMembers.AddLine($"set => SetValue({descriptorName}, value);");
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

                destinationMembers.AddLine($"{SourceType} {Interface.Name}.{Name}");
                destinationMembers.AddLine("{");
                using (var indentRestorer = destinationMembers.Indent())
                {
                    destinationMembers.AddLine($"get => {getterValue};");
                    if (hasSetter)
                        destinationMembers.AddLine($"set => {setterAssignment};");
                }
                destinationMembers.AddLine("}");
            }
        }
    }
}
