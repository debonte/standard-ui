namespace Microsoft.StandardUI
{
    /// <summary>
    /// Represents number values that specify a height and width.
    /// </summary>
    public struct Size
    {
        public static readonly Size Default = new Size(0, 0);

        public Size(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public double Height { get; }

        public double Width { get; }
    }
}
