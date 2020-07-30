namespace Microsoft.StandardUI.Shapes
{
    [UIModelObject]
    public interface IRectangle : IShape
    {
        [DefaultValue(0.0)]
        double RadiusX { get; set; }

        [DefaultValue(0.0)]
        double RadiusY { get; set; }
    }
}
