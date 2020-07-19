namespace Microsoft.StandardUI.Media
{
    [UIModelObject]
    public interface IQuadraticBezierSegment : IPathSegment
    {
        Point Point1 { get; }

        Point Point2 { get; }
    }
}
