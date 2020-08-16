using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace StandardUI.CodeGenerator
{
    public class AttachedProperty
    {
        public Context Context { get; }
        public Interface Interface { get; }
        public InterfaceDeclarationSyntax AttachedInterfaceDeclaration { get; }
        public MethodDeclarationSyntax GetterDeclaration { get; }
        public MethodDeclarationSyntax? SetterDeclaration { get; }
        public string Name { get; }
        public TypeSyntax SourceType { get; }
        public TypeSyntax DestinationType { get; }
        public TypeSyntax TargetSourceType { get; }
        public TypeSyntax TargetDestinationType { get; }
        public string? TargetParameterName { get; }
        public ExpressionSyntax DefaultValue { get; }

        public AttachedProperty(Context context, Interface intface, InterfaceDeclarationSyntax attachedInterfaceDeclaration, MethodDeclarationSyntax getterDeclaration, MethodDeclarationSyntax? setterDeclaration)
        {
            string getterMethodName = getterDeclaration.Identifier.Text;
            string propertyName = getterMethodName.Substring("Get".Length);

            if (getterDeclaration.ParameterList.Parameters.Count != 1)
                throw new UserViewableException(
                        $"Attached type getter method {attachedInterfaceDeclaration.Identifier.Text}.{getterMethodName} doesn't take a single parameter");
            ParameterSyntax targetParameter = getterDeclaration.ParameterList.Parameters.FirstOrDefault()!;

            Context = context;
            Interface = intface;
            AttachedInterfaceDeclaration = attachedInterfaceDeclaration;
            GetterDeclaration = getterDeclaration;
            SetterDeclaration = setterDeclaration;
            Name = propertyName;
            SourceType = getterDeclaration.ReturnType;
            DestinationType = context.ToDestinationType(SourceType);
            TargetSourceType = targetParameter.Type!;
            TargetDestinationType = context.ToDestinationType(TargetSourceType);
            TargetParameterName = targetParameter.Identifier.Text;
            DefaultValue = context.GetDefaultValue(getterDeclaration.AttributeLists, propertyName, SourceType, DestinationType);

            if (setterDeclaration != null)
            {
                if (setterDeclaration.ParameterList.Parameters.Count != 2)
                    throw new UserViewableException(
                            $"Attached type setter method {attachedInterfaceDeclaration.Identifier.Text}.{setterDeclaration.Identifier.Text} doesn't take two parameters as expected");

                TargetParameterName = setterDeclaration.ParameterList.Parameters.FirstOrDefault()!.Identifier.Text;
            }
        }

        public void GenerateMainClassDescriptor(Source source)
        {
            if (!(Context.OutputType is XamlOutputType xamlOutputType))
                return;

            TypeSyntax nonNullablePropertyType;
            if (DestinationType is NullableTypeSyntax nullablePropertyType)
                nonNullablePropertyType = nullablePropertyType.ElementType;
            else nonNullablePropertyType = DestinationType;

            string descriptorName = xamlOutputType.GetPropertyDescriptorName(Name);

            source.AddLine(
                $"public static readonly {xamlOutputType.DependencyPropertyClassName} {descriptorName} = PropertyUtils.RegisterAttached(\"{Name}\", typeof({nonNullablePropertyType}), typeof({TargetDestinationType}), {DefaultValue});");
        }

        public void GenerateMainClassMethods(Source source)
        {
            bool classPropertyTypeDiffersFromInterface = SourceType.ToString() != DestinationType.ToString();

            source.AddBlankLineIfNonempty();
            if (Context.OutputType is XamlOutputType xamlOutputType)
            {
                string descriptorName = xamlOutputType.GetPropertyDescriptorName(Name);

                source.AddLine($"public static {DestinationType} Get{Name}({TargetDestinationType} {TargetParameterName}) => ({DestinationType}) {TargetParameterName}.GetValue({descriptorName});");

                if (SetterDeclaration != null)
                    source.AddLine($"public static void Set{Name}({TargetDestinationType} {TargetParameterName}, {DestinationType} value) => {TargetParameterName}.SetValue({descriptorName}, value);");
            }
            else
            {
                // TODO: Support this
            }

#if LATER
            //if (!includeXmlComment)
            propertyDeclaration = propertyDeclaration.WithLeadingTrivia(
                    TriviaList(propertyDeclaration.GetLeadingTrivia()
                        .Insert(0, CarriageReturnLineFeed)
                        .Insert(0, CarriageReturnLineFeed)));
#endif
        }

        public void GenerateAttachedClassMethods(Source source)
        {
            bool classPropertyTypeDiffersFromInterface = SourceType.ToString() != DestinationType.ToString();

            source.AddBlankLineIfNonempty();
            if (Context.OutputType is XamlOutputType xamlOutputType)
            {
                source.AddLine($"public {DestinationType} Get{Name}({TargetSourceType} {TargetParameterName}) => {Interface.DestinationClassName}.Get{Name}(({TargetDestinationType}) {TargetParameterName});");
                if (SetterDeclaration != null)
                    source.AddLine($"public void Set{Name}({TargetSourceType} {TargetParameterName}, {DestinationType} value) => {Interface.DestinationClassName}.Set{Name}(({TargetDestinationType}) {TargetParameterName}, value);");
            }
            else
            {
                // TODO: Support this
            }
        }
    }
}
