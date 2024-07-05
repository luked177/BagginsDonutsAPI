using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos;
using Container = Microsoft.Azure.Cosmos.Container;
using System.Linq;

namespace BagginsDonutsAPI
{
    public static class GetDonutInfo
    {
        [FunctionName("GetDonutInfo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            DBHandler dbHandler = new DBHandler();
            Container teamMembersContainer = dbHandler.GetTeamMembersContainer();

            bool showArchive = false;
            if (req.Query.ContainsKey("showArchive"))
            {
                bool.TryParse(req.Query["showArchive"], out showArchive);
            }

            Console.WriteLine(showArchive);

            using FeedIterator<TeamMember> feed = teamMembersContainer.GetItemQueryIterator<TeamMember>(
                queryText: "SELECT * FROM c"
            );

            var team = new List<TeamMember> { };

            while (feed.HasMoreResults)
            {
                FeedResponse<TeamMember> response = await feed.ReadNextAsync();

                foreach (TeamMember item in response)
                {
                    if (!showArchive)
                    {
                        int remainder = item.Donuts.Count % 6;
                        item.Donuts = item.Donuts.TakeLast(remainder).ToList();
                    }
                    item.Score = (item.Croissants.Count * 3) - item.Donuts.Count;
                    team.Add(item);
                }
            }

            return new OkObjectResult(team.OrderByDescending(x => x.Score).ToList());
        }
    }
}
