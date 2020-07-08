using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ParticipationMarker.App.Services.Stats;

namespace ParticipationMarker.FunctionApp
{
    public class UserStats
    {
        private readonly IStatsService _statsService;

        public UserStats(IStatsService statsService)
        {
            _statsService = statsService;
        }

        [FunctionName("UserStats")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            HttpRequest req,
            ILogger log)
        {
            var name = req.Query["chatName"];

            if (string.IsNullOrEmpty(name))
            {
                return new BadRequestObjectResult("chatName is empty");
            }

            var result = await _statsService.GetChatStatAsync(name);
            if (!result.Any())
            {
                return new NotFoundObjectResult($"No stats found for chat {name}");
            }

            var jResult = JsonConvert.SerializeObject(result);
            return new OkObjectResult(jResult);
        }
    }
}