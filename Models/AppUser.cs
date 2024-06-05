using System.ComponentModel.DataAnnotations;

namespace Producty.Models
{
    public class AppUser
    {
        [Key]
        public string Id { get; set; }
        public string Auth0Id { get; set; }

        public ICollection<Todo> Todos { get; set; }
    }
}
