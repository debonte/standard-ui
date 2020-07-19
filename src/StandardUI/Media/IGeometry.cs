namespace Microsoft.StandardUI.Media
{
    [UIModelObject]
    public interface IGeometry
    {
        [DefaultValue(0.25)]
        double StandardFlatteningTolerance { get; }

        [DefaultValue(null)]
        ITransform Transform { get; }
    }
}
