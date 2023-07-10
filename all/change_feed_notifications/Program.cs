// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using System;
using Microsoft.Azure.Cosmos;
using MyApp;

string cosmosDBEndpointUri = "https://cosmosgopal.documents.azure.com:443/";
string cosmosDBKey = "b8c2v5rmpRnSTOh9XEUWcBhfrpQx7tDhG7LJ7izDUldIZnE75eY5lQoWvjn5dbK4uW609SuSxhCsACDbmdZ5gg==";
string databaseName = "appdb";
string sourcecontainerName = "Orders";
string leasecontainerName = "lease";

await StartChangeProcessor();
async Task StartChangeProcessor()
{
    CosmosClient cosmosClient;
    cosmosClient = new CosmosClient(cosmosDBEndpointUri, cosmosDBKey);

    Container leaseContainer = cosmosClient.GetContainer(databaseName, leasecontainerName);

    ChangeFeedProcessor changeFeedProcessor = cosmosClient.GetContainer(databaseName, sourcecontainerName)
        .GetChangeFeedProcessorBuilder<Order>(processorName: "ManageChanges", onChangesDelegate: ManageChanges)
        .WithInstanceName("appHost")
        .WithLeaseContainer(leaseContainer)
        .Build();

    Console.WriteLine("Starting the Change Feed Processor");
    await changeFeedProcessor.StartAsync();
    Console.Read();
    await changeFeedProcessor.StopAsync();
}

static async Task ManageChanges(
    ChangeFeedProcessorContext context,
    IReadOnlyCollection<Order> itemCollection,
    CancellationToken cancellationToken)
{
    foreach (Order item in itemCollection)
    {
        Console.WriteLine("Id {0}", item.id);
        Console.WriteLine("Order Id {0}", item.orderId);
        Console.WriteLine("Creation Time {0}", item.creationTime);
        await Task.Delay(10);
    }
}