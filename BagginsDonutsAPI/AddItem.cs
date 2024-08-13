using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Container = Microsoft.Azure.Cosmos.Container;

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

            var partitionKey = new PartitionKey(user.UserId);
            ItemResponse<TeamMember> response = await teamMembersContainer.ReadItemAsync<TeamMember>(user.Id, partitionKey);
            TeamMember currentItem = response.Resource;

            if (currentItem == null) return new NotFoundObjectResult("No team member found");

            var newAward = new Award(reason);
            List<PatchOperation> patchOperations = new List<PatchOperation>();

            switch (type.ToLower())
            {
                case "donut":
                    currentItem.Donuts.Add(newAward);
                    patchOperations.Add(PatchOperation.Replace("/Donuts", currentItem.Donuts));
                    break;
                case "croissant":
                    currentItem.Croissants.Add(newAward);
                    patchOperations.Add(PatchOperation.Replace("/Croissants", currentItem.Croissants));
                    break;
                default:
                    return new BadRequestObjectResult("Invalid type. Please use 'donut' or 'croissant'");
            }

            await teamMembersContainer.PatchItemAsync<TeamMember>(user.Id, partitionKey, patchOperations);
            string responseMessage = $"Added a {type} for user {currentItem.Name} with id {newAward.AwardId}";
            return new OkObjectResult(responseMessage);
        }
    }
}
