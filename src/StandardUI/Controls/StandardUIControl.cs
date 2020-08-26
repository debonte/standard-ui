using System;

namespace Microsoft.StandardUI.Controls
{
    public abstract class StandardUIControl : IUIElement
    {
        public double Width { get; set; } = double.NaN;
        public double MinWidth { get; set; } = 0.0;
        public double MaxWidth { get; set; } = double.PositiveInfinity;
        public double Height { get; set; } = double.NaN;
        public double MinHeight { get; set; } = 0.0;
        public double MaxHeight { get; set; } = double.PositiveInfinity;

        public Size DesiredSize { get; private set; }

        public void Measure(Size availableSize)
        {
            var desiredSize = MeasureOverride(availableSize);

            //enforce that MeasureCore can not return PositiveInfinity size even if given Infinte availabel size.
            //Note: NegativeInfinity can not be returned by definition of Size structure.
            if (double.IsPositiveInfinity(desiredSize.Width) || double.IsPositiveInfinity(desiredSize.Height))
                throw new InvalidOperationException($"Layout measurement override of element '{GetType().FullName}' should not return PositiveInfinity as its DesiredSize, even if Infinity is passed in as available size.");

            //enforce that MeasureCore can not return NaN size .
            if (double.IsNaN(desiredSize.Width) || double.IsNaN(desiredSize.Height))
                throw new InvalidOperationException($"Layout measurement override of element '{GetType().FullName}' should not return NaN values as its DesiredSize.");

            DesiredSize = desiredSize;
        }

        public void Arrange(Rect finalRect)
        {
            ArrangeOverride(new Size(finalRect.Width, finalRect.Height));
        }

        protected abstract Size MeasureOverride(Size availableSize);
        protected abstract Size ArrangeOverride(Size finalSize);

    }
}
