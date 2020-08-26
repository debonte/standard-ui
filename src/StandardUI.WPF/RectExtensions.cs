namespace System.StandardUI.Wpf
{
    public static class RectExtensions
    {
        public static System.Windows.Rect ToWpfRect(this Rect rect)
        {
            return new System.Windows.Rect(rect.X, rect.Y, rect.Width, rect.Height);
        }
    }
}
