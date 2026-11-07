using System.Globalization;
using System.Text;

namespace CodeInspectionLab11;

public class OrderProcessor
{
    private readonly List<Order> data = new();
    private readonly string outputPath;

    public OrderProcessor(string path)
    {
        outputPath = path;
    }

    public void Add(Order order)
    {
        if (order == null)
        {
            throw new ArgumentNullException(nameof(order));
        }

        data.Add(order);
    }

    public decimal CalculateOrderTotal(Order order)
    {
        decimal total = 0;
        foreach (var item in order.Items)
        {
            total += item.Price * item.Quantity;
        }

        if (order.CustomerType == "vip")
        {
            total = total * 0.90m;
        }
        else if (order.CustomerType == "staff")
        {
            total = total * 0.85m;
        }

        if (total > 10000)
        {
            total = total - 300;
        }

        return total;
    }

    public decimal GetTotal(int id)
    {
        var x = data.FirstOrDefault(d => d.Id == id);
        if (x == null)
        {
            return -1;
        }

        return CalculateOrderTotal(x);
    }

    public decimal GetSumOfAllOrders()
    {
        decimal sum = 0;
        foreach (var order in data)
        {
            sum += CalculateOrderTotal(order);
        }

        return sum;
    }

    public async Task SaveAsync()
    {
        var lines = new List<string>();
        foreach (var x in data)
        {
            var total = CalculateOrderTotal(x);
            var result = $"{x.Id};{x.Customer};{total.ToString(CultureInfo.InvariantCulture)}";
            lines.Add(result);
            Console.WriteLine("Saved order: " + result);
        }

        try
        {
            await File.WriteAllLinesAsync(outputPath, lines);
        }
        catch (IOException ex)
        {
            Console.Error.WriteLine("Failed to save orders: " + ex.Message);
            throw;
        }
    }

    public void Print()
    {
        Console.WriteLine("ORDERS");
        foreach (var x in data)
        {
            Console.WriteLine("Order: " + x.Id + " | " + x.Customer);
            foreach (var item in x.Items)
            {
                Console.WriteLine(item.Name + " - " + item.Quantity + " x " + item.Price);
            }

            Console.WriteLine("Total: " + CalculateOrderTotal(x));
        }
    }

    public string ExportSummary()
    {
        var result = new StringBuilder();
        result.Append("Orders: " + data.Count + Environment.NewLine);
        foreach (var order in data)
        {
            result.Append("#" + order.Id + ": " + CalculateOrderTotal(order) + Environment.NewLine);
        }

        return result.ToString();
    }

    public List<Order> GetOrders()
    {
        return data;
    }
}

public class Order
{
    public int Id { get; set; }
    public string Customer { get; set; } = "";
    public string CustomerType { get; set; } = "regular";
    public List<OrderItem> Items { get; set; } = new();
}

public class OrderItem
{
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
