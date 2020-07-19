namespace Microsoft.StandardUI
{
    [UIModelObject]
    public interface IUIElement
    {
        [DefaultValue(double.NaN)]
        double Width { get; }

        [DefaultValue(double.NaN)]
        double Height { get; }
    }
}
