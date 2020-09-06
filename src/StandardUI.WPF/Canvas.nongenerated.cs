// This file is generated from ICanvas.cs. Update the source file to change its contents.

using System.StandardUI.Controls;
using System.Windows;

namespace System.StandardUI.Wpf.Controls
{
    public partial class Canvas : Panel, ICanvas
    {
        protected override Windows.Size MeasureOverride(Windows.Size constraint)
        {
            var childConstraint = new Windows.Size(double.PositiveInfinity, double.PositiveInfinity);

            foreach (IUIElement? childInterface in Children)
            {
                var child = (UIElement?)childInterface;

                if (child == null)
                    continue;
                child.Measure(childConstraint);
            }

            return new Windows.Size();
        }

        protected override Windows.Size ArrangeOverride(Windows.Size arrangeSize)
        {
            //Canvas arranges children at their DesiredSize.
            //This means that Margin on children is actually respected and added
            //to the size of layout partition for a child. 
            //Therefore, is Margin is 10 and Left is 20, the child's ink will start at 30.

            foreach (IUIElement childInterface in Children)
            {
                var child = (UIElement?)childInterface;

                if (child == null)
                    continue;

                double x = 0;
                double y = 0;

                //Compute offset of the child:
                //If Left is specified, then Right is ignored
                //If Left is not specified, then Right is used
                //If both are not there, then 0
                double left = Canvas.GetLeft(child);
                if (!double.IsNaN(left))
                    x = left;

                double top = Canvas.GetTop(child);
                if (!double.IsNaN(top))
                    y = top;

                child.Arrange(new Windows.Rect(new Windows.Point(x, y), child.DesiredSize));
            }
            return arrangeSize;
        }
    }
}
