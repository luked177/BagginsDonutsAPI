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
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;

namespace BagginsDonutsAPI
{
    public static class GetLastWeeksSuggestions
    {
        [FunctionName("GetLastWeeksSuggestions")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            DBHandler dbHandler = new DBHandler();
            Container suggestionsContainer = dbHandler.GetSuggestionsContainer();

            DateTime now = DateTime.Now;
            int daysUntilFriday = ((int)DayOfWeek.Friday - (int)now.DayOfWeek - 7) % 7;
            DateTime lastFriday = now.AddDays(daysUntilFriday).Date.AddHours(9);

            var query = $"SELECT * FROM c WHERE c.suggestionDate > @Date";
            var queryDefinition = new QueryDefinition(query).WithParameter("@Date", lastFriday);

            using FeedIterator<DonutSuggestion> feed = suggestionsContainer.GetItemQueryIterator<DonutSuggestion>(queryDefinition);

            var suggestions = new List<DonutSuggestion> { };

            while (feed.HasMoreResults)
            {
                FeedResponse<DonutSuggestion> response = await feed.ReadNextAsync();

                foreach (DonutSuggestion item in response)
                {
                    suggestions.Add(item);
                }
            }

            return new OkObjectResult(suggestions.OrderByDescending(x => x.SuggestionDate));
        }
    }
}
