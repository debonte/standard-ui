// This file is generated from IPath.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using Microsoft.StandardUI.Wpf.Media;
using Microsoft.StandardUI.Shapes;

namespace Microsoft.StandardUI.Wpf.Shapes
{
    public class Path : Shape, IPath
    {
        public static readonly System.Windows.DependencyProperty DataProperty = PropertyUtils.Register(nameof(Data), typeof(Geometry), typeof(Path), null);
        
        public Geometry Data
        {
            get => (Geometry) GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }
        IGeometry IPath.Data
        {
            get => Data;
            set => Data = (Geometry) value;
        }
        
        public override void Draw(IDrawingContext visualizer) => visualizer.DrawPath(this);
    }
}
