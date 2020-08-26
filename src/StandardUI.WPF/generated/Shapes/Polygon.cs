// This file is generated from IPolygon.cs. Update the source file to change its contents.

using System.StandardUI.Media;
using System.StandardUI.Wpf.Media;
using System.StandardUI.Shapes;
using System.Windows;

namespace System.StandardUI.Wpf.Shapes
{
    public class Polygon : Shape, IPolygon
    {
        public static readonly Windows.DependencyProperty FillRuleProperty = PropertyUtils.Register(nameof(FillRule), typeof(FillRule), typeof(Polygon), FillRule.EvenOdd);
        public static readonly Windows.DependencyProperty PointsProperty = PropertyUtils.Register(nameof(Points), typeof(PointsWpf), typeof(Polygon), PointsWpf.Default);
        
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
        Points IPolygon.Points
        {
            get => Points.Points;
            set => Points = new PointsWpf(value);
        }
    }
}
