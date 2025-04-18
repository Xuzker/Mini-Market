using Mini_Market.Models;

namespace Mini_Market.Data
{
    public interface IDataStore
    {
        List<Product> Products { get; }
        List<Category> Categories { get; }
        List<Order> Orders { get; }
    }

}