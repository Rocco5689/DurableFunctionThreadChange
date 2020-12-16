using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;

namespace DurableFunction201113
{
    public class MyLoggerProvider : IMyLoggerProvider
    {
        private ILogger _logger;
        private int counter;

        public MyLoggerProvider(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        public void TestMethod()
        {
            _logger.LogInformation("HELLO WORLD!!!");
        }

        public void WriteLog(string message)
        {
            _logger.LogInformation(message + " " + counter++);
        }

        public void WriteLogProvider(string logMessage, ExecutionContext context)
        {
            var path = String.Empty;

            try
            {
                path = Environment.GetEnvironmentVariable("HOME");

                if (path == null)
                {
                    path = String.Empty;
                }

                if (!path.ToLower().Contains("home"))
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }

                if (path.ToLower().Contains("home"))
                {
                    path = path + @"\Logfiles";
                }
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
            }

            _logger.LogInformation($"Log Path used by Provider: {path}");

            System.IO.File.AppendAllText(path + @"\" + "TestLoggingFile.txt", $"{DateTime.Now} - InvocId: {context.InvocationId} - {logMessage}\n");
        }
    }
}