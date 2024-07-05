using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

public class DBHandler
{
    private readonly CosmosClient _client;
    private readonly Database _database;
    private readonly Container _teamMembersContainer;
    private readonly Container _suggestionsContainer;

    public DBHandler()
    {
        _client = new CosmosClient(Environment.GetEnvironmentVariable("AzureWebJobsCosmosDBConnectionString"), new CosmosClientOptions {SerializerOptions = new CosmosSerializationOptions { IgnoreNullValues = true, PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase} });
        _database = _client.GetDatabase("BagginsDonuts");
        _teamMembersContainer = _database.GetContainer("TeamMembers");
        _suggestionsContainer = _database.GetContainer("Suggestions");
    }

    public Container GetTeamMembersContainer()
    {
        return _teamMembersContainer;
    }

    public Container GetSuggestionsContainer()
    {
        return _suggestionsContainer;
    }
}
