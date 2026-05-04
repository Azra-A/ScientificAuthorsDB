using System.ComponentModel.DataAnnotations;

namespace ScientificAuthorsDB.Models
{
    public class Publication
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Заглавието е задължително")]
        [StringLength(300)]
        [Display(Name = "Заглавие")]
        public string Title { get; set; } = "";

        [Display(Name = "Резюме")]
        [DataType(DataType.MultilineText)]
        public string? Abstract { get; set; }

        [Display(Name = "Година")]
        [Range(1900, 2100)]
        public int? Year { get; set; }

        [Display(Name = "DOI")]
        public string? Doi { get; set; }

        [Display(Name = "Журнал / Конференция")]
        public string? Journal { get; set; }

        [Display(Name = "Тип")]
        public string PublicationType { get; set; } = "Article";

        public ICollection<AuthorPublication> AuthorPublications { get; set; } = new List<AuthorPublication>();
        public ICollection<PublicationField> PublicationFields { get; set; } = new List<PublicationField>();
    }
}