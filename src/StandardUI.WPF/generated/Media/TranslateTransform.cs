// This file is generated from ITranslateTransform.cs. Update the source file to change its contents.

using System.StandardUI.Media;
using System.Windows;

namespace System.StandardUI.Wpf.Media
{
    public class TranslateTransform : Transform, ITranslateTransform
    {
        public static readonly System.Windows.DependencyProperty XProperty = PropertyUtils.Register(nameof(X), typeof(double), typeof(TranslateTransform), 0.0);
        public static readonly System.Windows.DependencyProperty YProperty = PropertyUtils.Register(nameof(Y), typeof(double), typeof(TranslateTransform), 0.0);
        
        public double X
        {
            get => (double) GetValue(XProperty);
            set => SetValue(XProperty, value);
        }
        
        public double Y
        {
            get => (double) GetValue(YProperty);
            set => SetValue(YProperty, value);
        }
    }
}
