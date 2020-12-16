using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace DurableFunction201113
{
    public interface IMyLoggerProvider
    {
        void TestMethod();
        void WriteLog(string messsage);
        void WriteLogProvider(string logMessage, ExecutionContext context);
    }
}