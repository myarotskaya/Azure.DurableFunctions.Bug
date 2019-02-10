using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
        [FunctionName("NonDurableFunction")]
        public static Task<HttpResponseMessage> NonDurableFunctionAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "some")] HttpRequest request,
            ExecutionContext executionContext,
            CancellationToken cancellationToken) =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(request.Headers.FirstOrDefault().Value, Encoding.UTF8, "application/json")
            });

        [FunctionName("TriggerViaHttp")]
        public static async Task<HttpResponseMessage> TriggerViaHttpAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "start")] HttpRequest request,
            [OrchestrationClient] DurableOrchestrationClient orchestrationClient,
            ExecutionContext executionContext,
            CancellationToken cancellationToken)
        {
            await orchestrationClient.StartNewAsync("StartOrchestration", new { });

            return new HttpResponseMessage(HttpStatusCode.OK);
        }


        [FunctionName("StartOrchestration")]
        public static Task StartOrchestrationAsync(
            [OrchestrationTrigger] DurableOrchestrationContext context,
            ExecutionContext executionContext,
            CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
