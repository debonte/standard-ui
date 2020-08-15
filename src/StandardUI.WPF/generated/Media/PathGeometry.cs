// This file is generated from IPathGeometry.cs. Update the source file to change its contents.

using System.Collections.Generic;
using Microsoft.StandardUI.Media;
using System.Windows;

namespace Microsoft.StandardUI.Wpf.Media
{
    public class PathGeometry : Geometry, IPathGeometry
    {
        public static readonly System.Windows.DependencyProperty FiguresProperty = PropertyUtils.Register(nameof(Figures), typeof(IEnumerable<IPathFigure>), typeof(PathGeometry), null);
        public static readonly System.Windows.DependencyProperty FillRuleProperty = PropertyUtils.Register(nameof(FillRule), typeof(FillRule), typeof(PathGeometry), FillRule.EvenOdd);
        
        public IEnumerable<IPathFigure> Figures
        {
            get => (IEnumerable<IPathFigure>) GetValue(FiguresProperty);
            set => SetValue(FiguresProperty, value);
        }
        
        public FillRule FillRule
        {
            get => (FillRule) GetValue(FillRuleProperty);
            set => SetValue(FillRuleProperty, value);
        }
    }
}
