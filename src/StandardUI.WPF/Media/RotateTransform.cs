// This file is generated from IRotateTransform.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using System.Windows;

namespace Microsoft.StandardUI.Wpf.Media
{
    public class RotateTransform : Transform, IRotateTransform
    {
        public static readonly System.Windows.DependencyProperty AngleProperty = PropertyUtils.Create(nameof(Angle), typeof(double), typeof(double), 0.0);
        public static readonly System.Windows.DependencyProperty CenterXProperty = PropertyUtils.Create(nameof(CenterX), typeof(double), typeof(double), 0.0);
        public static readonly System.Windows.DependencyProperty CenterYProperty = PropertyUtils.Create(nameof(CenterY), typeof(double), typeof(double), 0.0);
        
        public double Angle
        {
            get => (double) GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
        }
        
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
    }
}
