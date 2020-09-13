// This file is generated from IPath.cs. Update the source file to change its contents.

using System.StandardUI.Media;
using System.StandardUI.Wpf.Media;
using System.StandardUI.Shapes;

namespace System.StandardUI.Wpf.Shapes
{
    public class Path : Shape, IPath
    {
        public static readonly Windows.DependencyProperty DataProperty = PropertyUtils.Register(nameof(Data), typeof(Geometry), typeof(Path), null);
        
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
        
        public override void OnDraw(IVisualizer visualizer) => visualizer.DrawPath(this);
    }
}
