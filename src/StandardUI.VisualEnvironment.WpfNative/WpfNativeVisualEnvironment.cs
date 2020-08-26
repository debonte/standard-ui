using System;

namespace System.StandardUI.VisualEnvironment.WpfNative
{
    public class WpfNativeVisualEnvironment : IVisualEnvironment
    {
        public IVisualizer CreateVisualizer(in Rect cullingRect) => new WpfNativeVisualizer(cullingRect);

        public void RenderToBuffer(IVisual visual, IntPtr pixels, int width, int height, int rowBytes)
        {
            throw new NotImplementedException();
        }

        public IVisualHostControl CreateHostControl(object? arg1 = null, object? arg2 = null, object? arg3 = null)
        {
            throw new NotImplementedException();
        }
    }
}
