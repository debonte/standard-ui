namespace Microsoft.StandardUI.Controls
{
    public interface IStandardControlEnvironmentPeer : IDependencyObjectEnvironmentPeer
    {
        public IUIElement? BuildContent { get; }
    }
}
