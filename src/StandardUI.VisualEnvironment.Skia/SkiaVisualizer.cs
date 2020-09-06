using System.StandardUI.Media;
using System.StandardUI.Shapes;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.StandardUI.Controls;

namespace System.StandardUI.SkiaVisualizer
{
    public class SkiaVisualizer : IVisualizer
    {
        private SKPictureRecorder? _skPictureRecorder;
        private SKCanvas _skCanvas;

        public SkiaVisualizer(in Rect cullingRect)
        {
            _skPictureRecorder = new SKPictureRecorder();

            SKRect skCullingRect = SKRect.Create((float)cullingRect.X, (float)cullingRect.Y, (float)cullingRect.Width, (float)cullingRect.Height);
            _skCanvas = _skPictureRecorder.BeginRecording(skCullingRect);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_skPictureRecorder != null)
            {
                if (disposing)
                    _skPictureRecorder.Dispose();

                _skPictureRecorder = null;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void DrawEllipse(IEllipse ellipse)
        {
            SKPath skPath = new SKPath();
            SKRect skRect = SKRect.Create(0, 0, (float)ellipse.Width, (float)ellipse.Height);
            skPath.AddOval(skRect);

            DrawShapePath(skPath, ellipse);
        }

        public void DrawLine(ILine line)
        {
            SKPath skPath = new SKPath();
            skPath.MoveTo((float)line.X1, (float)line.Y1);
            skPath.LineTo((float)line.X2, (float)line.Y2);

            DrawShapePath(skPath, line);
        }

        public void DrawPath(IPath path)
        {
            throw new NotImplementedException();
        }

        public void DrawPolygon(IPolygon polygon)
        {
            SKPath skPath = new SKPath();
            skPath.FillType = FillRuleToSkiaPathFillType(polygon.FillRule);
            skPath.AddPoly(PointsToSkiaPoints(polygon.Points), close: true);

            DrawShapePath(skPath, polygon);
        }

        public void DrawPolyline(IPolyline polyline)
        {
            SKPath skPath = new SKPath();
            skPath.FillType = FillRuleToSkiaPathFillType(polyline.FillRule);
            skPath.AddPoly(PointsToSkiaPoints(polyline.Points), close: false);

            DrawShapePath(skPath, polyline);
        }

        public void DrawRectangle(IRectangle rectangle)
        {
            SKPath skPath = new SKPath();
            SKRect skRect = SKRect.Create(0, 0, (float)rectangle.Width, (float)rectangle.Height);
            if (rectangle.RadiusX > 0 || rectangle.RadiusY > 0)
                skPath.AddRoundRect(skRect, (float)rectangle.RadiusX, (float)rectangle.RadiusY);
            else
                skPath.AddRect(skRect);

            DrawShapePath(skPath, rectangle);
        }

        public void DrawTextBlock(ITextBlock textBlock)
        {
            IBrush? foreground = textBlock.Foreground;
            if (foreground != null)
            {
                using SKPaint paint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    TextSize = (float)textBlock.FontSize,
                    IsAntialias = true
                };

                InitSkiaPaintForBrush(paint, foreground, textBlock);

                SKRect textBounds = new SKRect();
                paint.MeasureText(textBlock.Text, ref textBounds);
                float baseline = -textBounds.Top;

                _skCanvas.DrawText(textBlock.Text, 0, baseline, paint);
            }
        }

        public IVisual End()
        {
            SKPicture skPicture = _skPictureRecorder!.EndRecording();
            return new SkiaVisual(skPicture);
        }

        private void DrawShapePath(SKPath skPath, IShape shape)
        {
            FillSkiaPath(skPath, shape);
            StrokeSkiaPath(skPath, shape);
        }

        private void FillSkiaPath(SKPath skPath, IShape shape)
        {
            IBrush? fill = shape.Fill;
            if (fill != null)
            {
                using SKPaint paint = new SKPaint { Style = SKPaintStyle.Fill, IsAntialias = true };
                InitSkiaPaintForBrush(paint, fill, shape);
                _skCanvas.DrawPath(skPath, paint);
            }
        }

