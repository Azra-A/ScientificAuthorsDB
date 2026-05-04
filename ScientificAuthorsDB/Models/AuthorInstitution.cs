namespace ScientificAuthorsDB.Models
{
    public class AuthorInstitution
    {
        public int AuthorId { get; set; }
        public Author Author { get; set; } = null!;

        public int InstitutionId { get; set; }
        public Institution Institution { get; set; } = null!;

        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
        public string? Role { get; set; }
    }
}