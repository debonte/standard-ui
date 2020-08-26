namespace System.StandardUI.Controls
{
    public interface IUserControl : IControl
    {
        public IUIElement? Content { get; set; }
    }
}
