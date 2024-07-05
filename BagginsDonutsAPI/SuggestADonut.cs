using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json.Serialization;

namespace BagginsDonutsAPI
{
    public static class SuggestADonut
    {
        static JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            }
        };
        [FunctionName("SuggestADonut")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            DBHandler dbHandler = new DBHandler();
            Container suggestionsContainer = dbHandler.GetSuggestionsContainer();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody, _serializerSettings);
            string usersName = data?.UsersName;
            string nomineesName = data?.Nominee;
            string reason = data?.Reason;

            if(String.IsNullOrWhiteSpace(usersName) || String.IsNullOrWhiteSpace(nomineesName) || String.IsNullOrWhiteSpace(reason)) {
                return new BadRequestObjectResult("Required properties are missing from the request body.");
            }

            var suggestion = new DonutSuggestion(usersName, nomineesName, reason);

            await suggestionsContainer.CreateItemAsync(suggestion);

            return new OkObjectResult($"{usersName} requested donut for {nomineesName} for {reason}");
        }
    }
}
