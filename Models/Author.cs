using System.ComponentModel.DataAnnotations;

namespace PustokTemplate.Models
{
    public class Author
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
