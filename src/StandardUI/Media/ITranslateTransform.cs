namespace Microsoft.StandardUI.Media
{
    [UIModelObject]
    public interface ITranslateTransform : ITransform
    {
        [DefaultValue(0.0)]
        double X { get; }

        [DefaultValue(0.0)]
        double Y { get; }
    }
}
