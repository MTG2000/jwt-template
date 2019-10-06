using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JwtTemplate.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        [MinLength(5)]
        public string Name { get; set; }

        [Required]
        [MaxLength(30)]
        [MinLength(8)]
        public string Password { get; set; }

        [Column("IsAdmin", TypeName = "bit")]   //This is neccessary otherwise it wont work (I dont know WHYYYYY !!!!)
        public bool IsAdmin { get; set; }
    }
}