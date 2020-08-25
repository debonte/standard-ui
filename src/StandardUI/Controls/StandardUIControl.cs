namespace Microsoft.StandardUI.Controls
{
    public abstract class StandardUIControl : IUIElement
    {
        public double Width { get; set; }
        public double MinWidth { get; set; }
        public double MaxWidth { get; set; }
        public double Height { get; set; }
        public double MinHeight { get; set; }
        public double MaxHeight { get; set; }
    }
}
