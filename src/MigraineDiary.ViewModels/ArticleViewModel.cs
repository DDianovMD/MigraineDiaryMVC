namespace MigraineDiary.ViewModels
{
    public class ArticleViewModel
    {
        /// <summary>
        /// Article's unique identifier.
        /// </summary>
        public string Id { get; set; } = null!;

        /// <summary>
        /// Article's title.
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// Article's content.
        /// </summary>
        public string Content { get; set; } = null!;

        /// <summary>
        /// Article's author(s).
        /// </summary>
        public string Author { get; set; } = null!;

        /// <summary>
        /// Article's source / source url.
        /// </summary>
        public string? SourceUrl { get; set; }

        /// <summary>
        /// Date when article was published in site.
        /// </summary>
        public DateTime CreatedOn { get; set; }
    }
}
