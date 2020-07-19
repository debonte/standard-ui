namespace Microsoft.StandardUI.Shapes
{
    [UIModelObject]
    public interface ILine : IShape
    {
        [DefaultValue(0.0)]
        double X1 { get; }

        [DefaultValue(0.0)]
        double Y1 { get; }

        [DefaultValue(0.0)]
        double X2 { get; }

        [DefaultValue(0.0)]
        double Y2 { get; }
    }
}
