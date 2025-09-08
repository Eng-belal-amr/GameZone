namespace GameZone.Models
{
    public class Category
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }

        // مهم جدًا عشان نقدر نجيب عدد الألعاب
        public ICollection<Game> Games { get; set; } = [];
    }

}
