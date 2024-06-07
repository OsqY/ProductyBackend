using System.ComponentModel.DataAnnotations;

namespace Producty.DTO
{
    public class JournalEntryDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
