// This file is generated from ITransformGroup.cs. Update the source file to change its contents.

using System.Collections.Generic;
using System.StandardUI.Media;
using System.Windows;

namespace System.StandardUI.Wpf.Media
{
    public class TransformGroup : Transform, ITransformGroup
    {
        public static readonly Windows.DependencyProperty ChildrenProperty = PropertyUtils.Register(nameof(Children), typeof(IEnumerable<ITransform>), typeof(TransformGroup), null);
        public IEnumerable<ITransform> Children
        {
            get => (IEnumerable<ITransform>) GetValue(ChildrenProperty);
        }
    }
}
