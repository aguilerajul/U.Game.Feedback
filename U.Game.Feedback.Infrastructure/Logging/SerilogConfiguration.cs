using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;

namespace U.Game.Feedback.Infrastructure.Logging
{
    public static class SerilogConfiguration
    {
        public static Logger GetNewLoggerToFileConfiguration(string logName)
        {
            return new LoggerConfiguration()
                   .MinimumLevel.Debug()
                   .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                   .Enrich.FromLogContext()
                   .WriteTo.File(System.IO.Path.Combine(Environment.CurrentDirectory, $"Logs\\{logName}.log"))
                   .CreateLogger();
        }
    }
}
