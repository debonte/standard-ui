using System.StandardUI.Media;

namespace System.StandardUI.Shapes
{
    [UIModelObject]
    public interface IPath : IShape
    {
        [DefaultValue(null)]
        IGeometry Data { get; set; }
    }
}
