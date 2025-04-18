using System.ComponentModel.DataAnnotations;

namespace Mini_Market.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
