namespace Microsoft.StandardUI.Media
{
    [UIModelObject]
    public interface IBezierSegment : IPathSegment
    {
        Point Point1 { get; }

        Point Point2 { get; }

        Point Point3 { get; }
    }
}
