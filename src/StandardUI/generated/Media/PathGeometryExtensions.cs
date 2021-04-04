// This file is generated from IPathGeometry.cs. Update the source file to change its contents.

using System.Collections.Generic;

namespace Microsoft.StandardUI.Media
{
    public static class PathGeometryExtensions
    {
        public static T Figures<T>(this T pathGeometry, IEnumerable<IPathFigure> value) where T : IPathGeometry
        {
            pathGeometry.Figures = value;
            return pathGeometry;
        }
        
        public static T FillRule<T>(this T pathGeometry, FillRule value) where T : IPathGeometry
        {
            pathGeometry.FillRule = value;
            return pathGeometry;
        }
    }
}
