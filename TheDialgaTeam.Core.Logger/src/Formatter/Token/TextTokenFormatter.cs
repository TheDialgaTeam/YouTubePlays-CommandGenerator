using System.IO;
using Serilog.Events;

namespace TheDialgaTeam.Core.Logger.Formatter.Token
{
    public class TextTokenFormatter : ITokenFormatter
    {
        private readonly string _text;

        public TextTokenFormatter(string text)
        {
            _text = text;
        }

        public void Format(LogEvent logEvent, TextWriter output)
        {
            AnsiEscapeCodeFormatter.Format(output, _text);
        }
    }
}