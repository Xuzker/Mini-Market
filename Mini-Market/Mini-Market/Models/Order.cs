namespace Mini_Market.Models
{
    public class Order
    {
        public int Id { get; set; }
        public List<int> ProductIds { get; set; } = new();
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
