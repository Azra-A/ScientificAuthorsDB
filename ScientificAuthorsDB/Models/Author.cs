using System.ComponentModel.DataAnnotations;

namespace ScientificAuthorsDB.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Първото име е задължително")]
        [StringLength(100)]
        [Display(Name = "Първо име")]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "Фамилията е задължителна")]
        [StringLength(100)]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = "";

        [EmailAddress]
        [Display(Name = "Имейл")]
        public string? Email { get; set; }

        [Display(Name = "ORCID")]
        public string? OrcidId { get; set; }

        [Display(Name = "Година на рождение")]
        [Range(1900, 2010)]
        public int? BirthYear { get; set; }

        [Display(Name = "Пълно име")]
        public string FullName => $"{FirstName} {LastName}";

        // Връзки с другите таблици (Many-to-Many)
        public ICollection<AuthorInstitution> AuthorInstitutions { get; set; } = new List<AuthorInstitution>();
        public ICollection<AuthorPublication> AuthorPublications { get; set; } = new List<AuthorPublication>();
    }
}