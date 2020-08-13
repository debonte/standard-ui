// This file is generated from IPolygon.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using Microsoft.StandardUI.Wpf.Media;
using Microsoft.StandardUI.Shapes;
using System.Windows;

namespace Microsoft.StandardUI.Wpf.Shapes
{
    public class Polygon : Shape, IPolygon
    {
        public static readonly System.Windows.DependencyProperty FillRuleProperty = PropertyUtils.Create(nameof(FillRule), typeof(FillRule), typeof(FillRule), FillRule.EvenOdd);
        public static readonly System.Windows.DependencyProperty PointsProperty = PropertyUtils.Create(nameof(Points), typeof(PointsWpf), typeof(PointsWpf), PointsWpf.Default);
        
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
