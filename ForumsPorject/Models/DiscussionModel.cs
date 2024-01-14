
namespace ForumsPorject.Models
{
    public class DiscussionModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
       public DateTime DateCreationDiscussion {  get; set; }

      
        public int? ThemeId { get; set; }

    
        public int? Utilisateurid { get; set; }

    }
}
