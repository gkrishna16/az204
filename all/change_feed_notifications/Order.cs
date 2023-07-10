// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
namespace MyApp
{
    public class Order
    {
        public string id { get; set; }
        public string orderId { get; set; }
        public string category { get; set; }
        public string quantity { get; set; }
        public DateTime creationTime { get; set; }
    }
}