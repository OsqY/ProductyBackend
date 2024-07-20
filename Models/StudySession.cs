namespace Producty.Models
{
    public class StudySession
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Duration => (int)(EndTime - StartTime).TotalMinutes;

        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public string UserId { get; set; }
        public AppUser? User { get; set; }
    }
}
