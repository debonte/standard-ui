namespace Microsoft.StandardUI.Media
{
    [UIModelObject]
    public interface IGeometry
    {
        [DefaultValue(0.25)]
        double StandardFlatteningTolerance { get; set; }

        [DefaultValue(null)]
        ITransform Transform { get; set; }
    }
}
