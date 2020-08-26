namespace Microsoft.StandardUI.Wpf
{
    public static class SizeExtensions
    {
        public static System.Windows.Size ToWpfSize(this Size size)
        {
            return new System.Windows.Size(size.Width, size.Height);
        }
    }
}
