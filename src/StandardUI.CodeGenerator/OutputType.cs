using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace StandardUI.CodeGenerator
{
    public abstract class OutputType
    {
        public static QualifiedNameSyntax MicrosoftStandardUI = QualifiedName(IdentifierName("Microsoft"), IdentifierName("StandardUI"));

        public abstract string ProjectBaseDirectory { get; }
        public abstract QualifiedNameSyntax RootNamespace { get; }
        public abstract string? FrameworkElementBaseClassName { get; }
        public abstract string? DefaultBaseClassName { get; }
        public abstract IEnumerable<QualifiedNameSyntax> GetUsings(bool hasPropertyDescriptors, bool hasTypeConverterAttribute);
        public abstract bool EmitChangedNotifications { get; }
    }

    public abstract class XamlOutputType : OutputType
    {
        public abstract string DependencyPropertyClassName { get; }
        public virtual string GetPropertyDescriptorName(string propertyName) => propertyName + "Property";
        public override bool EmitChangedNotifications => true;
        public abstract string WrapperSuffix { get; }
    }

    public class WpfXamlOutputType : XamlOutputType
    {
        public static readonly WpfXamlOutputType Instance = new WpfXamlOutputType();

        public override string ProjectBaseDirectory => "StandardUI.Wpf";
        public override QualifiedNameSyntax RootNamespace => QualifiedName(MicrosoftStandardUI, IdentifierName("Wpf"));
        public override string DependencyPropertyClassName => "System.Windows.DependencyProperty";
        public override string? FrameworkElementBaseClassName => "System.Windows.UIElement";
        public override string? DefaultBaseClassName => "System.Windows.DependencyObject";
        public override string WrapperSuffix => "Wpf";

        public override IEnumerable<QualifiedNameSyntax> GetUsings(bool hasPropertyDescriptors, bool hasTypeConverterAttribute)
        {
            var usings = new List<QualifiedNameSyntax>();

            if (hasPropertyDescriptors)
                usings.Add(QualifiedName(IdentifierName("System"), IdentifierName("Windows")));
            if (hasTypeConverterAttribute)
                usings.Add(QualifiedName(IdentifierName("System"), IdentifierName("ComponentModel")));

#if NOT_NEEDED
            usings.Add(QualifiedName(
                    QualifiedName(IdentifierName("System"), IdentifierName("Windows")),
                    IdentifierName("Markup")));
#endif

            return usings;
        }
    }

    public class UwpXamlOutputType : XamlOutputType
    {
        public static readonly UwpXamlOutputType Instance = new UwpXamlOutputType();

        public override string ProjectBaseDirectory => "StandardUI.UWP";
        public override QualifiedNameSyntax RootNamespace => QualifiedName(MicrosoftStandardUI, IdentifierName("UWP"));
        public override string DependencyPropertyClassName => "DependencyProperty";
        public override string? FrameworkElementBaseClassName => "Windows.UI.Xaml.UIElement";
        public override string? DefaultBaseClassName => "DependencyObject";
        public override string WrapperSuffix => "Uwp";
        public override IEnumerable<QualifiedNameSyntax> GetUsings(bool hasPropertyDescriptors, bool hasTypeConverterAttribute)
        {
            throw new NotImplementedException();
        }
    }

    public class XamarinFormsXamlOutputType : XamlOutputType
    {
        public static readonly XamarinFormsXamlOutputType Instance = new XamarinFormsXamlOutputType();

        public override string ProjectBaseDirectory => Path.Combine("XamarinForms", "StandardUI.XamarinForms");
        public override QualifiedNameSyntax RootNamespace => QualifiedName(MicrosoftStandardUI, IdentifierName("XamarinForms"));
        public override string DependencyPropertyClassName => "BindableProperty";
        public override string? FrameworkElementBaseClassName => "VisualElement";
        public override string? DefaultBaseClassName => "BindableObject";
        public override string WrapperSuffix => "Forms";

        public override IEnumerable<QualifiedNameSyntax> GetUsings(bool hasPropertyDescriptors, bool hasTypeConverterAttribute)
        {
            var usings = new List<QualifiedNameSyntax>();
            usings.Add(QualifiedName(IdentifierName("Xamarin"), IdentifierName("Forms")));
            return usings;
        }
    }

    public class StandardModelOutputType : OutputType
    {
        public static readonly StandardModelOutputType Instance = new StandardModelOutputType();

        public override string ProjectBaseDirectory => Path.Combine("StandardUI", "StandardModel");
        public override QualifiedNameSyntax RootNamespace => QualifiedName(MicrosoftStandardUI, IdentifierName("StandardModel"));
        public override string? FrameworkElementBaseClassName => "ObjectWithCascadingNotifications";
        public override string? DefaultBaseClassName => "ObjectWithCascadingNotifications";

        public override IEnumerable<QualifiedNameSyntax> GetUsings(bool hasPropertyDescriptors, bool hasTypeConverterAttribute)
        {
            return new List<QualifiedNameSyntax>();
        }

        public override bool EmitChangedNotifications => false;
    }
}
