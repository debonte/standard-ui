namespace System.StandardUI.Controls
{
    public interface IStandardUIControlEnvironmentPeer : IDependencyObjectEnvironmentPeer
    {
        double Width { get; set; }
        public double MinWidth { get; set; }
        public double MaxWidth { get; set; }

        double Height { get; set; }
        public double MinHeight { get; set; }
        public double MaxHeight { get; set; }

        public IControlTemplate? Template { get; set; }

        public IUIPropertyObject? GetTemplateChild(string childName);
    }
}
