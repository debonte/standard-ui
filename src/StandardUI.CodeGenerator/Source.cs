using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StandardUI.CodeGenerator
{
	public class Source
    {
        public static int IndentSize = 4;
        public static string LineEnding = "\r\n";

        private List<SourceLine> _lines = new List<SourceLine>();
        private int _indent = 0;
        private StringBuilder _buffer = new StringBuilder();

        public void AddLine(string text)
        {
            Add(text);
            EndLine();
        }

        public void AddLine()
		{
            EndLine();
		}

        public void EndLine()
        {
            var sourceLine = new SourceLine(_indent, _buffer.ToString());
            _lines.Add(sourceLine);
            _buffer.Clear();
        }

        public void Add(string text)
		{
            _buffer.Append(text);
		}

        public bool IsEmpty => _lines.Count == 0 && _buffer.Length == 0;

        public IEnumerable<SourceLine> SourceLines => _lines;

        public IndentRestorer Indent()
		{
            IndentRestorer indentRestorer = new IndentRestorer(this, _indent);
            _indent += IndentSize;
            return indentRestorer;
		}

        public void RestoreIndent(int previousIndent)
		{
            _indent = previousIndent;
		}

        public void Add(Source source)
		{
            foreach (SourceLine sourceLine in source._lines)
			{
                if (_indent == 0)
                    _lines.Add(sourceLine);
                else _lines.Add(new SourceLine(_indent + sourceLine.Indent, sourceLine.Text));
			}
		}

        public void Write(StreamWriter stream)
        {
            foreach (SourceLine line in _lines)
                line.Write(stream, LineEnding);
        }

        public void WriteToFile(string directory, string fileName)
		{
            Directory.CreateDirectory(directory);

            string destinationFilePath = Path.Combine(directory, fileName);
            using (StreamWriter stream = File.CreateText(destinationFilePath))
                Write(stream);
        }

        public class IndentRestorer : IDisposable
		{
            private Source _source;
            private int _previousIndent;

            public IndentRestorer(Source source, int previousIndent)
			{
                _source = source;
                _previousIndent = previousIndent;
			}

			public void Dispose()
			{
                _source.RestoreIndent(_previousIndent);
			}
		}
	}
}
