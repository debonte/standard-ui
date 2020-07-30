namespace Microsoft.StandardUI.Controls
{
    [UIModelObject]
    public interface IPanel : IUIElement
    {
        IUIElementCollection Children { get; }
    }
}
