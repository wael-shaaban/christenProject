namespace christenProject.Models
{
    public class CategoryModel
    {
        public CategoryModel()
        {
            Products = new HashSet<ProductModel>(); 
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ProductModel>? Products { get; set; }
    }
}
