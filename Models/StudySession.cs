namespace Producty.Models
{
    public class StudySession
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }

        // public int CurrentStreak { get; set; }
        // public int LastStreak { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
