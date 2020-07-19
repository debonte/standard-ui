using System.Collections.Generic;

namespace Microsoft.StandardUI
{
    [UIModelObject]
    public interface IUIElementCollection : IEnumerable<IUIElement>, IList<IUIElement>
    {
    }
}
