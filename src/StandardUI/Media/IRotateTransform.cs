namespace Microsoft.StandardUI.Media
{
    [UIModelObject]
    public interface IRotateTransform : ITransform
    {
        [DefaultValue(0.0)]
        double Angle { get; }

        [DefaultValue(0.0)]
        double CenterX { get; }

        [DefaultValue(0.0)]
        double CenterY { get; }
    }
}
