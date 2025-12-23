using System.ComponentModel.DataAnnotations;

namespace AuthRole.Entities
{
    public class Meet
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public string Role { get; set; } // Admin / User
    }
}
