namespace ScientificAuthorsDB.Models
{
    public class PublicationField
    {
        public int PublicationId { get; set; }
        public Publication Publication { get; set; } = null!;

        public int ResearchFieldId { get; set; }
        public ResearchField ResearchField { get; set; } = null!;
    }
}