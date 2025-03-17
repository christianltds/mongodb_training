using System.ComponentModel.DataAnnotations;

namespace mongodb.dtos
{
    public class CreateAccountDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public double InitialBalance { get; set; }
    }
}