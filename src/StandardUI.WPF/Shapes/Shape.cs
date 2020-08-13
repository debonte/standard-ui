// This file is generated from IShape.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using Microsoft.StandardUI.Wpf.Media;
using Microsoft.StandardUI.Shapes;
using System.Windows;

namespace Microsoft.StandardUI.Wpf.Shapes
{
    public class Shape : UIElement, IShape
    {
        public static readonly System.Windows.DependencyProperty FillProperty = PropertyUtils.Create(nameof(Fill), typeof(Brush), typeof(Brush?), null);
        public static readonly System.Windows.DependencyProperty StrokeProperty = PropertyUtils.Create(nameof(Stroke), typeof(Brush), typeof(Brush?), null);
        public static readonly System.Windows.DependencyProperty StrokeThicknessProperty = PropertyUtils.Create(nameof(StrokeThickness), typeof(double), typeof(double), 1.0);
        public static readonly System.Windows.DependencyProperty StrokeMiterLimitProperty = PropertyUtils.Create(nameof(StrokeMiterLimit), typeof(double), typeof(double), 10.0);
        public static readonly System.Windows.DependencyProperty StrokeLineCapProperty = PropertyUtils.Create(nameof(StrokeLineCap), typeof(PenLineCap), typeof(PenLineCap), PenLineCap.Flat);
        public static readonly System.Windows.DependencyProperty StrokeLineJoinProperty = PropertyUtils.Create(nameof(StrokeLineJoin), typeof(PenLineJoin), typeof(PenLineJoin), PenLineJoin.Miter);
        
        public Brush? Fill
        {
            get => (Brush?) GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }
        IBrush? IShape.Fill
        {
            get => Fill;
            set => Fill = (Brush?) value;
        }
        
        public Brush? Stroke
        {
            get => (Brush?) GetValue(StrokeProperty);
            set => SetValue(StrokeProperty, value);
        }
        IBrush? IShape.Stroke
        {
            get => Stroke;
            set => Stroke = (Brush?) value;
        }
        
        public double StrokeThickness
        {
            get => (double) GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }
        
        public double StrokeMiterLimit
        {
            get => (double) GetValue(StrokeMiterLimitProperty);
            set => SetValue(StrokeMiterLimitProperty, value);
        }
        
        public PenLineCap StrokeLineCap
        {
            get => (PenLineCap) GetValue(StrokeLineCapProperty);
            set => SetValue(StrokeLineCapProperty, value);
        }
        
        public PenLineJoin StrokeLineJoin
        {
            get => (PenLineJoin) GetValue(StrokeLineJoinProperty);
            set => SetValue(StrokeLineJoinProperty, value);
        }
    }
}
