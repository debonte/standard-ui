using System;
using static Microsoft.StandardUI.FactoryStatics;

namespace Microsoft.StandardUI.Controls
{
    public abstract class StandardUIControl : IUIElement
    {
        IStandardUIControlEnvironmentPeer _peer;

        public StandardUIControl(IStandardUIControlEnvironmentPeer? peer = null)
        {
            if (peer == null)
                peer = StandardUIControlEnvironmentPeer(this);
            _peer = peer;
        }

        public double Width
        {
            get => _peer.Width;
            set => _peer.Height = value;
        }

        public double MinWidth
        {
            get => _peer.MinWidth;
            set => _peer.MinWidth = value;
        }

        public double MaxWidth
        {
            get => _peer.MaxWidth;
            set => _peer.MaxWidth = value;
        }

        public double Height
        {
            get => _peer.Height;
            set => _peer.Height = value;
        }

        public double MinHeight
        {
            get => _peer.MinHeight;
            set => _peer.MinHeight = value;
        }

        public double MaxHeight
        {
            get => _peer.MaxHeight;
            set => _peer.MaxHeight = value;
        }

        public IControlTemplate Template
        {
            get => _peer.Template;
            set => _peer.Template = value;
        }

        /// <summary>
        /// Retrieves the named element in the instantiated ControlTemplate visual tree.
        /// </summary>
        /// <param name="childName">The name of the element to find.</param>
        /// <returns>The named element from the template, if the element is found. Can
        /// return null if no element with name childName was found in the template.</returns>
        protected IUIPropertyObject GetTemplateChild(string childName)
        {
            // TODO: Finish this
            return null;
        }

        public Size DesiredSize { get; private set; }

        public double ActualX => throw new NotImplementedException();

        public double ActualY => throw new NotImplementedException();

        public double ActualWidth => throw new NotImplementedException();

        public double ActualHeight => throw new NotImplementedException();

        public void Measure(Size availableSize)
        {
            var desiredSize = MeasureOverride(availableSize);

            //enforce that MeasureCore can not return PositiveInfinity size even if given Infinte availabel size.
            //Note: NegativeInfinity can not be returned by definition of Size structure.
            if (double.IsPositiveInfinity(desiredSize.Width) || double.IsPositiveInfinity(desiredSize.Height))
                throw new InvalidOperationException($"Layout measurement override of element '{GetType().FullName}' should not return PositiveInfinity as its DesiredSize, even if Infinity is passed in as available size.");

            //enforce that MeasureCore cannot return NaN size.
            if (double.IsNaN(desiredSize.Width) || double.IsNaN(desiredSize.Height))
                throw new InvalidOperationException($"Layout measurement override of element '{GetType().FullName}' should not return NaN values as its DesiredSize.");

            DesiredSize = desiredSize;
        }

        public void Arrange(Rect finalRect)
        {
            ArrangeOverride(new Size(finalRect.Width, finalRect.Height));
        }

        protected abstract Size MeasureOverride(Size availableSize);
        protected abstract Size ArrangeOverride(Size finalSize);

        public object GetValue(IUIProperty dp) => _peer.GetValue(dp);
        public object ReadLocalValue(IUIProperty dp) => _peer.ReadLocalValue(dp);
        public void SetValue(IUIProperty dp, object value) => _peer.SetValue(dp, value);

        /// <summary>
        /// This method can be overridden to add further graphical elements (not previously defined in a logical tree) to a drawn element.
        /// It's similar to the OnRender method in WPF.
        /// </summary>
        /// <param name="visualizer">visualizer that should draw to</param>
        public virtual void OnVisualize(IVisualizer visualizer) { }
    }
}
