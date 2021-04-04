namespace Microsoft.StandardUI.Wpf
{
    public static class SizeExtensions
    {
        public static System.Windows.Size ToWpfSize(this Size size) => new System.Windows.Size(size.Width, size.Height);

        public static Size FromWpfSize(System.Windows.Size size) => new Size(size.Width, size.Height);
    }
}
