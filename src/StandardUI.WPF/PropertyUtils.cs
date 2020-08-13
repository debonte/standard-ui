using Microsoft.StandardUI.Media;
using Microsoft.StandardUI.Wpf.Media;
using System;
using System.Collections;
using System.Linq;
using System.Windows;

namespace Microsoft.StandardUI.Wpf
{
    public static class PropertyUtils
    {
        public static IEnumerable Empty<T>()
        {
            return Enumerable.Empty<T>();
        }

        public static System.Windows.DependencyProperty Create(string propertyName, Type propertyType, Type ownerType, object? defaultValue)
        {
            if (propertyType == typeof(IBrush))
                propertyType = typeof(Brush);
            else if (propertyType == typeof(ITransform))
                propertyType = typeof(Transform);
            else if (propertyType == typeof(Color))
            {
                propertyType = typeof(ColorWpf);
                defaultValue = ColorWpf.Transparent;
            }

            var propertyMetadata = new PropertyMetadata(defaultValue, OnPropertyChanged);
            return System.Windows.DependencyProperty.Register(propertyName, propertyType, ownerType, propertyMetadata);
        }

        private static void OnPropertyChanged(System.Windows.DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(obj is INotifyObjectOrSubobjectChanged parentObj))
                return;

            // The logic below cascades change notifications from subobjects up the object hierarchy, eventually causing the GraphicsCanvas
            // to be invalidated on any change
            if (e.OldValue is INotifyObjectOrSubobjectChanged oldChildObj)
                oldChildObj.Changed -= parentObj.NotifySinceSubobjectChanged;

            if (e.NewValue is INotifyObjectOrSubobjectChanged newChildObj)
                newChildObj.Changed += parentObj.NotifySinceSubobjectChanged;

            parentObj.NotifySinceObjectChanged();
        }
    }
}
