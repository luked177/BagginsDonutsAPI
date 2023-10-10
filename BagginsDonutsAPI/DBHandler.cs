using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

public class DBHandler
{
    private readonly CosmosClient _client;
    private readonly Database _database;
    private readonly Container _teamMembersContainer;

    public DBHandler()
    {
        _client = new CosmosClient(Environment.GetEnvironmentVariable("AzureWebJobsCosmosDBConnectionString"));
        _database = _client.GetDatabase("BagginsDonuts");
        _teamMembersContainer = _database.GetContainer("TeamMembers");
    }

    public Container GetTeamMembersContainer()
    {
        return _teamMembersContainer;
    }
}
