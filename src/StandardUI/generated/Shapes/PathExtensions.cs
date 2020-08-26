// This file is generated from IPath.cs. Update the source file to change its contents.

using System.StandardUI.Media;

namespace System.StandardUI.Shapes
{
    public static class PathExtensions
    {
        public static T Data<T>(this T path, IGeometry value) where T : IPath
        {
            path.Data = value;
            return path;
        }
    }
}
