namespace Microsoft.StandardUI.Media
{
    [UIModelObject]
    public interface ILinearGradientBrush : IGradientBrush
    {
        Point StartPoint { get; }

        Point EndPoint { get; }
    }
}
