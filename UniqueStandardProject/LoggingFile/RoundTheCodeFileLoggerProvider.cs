using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;

namespace UniqueStandardProject.LoggingFile
{
    [ProviderAlias("RoundTheCodeFile")]
    public class RoundTheCodeFileLoggerProvider : ILoggerProvider
    {
        public readonly RoundTheCodeFileLoggerOptions Options;

        public RoundTheCodeFileLoggerProvider(IOptions<RoundTheCodeFileLoggerOptions> _options)
        {
            Options = _options.Value;

            if (!Directory.Exists(Options.FolderPath))
            {
                Directory.CreateDirectory(Options.FolderPath);
            }
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new RoundTheCodeFileLogger(this);
        }

        public void Dispose()
        {
        }
    }
}
