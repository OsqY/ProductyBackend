using System.ComponentModel.DataAnnotations;

namespace Producty.DTO
{
    public class ExpenseDTO
    {
        [Required]
        public string Name { get; set; }
        public string Category { get; set; }

        [Required]
        public double SpentMoney { get; set; }
    }
}
