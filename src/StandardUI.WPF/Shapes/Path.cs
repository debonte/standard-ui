// This file is generated from IPath.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using Microsoft.StandardUI.Wpf.Media;
using Microsoft.StandardUI.Shapes;
using System.Windows;

namespace Microsoft.StandardUI.Wpf.Shapes
{
    public class Path : Shape, IPath
    {
        public static readonly System.Windows.DependencyProperty DataProperty = PropertyUtils.Create(nameof(Data), typeof(Geometry), typeof(Geometry), null);
        
        public Geometry Data
        {
            get => (Geometry) GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }
        IGeometry IPath.Data
        {
            get => Data;
            set => Data = (Geometry) value;
        }
    }
}
