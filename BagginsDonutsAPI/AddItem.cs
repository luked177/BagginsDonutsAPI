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
    public static class AddItem
    {
        [FunctionName("AddItem")]
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
            string reason = data?.reason;


            if(string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(user.UserId) || string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(reason))
            {
                return new BadRequestObjectResult("Required properties are missing from the request body.");
            }


            var partitionKey = new Microsoft.Azure.Cosmos.PartitionKey(user.UserId);
            ItemResponse<TeamMember> response = await teamMembersContainer.ReadItemAsync<TeamMember>(user.Id, partitionKey);
            TeamMember currentItem = response.Resource;

            if (currentItem == null) return new NotFoundObjectResult("No team member found");

            Guid guid = Guid.NewGuid();

            if(type.ToLower() == "donut")
            {
                currentItem.Donuts.Add(new Award
                {
                    AwardedDate = DateTime.Now,
                    AwardedReason = reason,
                    AwardId = guid,
                });
            }

            if (type.ToLower() == "croissant")
            {
                currentItem.Croissants.Add(new Award
                {
                    AwardedDate = DateTime.Now,
                    AwardedReason = reason,
                    AwardId = guid,
                });
            }


            await teamMembersContainer.ReplaceItemAsync(currentItem, user.Id, partitionKey);

            string responseMessage = $"Added a {type} for user {currentItem.Name} with id {guid}";
            return new OkObjectResult(responseMessage);
        }
    }
}
