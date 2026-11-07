using CodeInspectionLab11;

var reportFile = Path.Combine(Environment.CurrentDirectory, "orders.csv");
var service = new OrderProcessor(reportFile);

service.Add(new Order
{
    Id = 1,
    Customer = "Анна",
    CustomerType = "vip",
    Items = new List<OrderItem>
    {
        new() { Name = "Клавиатура", Price = 2500m, Quantity = 2 },
        new() { Name = "Мышь", Price = 1500m, Quantity = 1 }
    }
});

service.Add(new Order
{
    Id = 2,
    Customer = "Иван",
    CustomerType = "regular",
    Items = new List<OrderItem>
    {
        new() { Name = "Монитор", Price = 12000m, Quantity = 1 }
    }
});

service.Print();
Console.WriteLine(service.ExportSummary());
Console.WriteLine("Total all orders: " + service.GetSumOfAllOrders());
await service.SaveAsync();
Console.WriteLine("Finished. Verify orders.csv in the current directory.");
