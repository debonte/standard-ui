// This file is generated from IPolyline.cs. Update the source file to change its contents.

using System.StandardUI.Media;
using System.StandardUI.Wpf.Media;
using System.StandardUI.Shapes;

namespace System.StandardUI.Wpf.Shapes
{
    public class Polyline : Shape, IPolyline
    {
        public static readonly Windows.DependencyProperty FillRuleProperty = PropertyUtils.Register(nameof(FillRule), typeof(FillRule), typeof(Polyline), FillRule.EvenOdd);
        public static readonly Windows.DependencyProperty PointsProperty = PropertyUtils.Register(nameof(Points), typeof(PointsWpf), typeof(Polyline), PointsWpf.Default);
        
        public FillRule FillRule
        {
            get => (FillRule) GetValue(FillRuleProperty);
            set => SetValue(FillRuleProperty, value);
        }
        
        public PointsWpf Points
        {
            get => (PointsWpf) GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }
        Points IPolyline.Points
        {
            get => Points.Points;
            set => Points = new PointsWpf(value);
        }
        
        public override void OnVisualize(IVisualizer visualizer) => visualizer.DrawPolyline(this);
    }
}
