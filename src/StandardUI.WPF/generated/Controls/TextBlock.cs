// This file is generated from ITextBlock.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using Microsoft.StandardUI.Wpf.Media;
using Microsoft.StandardUI.Text;
using Microsoft.StandardUI.Wpf.Text;
using Microsoft.StandardUI.Controls;

namespace Microsoft.StandardUI.Wpf.Controls
{
    public class TextBlock : StandardUIFrameworkElement, ITextBlock
    {
        public static readonly System.Windows.DependencyProperty ForegroundProperty = PropertyUtils.Register(nameof(Foreground), typeof(Brush), typeof(TextBlock), null);
        public static readonly System.Windows.DependencyProperty TextProperty = PropertyUtils.Register(nameof(Text), typeof(string), typeof(TextBlock), "");
        public static readonly System.Windows.DependencyProperty FontStyleProperty = PropertyUtils.Register(nameof(FontStyle), typeof(FontStyle), typeof(TextBlock), FontStyle.Normal);
        public static readonly System.Windows.DependencyProperty FontWeightProperty = PropertyUtils.Register(nameof(FontWeight), typeof(FontWeightWpf), typeof(TextBlock), FontWeightWpf.Default);
        public static readonly System.Windows.DependencyProperty FontSizeProperty = PropertyUtils.Register(nameof(FontSize), typeof(double), typeof(TextBlock), 11.0);
        public static readonly System.Windows.DependencyProperty TextAlignmentProperty = PropertyUtils.Register(nameof(TextAlignment), typeof(TextAlignment), typeof(TextBlock), TextAlignment.Left);
        
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
        
        public override void Draw(IDrawingContext visualizer) => visualizer.DrawTextBlock(this);
    }
}
