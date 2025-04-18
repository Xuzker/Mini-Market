using Mini_Market.Models;

namespace Mini_Market.Data
{
    public class InMemoryDataStore : IDataStore
    {
        public List<Product> Products { get; } = new();
        public List<Category> Categories { get; } = new();
        public List<Order> Orders { get; } = new();
    }


}
