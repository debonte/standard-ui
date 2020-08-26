// This file is generated from IPolygon.cs. Update the source file to change its contents.

using System.StandardUI.Media;

namespace System.StandardUI.Shapes
{
    public static class PolygonExtensions
    {
        public static T FillRule<T>(this T polygon, FillRule value) where T : IPolygon
        {
            polygon.FillRule = value;
            return polygon;
        }
        
        public static T Points<T>(this T polygon, Points value) where T : IPolygon
        {
            polygon.Points = value;
            return polygon;
        }
    }
}
