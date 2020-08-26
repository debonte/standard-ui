namespace Microsoft.StandardUI
{
    /// <summary>
    /// Contains number values that represent the location and size of a rectangle.
    /// </summary>
    public struct Rect
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public double Y { get; set; }
        public double X { get; set; }

        public Rect(Point location, Size size)
        {
            X = location.X;
            Y = location.Y;
            Width = size.Width;
            Height = size.Height;
        }

        public Rect(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}
