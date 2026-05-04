using System.ComponentModel.DataAnnotations;

namespace ScientificAuthorsDB.Models
{
    public class ResearchField
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Научна област")]
        public string Name { get; set; } = "";

        [Display(Name = "Описание")]
        public string? Description { get; set; }

        public ICollection<PublicationField> PublicationFields { get; set; } = new List<PublicationField>();
    }
}