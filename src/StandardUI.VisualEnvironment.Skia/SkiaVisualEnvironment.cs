using SkiaSharp;
using System;

namespace System.StandardUI.SkiaVisualizer
{
    public class SkiaVisualEnvironment : IVisualEnvironment
    {
        public IVisualizer CreateVisualizer(in Rect cullingRect) => new SkiaVisualizer(cullingRect);

        public void RenderToBuffer(IVisual visual, IntPtr pixels, int width, int height, int rowBytes)
        {
            SkiaVisual skiaVisual = (SkiaVisual)visual;

            var info = new SKImageInfo(width, height, SKImageInfo.PlatformColorType, SKAlphaType.Premul);

            using (SKSurface surface = SKSurface.Create(info, pixels, rowBytes))
            {
                surface.Canvas.DrawPicture(skiaVisual.SKPicture);
            }
        }

        public IVisualHostControl CreateHostControl(object? arg1 = null, object? arg2 = null, object? arg3 = null)
        {
            throw new NotImplementedException();
        }
    }
}
