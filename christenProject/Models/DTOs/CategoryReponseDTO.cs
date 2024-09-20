namespace christenProject.Models.DTOs
{
    public class CategoryReponseDTO
    {
        public int CId { get; set; }
        public string  CName { get; set; }
        public List<string> CProductsNames { get; set; } = new();
    }
}
