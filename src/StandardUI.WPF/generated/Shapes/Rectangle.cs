// This file is generated from IRectangle.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Shapes;

namespace Microsoft.StandardUI.Wpf.Shapes
{
    public class Rectangle : Shape, IRectangle
    {
        public static readonly System.Windows.DependencyProperty RadiusXProperty = PropertyUtils.Register(nameof(RadiusX), typeof(double), typeof(Rectangle), 0.0);
        public static readonly System.Windows.DependencyProperty RadiusYProperty = PropertyUtils.Register(nameof(RadiusY), typeof(double), typeof(Rectangle), 0.0);
        
        public double RadiusX
        {
            get => (double) GetValue(RadiusXProperty);
            set => SetValue(RadiusXProperty, value);
        }
        
        public double RadiusY
        {
            get => (double) GetValue(RadiusYProperty);
            set => SetValue(RadiusYProperty, value);
        }
        
        public override void Draw(IDrawingContext drawingContext) => drawingContext.DrawRectangle(this);
    }
}
