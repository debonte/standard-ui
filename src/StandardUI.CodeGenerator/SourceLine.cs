using System.IO;

namespace StandardUI.CodeGenerator
{
	public class SourceLine
    {
        public int Indent { get; }
        public string Text { get; }

        public SourceLine(int indent, string text)
        {
            Indent = indent;
            Text = text;
        }

        public void Write(StreamWriter stream, string lineEnding)
        {
            for (int i = 0; i < Indent; ++i)
                stream.Write(' ');
            stream.Write(Text);
            stream.Write(lineEnding);
        }
    }
}
