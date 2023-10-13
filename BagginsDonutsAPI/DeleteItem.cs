using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Container = Microsoft.Azure.Cosmos.Container;
using Microsoft.Azure.Cosmos;

namespace BagginsDonutsAPI
{
    public static class DeleteItem
    {
        [FunctionName("DeleteItem")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = null)] HttpRequest req,
            ILogger log)
        {
            DBHandler dbHandler = new DBHandler();
            Container teamMembersContainer = dbHandler.GetTeamMembersContainer();
            MapNameToIds nameToIds = new MapNameToIds();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string name = data?.name;
            string type = data?.type;
            var user = nameToIds.GetUserDetails(name);
            Guid awardToRemove = data?.awardToRemove;



            if (String.IsNullOrWhiteSpace(name) || String.IsNullOrWhiteSpace(user.UserId) || String.IsNullOrWhiteSpace(type))
            {
                return new BadRequestObjectResult("Required properties are missing from the request body.");
            }


            var partitionKey = new Microsoft.Azure.Cosmos.PartitionKey(user.UserId);


            ItemResponse<TeamMember> response = await teamMembersContainer.ReadItemAsync<TeamMember>(user.Id, partitionKey);
            TeamMember currentItem = response.Resource;

            if (currentItem == null) return new NotFoundObjectResult("No team member found");



            if (type.ToLower() == "donut")
            {
                bool donutExists = false;

                foreach (var donut in currentItem.Donuts)
                {
                    if (donut.AwardId == awardToRemove)
                    {
                        donutExists = true;
                        break;
                    }
                }

                if(!donutExists) return new NotFoundObjectResult("Specified Donut not found");
                currentItem.Donuts.RemoveAll(donut => donut.AwardId == awardToRemove);
            }

            if (type.ToLower() == "croissant")
            {
                bool croissantExists = false;

                foreach (var croissant in currentItem.Croissants)
                {
                    if (croissant.AwardId == awardToRemove)
                    {
                        croissantExists = true;
                        break;
                    }
                }
                if (!croissantExists) return new NotFoundObjectResult("Specified Croissant not found");
                currentItem.Croissants.RemoveAll(croissant => croissant.AwardId == awardToRemove);
            }


            await teamMembersContainer.ReplaceItemAsync(currentItem, user.Id, partitionKey);

            string responseMessage = $"Deleted a {type} for user {currentItem.Name}";

            return new OkObjectResult(responseMessage);
        }
    }
}
