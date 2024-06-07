namespace Producty.Models
{
    public class JournalEntry
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public string UserId { get; set; }
        public AppUser? User { get; set; }
    }
}
