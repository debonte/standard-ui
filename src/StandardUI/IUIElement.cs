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
    }
}
