namespace Microsoft.StandardUI
{
    /// <summary>
    /// Represents number values that specify a height and width.
    /// </summary>
    public struct Size
    {
        public static readonly Size Default = new Size(0, 0);

        public double Height { get; set; }
        public double Width { get; set; }

        public Size(double width, double height)
        {
            Width = width;
            Height = height;
        }
    }
}
