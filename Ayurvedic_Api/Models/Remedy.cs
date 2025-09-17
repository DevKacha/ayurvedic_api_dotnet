namespace Ayurvedic_Api.Models
{
    public class Remedy
    {
        public string? RemedyId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Ingredients { get; set; }
        public List<string> PreparationSteps { get; set; }
        public string Usage { get; set; }
        public string Ailment { get; set; }
        public string? Precautions { get; set; }
        public string? Category { get; set; }
        public string? Dosage { get; set; }
        public string? AgeGroup { get; set; }
        public string? ImageUrl { get; set; }
        public string? SourceReference { get; set; }
        public bool IsVerified { get; set; }
        public string Language { get; set; }
        public List<string>? Tags { get; set; }
        public string? Tip { get; set; }
        public string? Type { get; set; }
    }
}
