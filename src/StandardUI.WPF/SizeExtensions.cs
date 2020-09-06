namespace System.StandardUI.Wpf
{
    public static class SizeExtensions
    {
        public static Windows.Size ToWpfSize(this Size size) => new Windows.Size(size.Width, size.Height);

        public static Size FromWpfSize(Windows.Size size) => new Size(size.Width, size.Height);
    }
}
