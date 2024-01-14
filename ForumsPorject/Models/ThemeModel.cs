namespace ForumsPorject.Models
{
    public class ThemeModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public int? Forumid { get; set; }
    }
}
