// This file is generated from ICanvas.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;

namespace Microsoft.StandardUI.Wpf.Controls
{
    public partial class Canvas : Panel, ICanvas
    {
        protected override System.Windows.Size MeasureOverride(System.Windows.Size constraint)
        {
            var childConstraint = new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity);

            foreach (IUIElement? childInterface in Children)
            {
                var child = (System.Windows.UIElement?)childInterface;

                if (child == null)
                    continue;
                child.Measure(childConstraint);
            }

            return new System.Windows.Size();
        }

        protected override System.Windows.Size ArrangeOverride(System.Windows.Size arrangeSize)
        {
            //Canvas arranges children at their DesiredSize.
            //This means that Margin on children is actually respected and added
            //to the size of layout partition for a child. 
            //Therefore, is Margin is 10 and Left is 20, the child's ink will start at 30.

            foreach (IUIElement childInterface in Children)
            {
                var child = (System.Windows.UIElement?)childInterface;

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

                child.Arrange(new System.Windows.Rect(new System.Windows.Point(x, y), child.DesiredSize));
            }
            return arrangeSize;
        }
    }
}
