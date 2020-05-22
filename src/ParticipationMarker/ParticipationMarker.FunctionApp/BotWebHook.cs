using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ParticipationMarker.TelegramBot;

namespace ParticipationMarker.FunctionApp
{
    public static class BotWebHook
    {
        private const string botKey = "1070334956:AAEwa5k3bhOVJjOKOCV9ZU2uC7WwHJMIhHI";
        
        [FunctionName("BotWebHook")]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            HttpRequest req, ILogger log)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation(requestBody);
            
            InputParser.ParseRequest(requestBody);
            
            return new OkObjectResult("WebHook is working");
        }
    }
}