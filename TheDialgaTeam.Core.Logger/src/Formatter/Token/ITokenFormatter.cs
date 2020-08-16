using System.IO;
using Serilog.Events;

namespace TheDialgaTeam.Core.Logger.Formatter.Token
{
    public interface ITokenFormatter
    {
        void Format(LogEvent logEvent, TextWriter output);
    }
}