using System.ComponentModel.DataAnnotations.Schema;

namespace christenProject.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        [ForeignKey(nameof(Category))]
        public int? CategoryId { get; set; }
        public CategoryModel? Category { get; set; }
    }
}