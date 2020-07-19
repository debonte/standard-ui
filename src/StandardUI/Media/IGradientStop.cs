namespace Microsoft.StandardUI.Media
{
    [UIModelObject]
    public interface IGradientStop
    {
        // The default is Transparent
        Color Color { get; }

        [DefaultValue(0.0)]
        double Offset { get; }
    }
}
