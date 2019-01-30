using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

using ExecutionContext = Microsoft.Azure.WebJobs.ExecutionContext;

namespace Azure.DurableFunctions.Bug
{
    public class Runner
    {
        [FunctionName("TriggerViaHttp")]
        public static Task<string> TriggerViaHttpAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "start")] HttpRequest request,
            [OrchestrationClient] DurableOrchestrationClient orchestrationClient,
            ExecutionContext executionContext,
            CancellationToken cancellationToken) =>
            orchestrationClient.StartNewAsync("StartOrchestration", new { });

        [FunctionName("StartOrchestration")]
        public static Task StartOrchestrationAsync(
            [OrchestrationTrigger] DurableOrchestrationContext context,
            ExecutionContext executionContext,
            CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
