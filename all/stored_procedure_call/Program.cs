using Microsoft.Azure.Cosmos;
string cosmosDBEndpointUri = "https://gopalcosmosdb.documents.azure.com:443/";
string cosmosDBKey = "BtdI6FOjkCYwwqg2SOXrbFcI3ck4FFv0lILM4Al8w6VvJnzr1Bgqdtrziy1aabBq50L3aqh0Ge9KACDb4l2usA==";
string databaseName = "appdb";
string containerName = "Orders";

// await CreateDatabase("appdb");
CosmosClient cosmosClient;
cosmosClient = new CosmosClient(cosmosDBEndpointUri, cosmosDBKey);
Container container = cosmosClient.GetContainer(databaseName, containerName);

await CreateItem();
async Task CreateItem()
{
    CosmosClient cosmosClient;
    cosmosClient = new CosmosClient(cosmosDBEndpointUri, cosmosDBKey);
    Container container = cosmosClient.GetContainer(databaseName, containerName);
    dynamic orderItem = new { id = Guid.NewGuid().ToString(), orderId = "01", category = "Laptop" };
    await container.CreateItemAsync(orderItem, null, new ItemRequestOptions { PreTriggers = new List<string> { "validateItem" } });
    Console.WriteLine("Item has been inserted.");
}

//async Task CallStoredProcedure()
//{

//    dynamic[] orderItems = new dynamic[]
//{
//    new {
//        id = Guid.NewGuid().ToString(),
//        orderId = "01",
//        category  = "Laptop",
//        quantity  = 100
//    },
//    new {
//        id = Guid.NewGuid().ToString(),
//        orderId = "02",
//        category  = "Laptop",
//        quantity  = 200
//    },
//    new {
//        id = Guid.NewGuid().ToString(),
//        orderId = "03",
//        category  = "Laptop",
//        quantity  = 75
//    },
//};

//    var result = await container.Scripts.ExecuteStoredProcedureAsync<string>("createItems", new PartitionKey("Laptop"), new[] { orderItems });

//    Console.WriteLine(result);


//}