        private void StrokeSkiaPath(SKPath skPath, IShape shape)
        {
            IBrush? stroke = shape.Stroke;
            if (stroke != null)
            {
                using SKPaint paint = new SKPaint { Style = SKPaintStyle.Stroke, IsAntialias = true };
                InitSkiaPaintForBrush(paint, stroke, shape);
                paint.StrokeWidth = (int)shape.StrokeThickness;
                paint.StrokeMiter = (float)shape.StrokeMiterLimit;

                SKStrokeCap strokeCap = shape.StrokeLineCap switch
                {
                    PenLineCap.Flat => SKStrokeCap.Butt,
                    PenLineCap.Round => SKStrokeCap.Round,
                    PenLineCap.Square => SKStrokeCap.Square,
                    _ => throw new InvalidOperationException($"Unknown PenLineCap value {shape.StrokeLineCap}")
                };
                paint.StrokeCap = strokeCap;

                SKStrokeJoin strokeJoin = shape.StrokeLineJoin switch
                {
                    PenLineJoin.Miter => SKStrokeJoin.Miter,
                    PenLineJoin.Bevel => SKStrokeJoin.Bevel,
                    PenLineJoin.Round => SKStrokeJoin.Round,
                    _ => throw new InvalidOperationException($"Unknown PenLineJoin value {shape.StrokeLineJoin}")
                };
                paint.StrokeJoin = strokeJoin;

                _skCanvas.DrawPath(skPath, paint);
            }
        }

        private static void InitSkiaPaintForBrush(SKPaint paint, IBrush brush, IUIElement uiElement)
        {
            if (brush is ISolidColorBrush solidColorBrush)
                paint.Color = ToSkiaColor(solidColorBrush.Color);
            else if (brush is IGradientBrush gradientBrush)
                paint.Shader = ToSkiaShader(gradientBrush, uiElement);
            else throw new InvalidOperationException($"Brush type {brush.GetType()} isn't currently supported");
        }

        public static SKColor ToSkiaColor(Color color) => new SKColor(color.R, color.G, color.B, color.A);

        public static SKShader ToSkiaShader(IGradientBrush gradientBrush, IUIElement uiElement)
        {
            SKShaderTileMode tileMode = gradientBrush.SpreadMethod switch
            {
                GradientSpreadMethod.Pad => SKShaderTileMode.Clamp,
                GradientSpreadMethod.Reflect => SKShaderTileMode.Mirror,
                GradientSpreadMethod.Repeat => SKShaderTileMode.Repeat,
                _ => throw new InvalidOperationException($"Unknown GradientSpreadMethod value {gradientBrush.SpreadMethod}")
            };

            List<SKColor> skColors = new List<SKColor>();
            List<float> skiaColorPositions = new List<float>();
            foreach (IGradientStop gradientStop in gradientBrush.GradientStops)
            {
                skColors.Add(ToSkiaColor(gradientStop.Color));
                skiaColorPositions.Add((float)gradientStop.Offset);
            }

            if (gradientBrush is ILinearGradientBrush linearGradientBrush)
            {
                SKPoint skiaStartPoint = GradientBrushPointToSkiaPoint(linearGradientBrush.StartPoint, gradientBrush, uiElement);
                SKPoint skiaEndPoint = GradientBrushPointToSkiaPoint(linearGradientBrush.EndPoint, gradientBrush, uiElement);

                return SKShader.CreateLinearGradient(skiaStartPoint, skiaEndPoint, skColors.ToArray(), skiaColorPositions.ToArray(), tileMode);
            }
            else if (gradientBrush is IRadialGradientBrush radialGradientBrush)
            {
                SKPoint skiaCenterPoint = GradientBrushPointToSkiaPoint(radialGradientBrush.Center, gradientBrush, uiElement);

                float radius = (float)(radialGradientBrush.RadiusX * uiElement.Width);
                return SKShader.CreateRadialGradient(skiaCenterPoint, radius, skColors.ToArray(), skiaColorPositions.ToArray(), tileMode);
            }
            else throw new InvalidOperationException($"GradientBrush type {gradientBrush.GetType()} is unknown");
        }

        public static SKPoint GradientBrushPointToSkiaPoint(Point point, IGradientBrush gradientBrush, IUIElement uiElement)
        {
            if (gradientBrush.MappingMode == BrushMappingMode.RelativeToBoundingBox)
                return new SKPoint(
                    (float)(point.X * uiElement.Width),
                    (float)(point.Y * uiElement.Height));
            else
                return new SKPoint((float)point.X, (float)point.Y);
        }

        private static SKPathFillType FillRuleToSkiaPathFillType(FillRule fillRule)
        {
            return fillRule switch
            {
                FillRule.EvenOdd => SKPathFillType.EvenOdd,
                FillRule.Nonzero => SKPathFillType.Winding,
                _ => throw new InvalidOperationException($"Unknown fillRule value {fillRule}")
            };
        }

        private static SKPoint[] PointsToSkiaPoints(Points points)
        {
            int length = points.Length;
            SKPoint[] skiaPoints = new SKPoint[length];
            for (int i = 0; i < length; i++)
                skiaPoints[i] = new SKPoint((float)points[i].X, (float)points[i].Y);

            return skiaPoints;
        }
    }
}
