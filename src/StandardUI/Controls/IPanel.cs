namespace System.StandardUI.Controls
{
    [UIModelObject]
    public interface IPanel : IUIElement
    {
        IUIElementCollection Children { get; }
    }
}
