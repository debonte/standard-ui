namespace System.StandardUI.Controls
{
    public static class CanvasImplExtensions
    {
        public static Size MeasureOverrideImpl(this ICanvas canvas, Size constraint)
        {
            Size childConstraint = new Size(double.PositiveInfinity, double.PositiveInfinity);

            foreach (IUIElement child in canvas.Children)
            {
                if (child == null)
                    continue;
                child.Measure(childConstraint);
            }

            return new Size();
        }

        public static Size ArrangeOverride(this ICanvas canvas, Size arrangeSize)
        {
            //Canvas arranges children at their DesiredSize.
            //This means that Margin on children is actually respected and added
            //to the size of layout partition for a child. 
            //Therefore, is Margin is 10 and Left is 20, the child's ink will start at 30.

            foreach (IUIElement child in canvas.Children)
            {
                if (child == null)
                    continue;

                double x = 0;
                double y = 0;

                //Compute offset of the child:
                //If Left is specified, then Right is ignored
                //If Left is not specified, then Right is used
                //If both are not there, then 0
                double left = child.GetCanvasLeft();
                if (!double.IsNaN(left))
                    x = left;

                double top = child.GetCanvasTop();
                if (!double.IsNaN(top))
                    y = top;

                child.Arrange(new Rect(new Point(x, y), child.DesiredSize));
            }
            return arrangeSize;
        }
    }
}
