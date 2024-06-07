namespace Producty.Models
{
    public class Income
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public double EarnedMoney { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
