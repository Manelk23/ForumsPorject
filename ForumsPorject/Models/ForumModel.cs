
namespace ForumsPorject.Models
{
    public class ForumModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime dateCreation { get; set; }
        public string? Discription { get; set; }
        public int CategorieId { get; set; }
    }
}
