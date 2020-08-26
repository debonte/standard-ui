using System.Collections.Generic;

namespace System.StandardUI.Media
{
	[UIModelObject]
    public interface ITransformGroup : ITransform
    {
        IEnumerable<ITransform> Children { get; }
    }
}
