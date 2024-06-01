namespace Producty.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        public string Auth0Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public ICollection<Todo> Todos { get; set; }
    }
}
