namespace Microsoft.StandardUI
{
    /// <summary>
    /// Represents number values that specify a height and width.
    /// </summary>
    public struct Size
    {
        public static readonly Size Default = new Size(0, 0);

        /// <summary>
        /// Gets or sets the Width of this instance of Size.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the Height of this instance of Size.
        /// </summary>
        public double Height { get; set; }

        public Size(double width, double height)
        {
            Width = width;
            Height = height;
        }
    }
}
