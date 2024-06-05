namespace Producty.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime DeadLine { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public string UserId { get; set; }
        public AppUser? User { get; set; }
    }
}
