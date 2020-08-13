// This file is generated from IPathFigure.cs. Update the source file to change its contents.

using System.Collections.Generic;
using Microsoft.StandardUI.Media;
using System.Windows;

namespace Microsoft.StandardUI.Wpf.Media
{
    public class PathFigure : System.Windows.DependencyObject, IPathFigure
    {
        public static readonly System.Windows.DependencyProperty SegmentsProperty = PropertyUtils.Create(nameof(Segments), typeof(IEnumerable<IPathSegment>), typeof(IEnumerable<IPathSegment>), null);
        public static readonly System.Windows.DependencyProperty StartPointProperty = PropertyUtils.Create(nameof(StartPoint), typeof(PointWpf), typeof(PointWpf), PointWpf.Default);
        public static readonly System.Windows.DependencyProperty IsClosedProperty = PropertyUtils.Create(nameof(IsClosed), typeof(bool), typeof(bool), false);
        public static readonly System.Windows.DependencyProperty IsFilledProperty = PropertyUtils.Create(nameof(IsFilled), typeof(bool), typeof(bool), true);
        
        public IEnumerable<IPathSegment> Segments
        {
            get => (IEnumerable<IPathSegment>) GetValue(SegmentsProperty);
            set => SetValue(SegmentsProperty, value);
        }
        
        public PointWpf StartPoint
        {
            get => (PointWpf) GetValue(StartPointProperty);
            set => SetValue(StartPointProperty, value);
        }
        Point IPathFigure.StartPoint
        {
            get => StartPoint.Point;
            set => StartPoint = new PointWpf(value);
        }
        
        public bool IsClosed
        {
            get => (bool) GetValue(IsClosedProperty);
            set => SetValue(IsClosedProperty, value);
        }
        
        public bool IsFilled
        {
            get => (bool) GetValue(IsFilledProperty);
            set => SetValue(IsFilledProperty, value);
        }
    }
}
