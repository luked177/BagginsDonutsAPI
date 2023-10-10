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

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string id = data?.id;
            string uid = data?.userId;
            string type = data?.type;


            if (String.IsNullOrWhiteSpace(id) || String.IsNullOrWhiteSpace(uid) || String.IsNullOrWhiteSpace(type))
            {
                return new BadRequestObjectResult("Required properties are missing from the request body.");
            }


            var partitionKey = new Microsoft.Azure.Cosmos.PartitionKey(uid);

            ItemResponse<TeamMember> response = await teamMembersContainer.ReadItemAsync<TeamMember>(id, partitionKey);
            TeamMember currentItem = response.Resource;

            if (currentItem == null) return new NotFoundObjectResult("No team member found");

            
            

            if (string.Equals(type, "croissant", StringComparison.OrdinalIgnoreCase))
            {
                if (currentItem.Croissants == 0) return new BadRequestObjectResult("Croissants cannot fall below 0");
                currentItem.Croissants--;
            }

            if (string.Equals(type, "donut", StringComparison.OrdinalIgnoreCase))
            {
                if (currentItem.Donuts == 0) return new BadRequestObjectResult("Donuts cannot fall below 0");
                currentItem.Donuts--;
            }

            await teamMembersContainer.ReplaceItemAsync(currentItem, id, partitionKey);

            string responseMessage = $"Deleted a {type} for user {currentItem.Name}";

            return new OkObjectResult(responseMessage);
        }
    }
}
