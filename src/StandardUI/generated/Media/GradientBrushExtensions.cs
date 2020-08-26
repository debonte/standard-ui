// This file is generated from IGradientBrush.cs. Update the source file to change its contents.

using System.Collections.Generic;

namespace System.StandardUI.Media
{
    public static class GradientBrushExtensions
    {
        public static T GradientStops<T>(this T gradientBrush, IEnumerable<IGradientStop> value) where T : IGradientBrush
        {
            gradientBrush.GradientStops = value;
            return gradientBrush;
        }
        
        public static T MappingMode<T>(this T gradientBrush, BrushMappingMode value) where T : IGradientBrush
        {
            gradientBrush.MappingMode = value;
            return gradientBrush;
        }
        
        public static T SpreadMethod<T>(this T gradientBrush, GradientSpreadMethod value) where T : IGradientBrush
        {
            gradientBrush.SpreadMethod = value;
            return gradientBrush;
        }
    }
}
