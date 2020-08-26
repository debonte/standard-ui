// This file is generated from IGeometry.cs. Update the source file to change its contents.

using System.StandardUI.Media;
using System.Windows;
using System.ComponentModel;
using System.StandardUI.Wpf.Converters;

namespace System.StandardUI.Wpf.Media
{
    public class Geometry : Windows.DependencyObject, IGeometry
    {
        public static readonly Windows.DependencyProperty StandardFlatteningToleranceProperty = PropertyUtils.Register(nameof(StandardFlatteningTolerance), typeof(double), typeof(Geometry), 0.25);
        public static readonly Windows.DependencyProperty TransformProperty = PropertyUtils.Register(nameof(Transform), typeof(Transform), typeof(Geometry), null);
        
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
