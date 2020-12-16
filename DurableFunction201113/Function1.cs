using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace DurableFunction201113
{
    public class Function1
    {
        private readonly ILogger logger;
        private readonly IMyLoggerProvider _otherClass;

        public Function1(ILogger<Function1> _logger, IMyLoggerProvider otherClass)
        {
            logger = _logger;
            _otherClass = otherClass;
        }

        [FunctionName("Function1")]
        public async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            logger.LogInformation("Orchestration beginning!!!", context.InstanceId);

            var outputs = new List<string>();
            // Replace "hello" with the name of your Durable Activity Function.

            //Task.Factory.StartNew(() =>
            //{
            //    _otherClass.WriteLog("Before Tokyo" + " ManagedThreadID: " + Thread.CurrentThread.ManagedThreadId + " | ");
            //});
            //testMethod();
            logger.LogInformation("Before Tokyo" + " ManagedThreadID: " + Thread.CurrentThread.ManagedThreadId);
            outputs.Add(await context.CallActivityAsync<string>("Function1_Hello", "Tokyo"));

            //Task.Factory.StartNew(() =>
            //{
            //    _otherClass.WriteLog("Before Seattle" + " ManagedThreadID: " + Thread.CurrentThread.ManagedThreadId + " | ");
            //});
            //testMethod("Before Seattle");
            logger.LogInformation("Before Seattle" + " ManagedThreadID: " + Thread.CurrentThread.ManagedThreadId);
            outputs.Add(await context.CallActivityAsync<string>("Function1_Hello", "Seattle"));

            //Task.Factory.StartNew(() =>
            //{
            //    _otherClass.WriteLog("Before London" + " ManagedThreadID: " + Thread.CurrentThread.ManagedThreadId + " | ");
            //});
            //testMethod("Before London");
            logger.LogInformation("Before London" + " ManagedThreadID: " + Thread.CurrentThread.ManagedThreadId);
            outputs.Add(await context.CallActivityAsync<string>("Function1_Hello", "London"));

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]

            //Task.Factory.StartNew(() =>
            //{
            //    _otherClass.WriteLog("Before return!!!" + " ManagedThreadID: " + Thread.CurrentThread.ManagedThreadId + " | ");
            //});
            //testMethod("Before return!!!");
            logger.LogInformation("Before return!!!" + " ManagedThreadID: " + Thread.CurrentThread.ManagedThreadId);
            return outputs;
        }

        [FunctionName("Function1_Hello")]
        public string SayHello([ActivityTrigger] string name, ILogger log, Microsoft.Azure.WebJobs.ExecutionContext context)
        {
            logger.LogInformation("SayHello beginning " + " " + context.InvocationId);

            log.LogInformation($"Saying hello to {name}.");
            return $"Hello {name}!";
        }

        [FunctionName("Function1_HttpStart")]
        public async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            log.LogInformation($"StartNewAsync method called!!!");
            string instanceId = await starter.StartNewAsync("Function1", null);

            logger.LogInformation($"Started orchestration with ID = '{instanceId}'.");
            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            for (int x = 0; x < 1000; x++)
            {
                System.Threading.Thread.SpinWait(100000);
            }
            
            return starter.CreateCheckStatusResponse(req, instanceId);
        }

        public void testMethod(string message)
        {
            Task.Factory.StartNew(() =>
            {
                _otherClass.WriteLog(message);
            });
        }
    }
}