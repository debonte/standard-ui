// This file is generated from ITextBlock.cs. Update the source file to change its contents.

using System.StandardUI.Media;

namespace System.StandardUI.Controls
{
    public static class TextBlockExtensions
    {
        public static T Foreground<T>(this T textBlock, IBrush value) where T : ITextBlock
        {
            textBlock.Foreground = value;
            return textBlock;
        }
        
        public static T Text<T>(this T textBlock, string value) where T : ITextBlock
        {
            textBlock.Text = value;
            return textBlock;
        }
        
        public static T FontSize<T>(this T textBlock, double value) where T : ITextBlock
        {
            textBlock.FontSize = value;
            return textBlock;
        }
    }
}
