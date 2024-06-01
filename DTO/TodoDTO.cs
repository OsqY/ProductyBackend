using System.ComponentModel.DataAnnotations;

namespace Producty.DTO
{
    public class TodoDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        public DateTime DeadLine { get; set; }
    }
}
