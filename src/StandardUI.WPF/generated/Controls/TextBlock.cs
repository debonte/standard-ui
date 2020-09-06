// This file is generated from ITextBlock.cs. Update the source file to change its contents.

using System.StandardUI.Media;
using System.StandardUI.Wpf.Media;
using System.StandardUI.Controls;
using System.Windows;

namespace System.StandardUI.Wpf.Controls
{
    public class TextBlock : StandardUIElement, ITextBlock
    {
        public static readonly Windows.DependencyProperty ForegroundProperty = PropertyUtils.Register(nameof(Foreground), typeof(Brush), typeof(TextBlock), null);
        public static readonly Windows.DependencyProperty TextProperty = PropertyUtils.Register(nameof(Text), typeof(string), typeof(TextBlock), "");
        public static readonly Windows.DependencyProperty FontSizeProperty = PropertyUtils.Register(nameof(FontSize), typeof(double), typeof(TextBlock), 11.0);
        
        public Brush Foreground
        {
            get => (Brush) GetValue(ForegroundProperty);
            set => SetValue(ForegroundProperty, value);
        }
        IBrush ITextBlock.Foreground
        {
            get => Foreground;
            set => Foreground = (Brush) value;
        }
        
        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        
        public double FontSize
        {
            get => (double) GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public override void OnDraw(IVisualizer visualizer)
        {
            visualizer.DrawTextBlock(this);
        }
    }
}
