using System.ComponentModel.DataAnnotations;

namespace Producty.Models
{
    public class AppUser
    {
        [Key]
        public string Id { get; set; }
        public string Auth0Id { get; set; }

        public ICollection<Todo> Todos { get; set; }
        public ICollection<JournalEntry> JournalEntries { get; set; }
        public ICollection<Expense> Expenses { get; set; }
        public ICollection<Income> Incomes { get; set; }
    }
}
