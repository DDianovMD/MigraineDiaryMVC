namespace MigraineDiary.Web.Models
{
    public class ArticleViewModel
    {
        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string Author { get; set; } = null!;

        public string? SourceUrl { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
