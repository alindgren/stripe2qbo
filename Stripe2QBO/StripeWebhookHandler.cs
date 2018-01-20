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

            // parse query parameter
            //string name = req.GetQueryNameValuePairs()
            //    .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
            //    .Value;

            // Get request body
            string data = await req.Content.ReadAsStringAsync();
            string stripeSignatureHeader = req.Headers.GetValues("Stripe-Signature").FirstOrDefault();
            string secret = "whsec_peq5q5vfcN401gtRI7BYgUwZKFedCTqG";
            var stripeEvent = Stripe.StripeEventUtility.ConstructEvent(data, stripeSignatureHeader, secret);
            log.Info("type: " + stripeEvent.Type);
            log.Info("amount:  " + stripeEvent.Data.Object["amount"]);

            //if (stripeEvent.Data.Object["object"] == "charge")
            //{

            //}
            //object": "charge

           // log.Info("type: " + data.type);

            //// Set name to query string or body data
            //name = name ?? data?.name;

            //return name == null
            //    ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
            //    : req.CreateResponse(HttpStatusCode.OK, "Hello " + name);
            return req.CreateResponse(HttpStatusCode.OK, "OK");
        }
    }
}
