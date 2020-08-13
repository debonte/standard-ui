// This file is generated from IScaleTransform.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using System.Windows;

namespace Microsoft.StandardUI.Wpf.Media
{
    public class ScaleTransform : Transform, IScaleTransform
    {
        public static readonly System.Windows.DependencyProperty CenterXProperty = PropertyUtils.Create(nameof(CenterX), typeof(double), typeof(double), 0.0);
        public static readonly System.Windows.DependencyProperty CenterYProperty = PropertyUtils.Create(nameof(CenterY), typeof(double), typeof(double), 0.0);
        public static readonly System.Windows.DependencyProperty ScaleXProperty = PropertyUtils.Create(nameof(ScaleX), typeof(double), typeof(double), 1.0);
        public static readonly System.Windows.DependencyProperty ScaleYProperty = PropertyUtils.Create(nameof(ScaleY), typeof(double), typeof(double), 1.0);
        
        public double CenterX
        {
            get => (double) GetValue(CenterXProperty);
            set => SetValue(CenterXProperty, value);
        }
        
        public double CenterY
        {
            get => (double) GetValue(CenterYProperty);
            set => SetValue(CenterYProperty, value);
        }
        
        public double ScaleX
        {
            get => (double) GetValue(ScaleXProperty);
            set => SetValue(ScaleXProperty, value);
        }
        
        public double ScaleY
        {
            get => (double) GetValue(ScaleYProperty);
            set => SetValue(ScaleYProperty, value);
        }
    }
}
