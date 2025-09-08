namespace GameZone.Models
{
    public class Game
    {
        public int Id { get; set; }

        [MaxLength(250)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(2500)]
        public string Description { get; set; } = string.Empty;

        public string Cover { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = default!;

        // العلاقة Many-to-Many بين الألعاب والأجهزة
        public ICollection<GameDevice> Devices { get; set; } = new List<GameDevice>();
    }
}
