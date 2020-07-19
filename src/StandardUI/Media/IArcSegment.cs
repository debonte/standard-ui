namespace Microsoft.StandardUI.Media
{
    [UIModelObject]
    public interface IArcSegment : IPathSegment
    {
        Point Point { get; }

        Size Size { get; }

        [DefaultValue(0.0)]
        double RotationAngle { get; }

        [DefaultValue(false)]
        bool IsLargeArc { get; }

        [DefaultValue(SweepDirection.Counterclockwise)]
        SweepDirection SweepDirection { get; }
    }
}
