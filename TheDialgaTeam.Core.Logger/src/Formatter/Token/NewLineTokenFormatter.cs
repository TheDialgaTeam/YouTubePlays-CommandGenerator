using System;
using System.IO;
using Serilog.Events;
using Serilog.Parsing;

namespace TheDialgaTeam.Core.Logger.Formatter.Token
{
    public class NewLineTokenFormatter : ITokenFormatter
    {
        private readonly PropertyToken _propertyToken;

        public NewLineTokenFormatter(PropertyToken propertyToken)
        {
            _propertyToken = propertyToken;
        }

        public void Format(LogEvent logEvent, TextWriter output)
        {
            AnsiEscapeCodeFormatter.Format(output, Environment.NewLine, _propertyToken);
        }
    }
}