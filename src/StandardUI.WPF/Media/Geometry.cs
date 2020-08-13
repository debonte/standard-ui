// This file is generated from IGeometry.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using System.Windows;
using System.ComponentModel;
using Microsoft.StandardUI.Wpf.Converters;

namespace Microsoft.StandardUI.Wpf.Media
{
    public class Geometry : System.Windows.DependencyObject, IGeometry
    {
        public static readonly System.Windows.DependencyProperty StandardFlatteningToleranceProperty = PropertyUtils.Create(nameof(StandardFlatteningTolerance), typeof(double), typeof(double), 0.25);
        public static readonly System.Windows.DependencyProperty TransformProperty = PropertyUtils.Create(nameof(Transform), typeof(Transform), typeof(Transform), null);
        
        public double StandardFlatteningTolerance
        {
            get => (double) GetValue(StandardFlatteningToleranceProperty);
            set => SetValue(StandardFlatteningToleranceProperty, value);
        }
        
        public Transform Transform
        {
            get => (Transform) GetValue(TransformProperty);
            set => SetValue(TransformProperty, value);
        }
        ITransform IGeometry.Transform
        {
            get => Transform;
            set => Transform = (Transform) value;
        }
    }
}
