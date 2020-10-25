using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NumberService
{
    public class PutNumber
    {
        private static readonly string _clientId = Guid.NewGuid().ToString("N");
        private readonly TelemetryClient _telemetry;
        private readonly Container _container;

        public PutNumber(TelemetryClient telemetry)
        {
            _telemetry = telemetry;
        }

        [FunctionName("PutNumber")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "numbers/{key:alpha}")] HttpRequest req,
            string key,
            ILogger log)
        {
            
            var number = new {
            	Number = "test",
                ETag = "etag",
                Key = "Key",
                ClientId = "ClientId"
            };

            log.LogInformation($"Number {number.Number} issued to clientId {number.ClientId} with ETag {number.ETag} from key {number.Key}");

            _telemetry.TrackEvent(
                "PutNumber",
                properties: new Dictionary<string, string>
                    {
                        { "Number","123" },
                        { "ClientId", "_clientId" },
                        { "Key", "1" },
                        { "ETag", "number.ETag" }
                    },
                metrics: new Dictionary<string, double>
                    {
                        { "CosmosRequestCharge", 0 }
                    });

            return new OkObjectResult(number);
        }
    }
}
