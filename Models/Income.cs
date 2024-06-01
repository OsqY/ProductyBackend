namespace Producty.Models
{
    public class Income
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Topic { get; set; }
        public double EarnedMoney { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
