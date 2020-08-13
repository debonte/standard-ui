// This file is generated from IGradientBrush.cs. Update the source file to change its contents.

using System.Collections.Generic;
using Microsoft.StandardUI.Media;
using System.Windows;

namespace Microsoft.StandardUI.Wpf.Media
{
    public class GradientBrush : Brush, IGradientBrush
    {
        public static readonly System.Windows.DependencyProperty GradientStopsProperty = PropertyUtils.Create(nameof(GradientStops), typeof(IEnumerable<IGradientStop>), typeof(IEnumerable<IGradientStop>), null);
        public static readonly System.Windows.DependencyProperty MappingModeProperty = PropertyUtils.Create(nameof(MappingMode), typeof(BrushMappingMode), typeof(BrushMappingMode), BrushMappingMode.RelativeToBoundingBox);
        public static readonly System.Windows.DependencyProperty SpreadMethodProperty = PropertyUtils.Create(nameof(SpreadMethod), typeof(GradientSpreadMethod), typeof(GradientSpreadMethod), GradientSpreadMethod.Pad);
        
        public IEnumerable<IGradientStop> GradientStops
        {
            get => (IEnumerable<IGradientStop>) GetValue(GradientStopsProperty);
            set => SetValue(GradientStopsProperty, value);
        }
        
        public BrushMappingMode MappingMode
        {
            get => (BrushMappingMode) GetValue(MappingModeProperty);
            set => SetValue(MappingModeProperty, value);
        }
        
        public GradientSpreadMethod SpreadMethod
        {
            get => (GradientSpreadMethod) GetValue(SpreadMethodProperty);
            set => SetValue(SpreadMethodProperty, value);
        }
    }
}
