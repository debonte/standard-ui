namespace Microsoft.StandardUI.Media
{
    [UIModelObject]
    public interface IRadialGradientBrush : IGradientBrush
    {
        [DefaultValue("0.5,0.5")]
        Point Center { get; }

        [DefaultValue("0.5,0.5")]
        Point GradientOrigin { get; }

        [DefaultValue(0.5)]
        double RadiusX { get; }
    }
}
