namespace ScientificAuthorsDB.Models
{
    public class AuthorPublication
    {
        public int AuthorId { get; set; }
        public Author Author { get; set; } = null!;

        public int PublicationId { get; set; }
        public Publication Publication { get; set; } = null!;

        public string? ContributionRole { get; set; }
        public int? AuthorOrder { get; set; }
    }
}