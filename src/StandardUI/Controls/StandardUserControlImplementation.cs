namespace Microsoft.StandardUI.Controls
{
    public class StandardUserControlImplementation<TControl> : StandardControlImplementation<TControl> where TControl : IControl
    {
        public StandardUserControlImplementation(TControl control) : base(control)
        {
        }

        public IUIElement? Content { get; set; }

        protected override Size MeasureOverride(Size availableSize)
        {
            // By default, return the size of the content
            if (Content != null)
            {
                Content.Measure(availableSize);
                return Content.DesiredSize;
            }

            return new Size(0.0, 0.0);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            // By default, give all the space to the content
            if (Content != null)
            {
                Rect finalRect = new Rect(0, 0, finalSize.Width, finalSize.Height);
                Content.Arrange(finalRect);
            }

            return finalSize;
        }
    }
}
