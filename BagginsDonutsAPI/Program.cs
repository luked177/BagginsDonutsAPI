//using Azure.Identity;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Configuration.AzureAppConfiguration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using StreetsHeaver.Common.Crypto;
//using StreetsHeaver.ReportGenerator.Cosmos;
//using StreetsHeaver.ReportGenerator.Models;
//using AdminFunction.Services;
//using AdminFunction.Services.Interfaces;
//using System.Text.Json;

//var host = new HostBuilder()
//    .ConfigureFunctionsWorkerDefaults()
//    .ConfigureAppConfiguration(builder =>
//    {
//        builder.AddAzureAppConfiguration(config =>
//            config.Connect()
//            .ConfigureKeyVault(kv => kv.SetCredential(new DefaultAzureCredential()))
//            .Select("rg:CosmosDbOptions:*", labelFilter: LabelFilter.Null)
//            .Select("rg:AesEncryptionOptions:*", labelFilter: LabelFilter.Null)
//            .TrimKeyPrefix("rg:")
//            );
//    })
//    .ConfigureServices((builder, services) =>
//    {
//        //services.AddSingleton<IAesEncryption2, AesEncryption2>();

//        services.Configure<JsonSerializerOptions>(options =>
//        {
//            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
//        });
//    })
//    .Build();

//host.Run();
