namespace Microsoft.StandardUI.Media
{
    [UIModelObject]
    public interface IScaleTransform : ITransform
    {
        [DefaultValue(0.0)]
        double CenterX { get; }

        [DefaultValue(0.0)]
        double CenterY { get; }

        [DefaultValue(1.0)]
        double ScaleX { get; }

        [DefaultValue(1.0)]
        double ScaleY { get; }
    }
}
