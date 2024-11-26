using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System;

namespace UniqueStandardProject.LoggingFile
{
    public class RoundTheCodeFileLogger : ILogger
    {
        protected readonly RoundTheCodeFileLoggerProvider _roundTheCodeLoggerFileProvider;

        public RoundTheCodeFileLogger([NotNull] RoundTheCodeFileLoggerProvider roundTheCodeLoggerFileProvider)
        {
            _roundTheCodeLoggerFileProvider = roundTheCodeLoggerFileProvider;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            string fullFilePath = _roundTheCodeLoggerFileProvider.Options.FolderPath + "/" + _roundTheCodeLoggerFileProvider.Options.FilePath.Replace("{date}", DateTime.Now.ToString("yyyyMMdd"));
            string logRecord = string.Format("{0} [{1}] {2} {3}", "[" + DateTime.Now.ToString("yyyy/MMM/dd HH:mm:ss.fff") + "]", logLevel.ToString(), formatter(state, exception), exception != null ? exception.StackTrace : "");

            using StreamWriter streamWriter = new(fullFilePath, true);
            streamWriter.WriteLine(logRecord);
        }
    }
}
