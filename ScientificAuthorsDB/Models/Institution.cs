using System.ComponentModel.DataAnnotations;

namespace ScientificAuthorsDB.Models
{
    public class Institution
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Името на институцията е задължително")]
        [StringLength(200)]
        [Display(Name = "Институция")]
        public string Name { get; set; } = "";

        [Display(Name = "Държава")]
        public string? Country { get; set; }

        [Display(Name = "Град")]
        public string? City { get; set; }

        [Display(Name = "Тип")]
        public string? Type { get; set; }

        [Url]
        [Display(Name = "Уебсайт")]
        public string? Website { get; set; }

        public ICollection<AuthorInstitution> AuthorInstitutions { get; set; } = new List<AuthorInstitution>();
    }
}