namespace Microsoft.StandardUI
{
    public interface IUIElement
    {
        /// <summary>
        /// The width of the object, in pixels. The default is NaN. Except for the special NaN value, this value must be equal to or greater than 0.
        /// </summary>
        [DefaultValue(double.NaN)]
        double Width { get; set; }

        /// <summary>
        /// The minimum width of the object, in pixels. The default is 0. This value can be any value equal to or greater than 0.
        /// </summary>
        [DefaultValue(0.0)]
        public double MinWidth { get; set; }

        /// <summary>
        /// The maximum width of the object, in pixels. The default is PositiveInfinity. This value can be any value equal to or greater than 0.
        /// </summary>
        [DefaultValue(double.PositiveInfinity)]
        public double MaxWidth { get; set; }

        /// <summary>
        /// The height of the object, in pixels. The default is NaN. Except for the special NaN value, this value must be equal to or greater than 0.
        /// </summary>
        [DefaultValue(double.NaN)]
        double Height { get; set; }

        /// <summary>
        /// The minimum height of the object, in pixels. The default is 0. This value can be any value equal to or greater than 0.
        /// </summary>
        [DefaultValue(0.0)]
        public double MinHeight { get; set; }

        /// <summary>
        /// The maximum height of the object, in pixels. The default is PositiveInfinity. This value can be any value equal to or greater than 0.
        /// </summary>
        [DefaultValue(double.PositiveInfinity)]
        public double MaxHeight { get; set; }

        /// <summary>
        /// Gets the size that this UIElement computed during the measure pass of the layout process.
        /// </summary>
        public Size DesiredSize { get; }

        /// <summary>
        /// Updates the DesiredSize of a UIElement. Typically, objects that implement custom layout for their layout children call this method from
        /// their own MeasureOverride implementations to form a recursive layout update.
        /// </summary>
        /// <param name="availableSize">The available space that a parent can allocate to a child object. A child object can request a larger space
        /// than what is available; the provided size might be accommodated if scrolling or other resize behavior is possible in that particular container.
        /// </param>
        public void Measure(Size availableSize);

        /// <summary>
        /// Positions child objects and determines a size for a UIElement. Parent objects that implement custom layout for their child elements should
        /// call this method from their layout override implementations to form a recursive layout update.
        /// </summary>
        /// <param name="finalRect">The final size that the parent computes for the child in layout, provided as a Rect value.</param>
        public void Arrange(Rect finalRect);
    }
}
