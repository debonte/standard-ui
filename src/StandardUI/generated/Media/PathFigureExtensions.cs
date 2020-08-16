// This file is generated from IPathFigure.cs. Update the source file to change its contents.

using System.Collections.Generic;

namespace Microsoft.StandardUI.Media
{
    public static class PathFigureExtensions
    {
        public static T Segments<T>(this T pathFigure, IEnumerable<IPathSegment> value) where T : IPathFigure
        {
            pathFigure.Segments = value;
            return pathFigure;
        }
        
        public static T StartPoint<T>(this T pathFigure, Point value) where T : IPathFigure
        {
            pathFigure.StartPoint = value;
            return pathFigure;
        }
        
        public static T IsClosed<T>(this T pathFigure, bool value) where T : IPathFigure
        {
            pathFigure.IsClosed = value;
            return pathFigure;
        }
        
        public static T IsFilled<T>(this T pathFigure, bool value) where T : IPathFigure
        {
            pathFigure.IsFilled = value;
            return pathFigure;
        }
    }
}
