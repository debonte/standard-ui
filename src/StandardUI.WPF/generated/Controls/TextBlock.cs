// This file is generated from ITextBlock.cs. Update the source file to change its contents.

using System.StandardUI.Media;
using System.StandardUI.Wpf.Media;
using System.StandardUI.Text;
using System.StandardUI.Wpf.Text;
using System.StandardUI.Controls;
using System.Windows;

namespace System.StandardUI.Wpf.Controls
{
    public class TextBlock : StandardUIElement, ITextBlock
    {
        public static readonly Windows.DependencyProperty ForegroundProperty = PropertyUtils.Register(nameof(Foreground), typeof(Brush), typeof(TextBlock), null);
        public static readonly Windows.DependencyProperty TextProperty = PropertyUtils.Register(nameof(Text), typeof(string), typeof(TextBlock), "");
        public static readonly Windows.DependencyProperty FontStyleProperty = PropertyUtils.Register(nameof(FontStyle), typeof(FontStyle), typeof(TextBlock), FontStyle.Normal);
        public static readonly Windows.DependencyProperty FontWeightProperty = PropertyUtils.Register(nameof(FontWeight), typeof(FontWeightWpf), typeof(TextBlock), FontWeightWpf.Default);
        public static readonly Windows.DependencyProperty FontSizeProperty = PropertyUtils.Register(nameof(FontSize), typeof(double), typeof(TextBlock), 11.0);
        public static readonly Windows.DependencyProperty TextAlignmentProperty = PropertyUtils.Register(nameof(TextAlignment), typeof(TextAlignment), typeof(TextBlock), TextAlignment.Left);
        
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
        
        public FontStyle FontStyle
        {
            get => (FontStyle) GetValue(FontStyleProperty);
            set => SetValue(FontStyleProperty, value);
        }
        
        public FontWeightWpf FontWeight
        {
            get => (FontWeightWpf) GetValue(FontWeightProperty);
            set => SetValue(FontWeightProperty, value);
        }
        FontWeight ITextBlock.FontWeight
        {
            get => FontWeight.FontWeight;
            set => FontWeight = new FontWeightWpf(value);
        }
        
        public double FontSize
        {
            get => (double) GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }
        
        public TextAlignment TextAlignment
        {
            get => (TextAlignment) GetValue(TextAlignmentProperty);
            set => SetValue(TextAlignmentProperty, value);
        }
    }
}
