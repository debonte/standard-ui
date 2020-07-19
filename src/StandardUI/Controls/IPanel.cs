namespace Microsoft.StandardUI
{
    [UIModelObject]
    public interface IPanel : IUIElement
    {
        IUIElementCollection Children { get; }
    }
}
