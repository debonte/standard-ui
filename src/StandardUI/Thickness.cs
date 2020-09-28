namespace System.StandardUI
{
    /// <summary>
    /// Describes the thickness of a frame around a rectangle. Four Double values describe the Left, Top,
    /// Right, and Bottom sides of the rectangle, respectively.
    /// </summary>
    public struct Thickness
    {
        /// <summary>
        /// The bottom edge measure of the Thickness.
        /// </summary>
        public double Bottom { get; set; }

        /// <summary>
        /// The left side measure of the Thickness.
        /// </summary>
        public double Left { get; set; }

        /// <summary>
        /// The right side measure of the Thickness.
        /// </summary>
        public double Right { get; set; }

        /// <summary>
        /// The top edge measure of the Thickness.
        /// </summary>
        public double Top { get; set; }
    }
}
