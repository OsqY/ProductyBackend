using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Producty.DTO
{
    public class IncomeDTO
    {
        [Required]
        public string Name { get; set; }

        [DefaultValue("")]
        public string Category { get; set; } = "";

        [Required]
        public double EarnedMoney { get; set; }
    }
}
