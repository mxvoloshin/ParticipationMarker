using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ParticipationMarker.App.TelegramBot;

namespace ParticipationMarker.FunctionApp
{
    public class BotWebHook
    {
        private readonly ITelegramInputParser _telegramInputParser;

        public BotWebHook(ITelegramInputParser telegramInputParser)
        {
            _telegramInputParser = telegramInputParser;
        }

        [FunctionName("BotWebHook")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            HttpRequest req,
            ILogger log)
        {
            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                log.LogInformation(requestBody);

                await _telegramInputParser.ParseRequestAsync(requestBody);

                return new OkObjectResult("WebHook is working");
            }
            catch (Exception ex)
            {
                log.LogError("BotWebHook failed");
                log.LogError(ex.Message);
                log.LogError(ex.StackTrace);

                return new OkObjectResult("WebHook is failed");
            }
        }
    }
}