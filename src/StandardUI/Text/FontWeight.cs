namespace System.StandardUI.Text
{
    /// <summary>
    /// Expresses the density of a typeface, in terms of the lightness or heaviness of the strokes.
    /// </summary>
    public struct FontWeight
    {
        /// <summary>
        /// The font weight expressed as a numeric value.
        /// </summary>
        public ushort Weight { get; set; }

        public FontWeight(ushort weight)
        {
            Weight = weight;
        }
    }
}
