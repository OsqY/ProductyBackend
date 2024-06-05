using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Producty.DTO
{
    public class TodoDTO
    {
        [Required]
        public string Name { get; set; }

        [DefaultValue("")]
        public string Description { get; set; } = "";

        [Required]
        public string StartTime { get; set; }

        public string DeadLine { get; set; }

        [DefaultValue(false)]
        public bool IsCompleted { get; set; } = false;
    }
}
