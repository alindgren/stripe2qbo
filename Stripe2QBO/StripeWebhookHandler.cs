using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace Stripe2QBO
{
    public static class StripeWebhookHandler
    {
        [FunctionName("StripeWebhookHandler")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("StripeWebhookHandler - C# HTTP trigger function processed a request.");

            // Get request body
            var data = await req.Content.ReadAsStringAsync();
            var stripeSignatureHeader = req.Headers.GetValues("Stripe-Signature").FirstOrDefault();
            var secret = System.Configuration.ConfigurationManager.AppSettings["StripeWebhookSecret"];
            var stripeEvent = Stripe.StripeEventUtility.ConstructEvent(data, stripeSignatureHeader, secret);
            log.Info("type: " + stripeEvent.Type);
            log.Info("amount:  " + stripeEvent.Data.Object["amount"]);

            return req.CreateResponse(HttpStatusCode.OK, "OK");
        }
    }
}
