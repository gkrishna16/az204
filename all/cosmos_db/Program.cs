// See https://aka.ms/new-console-template for more information
using System;
using Microsoft.Azure.Cosmos;
using Myapp;

namespace cosmos_db // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        private readonly string EndPointUri = "https://cosmosgopal.documents.azure.com:443/";
        private readonly string CosmosKey = "b8c2v5rmpRnSTOh9XEUWcBhfrpQx7tDhG7LJ7izDUldIZnE75eY5lQoWvjn5dbK4uW609SuSxhCsACDbmdZ5gg==";
        private CosmosClient cosmosClient;
        private Database database;
        private Container container;
        private string dbname = "appdb";
        private string containerId = "Orders";
        private string leaseContainerName = "leases";
        public static async Task Main(String[] args)
        {
            try
            {
                Console.WriteLine("Beginning operations...\n");
                Program p = new Program();
                //await p.CosmosAsync();
                //await p.AddItem("01", "Laptop", "12");
                await p.StartChangeProcessor();

            }
            catch (CosmosException de)
            {
                Exception baseException = de.GetBaseException();
                Console.WriteLine("{0} error occurred: {1}", de.StatusCode, de);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
            }
            finally
            {
                Console.WriteLine("End of program, press any key to exit.");
                Console.ReadKey();
            }

        }

        private async Task StartChangeProcessor()
        {
            CosmosClient cosmosClient;
            cosmosClient = new CosmosClient(this.EndPointUri, this.CosmosKey);
            Container leaseContainer = cosmosClient.GetContainer(this.dbname, this.leaseContainerName);
            ChangeFeedProcessor changeFeedProcessor = cosmosClient.GetContainer(dbname, containerId).GetChangeFeedProcessorBuilder<Order>(processorName: "ManageChanges", onChangesDelegate: ManageChanges).WithInstanceName("appHost").WithLeaseContainer(leaseContainer).Build();

            Console.WriteLine("Starting the change feed processor.");
            await changeFeedProcessor.StartAsync();
            Console.Read();
            await changeFeedProcessor.StopAsync();
        }

        private async Task ManageChanges(ChangeFeedProcessorContext context,
        IReadOnlyCollection<Order> itemCollection,
        CancellationToken cancellationToken)
        {
            foreach (Order item in itemCollection)
            {
                Console.WriteLine("Id {0}", item.orderId);
                Console.WriteLine("Order Id {0}", item.orderId);
                Console.WriteLine("Creation time {0}", item.creationTimes);
            }
        }

        private async Task CosmosAsync()
        {
            this.cosmosClient = new CosmosClient(EndPointUri, CosmosKey);
            await this.CreateDatabaseAsync();
            await this.CreateContainerAsync();
        }
        private async Task CreateDatabaseAsync()
        {
            // Create a new database using the cosmosClient
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(dbname);
            Console.WriteLine("Created Database: {0}\n", this.database.Id);
        }
        private async Task CreateContainerAsync()
        {
            // Create a new container
            this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/category");
            Console.WriteLine("Created Container: {0}\n", this.container.Id);
        }

        private async Task AddItem(string orderId, string category, string quantity)
        {
            Order order = new Order()
            {
                id = Guid.NewGuid().ToString(),
                orderId = orderId,
                category = category,
                quantity = quantity
            };

            ItemResponse<Order> response = await this.container.CreateItemAsync<Order>(order, new PartitionKey(order.category));

            Console.WriteLine("Added item with Order Id {0}", orderId);
            Console.WriteLine("Request Units consumed {0}", response.RequestCharge);
        }
    }
}