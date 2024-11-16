using System.ComponentModel.DataAnnotations;

using TLP_API.Models; // This line is crucial to reference the MyItem class

// Models/MyItem.cs
namespace TLP_API.Models
{
    public class MyItem
    {
        [Required]
        [MaxLength(1024)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        public string id { get; set; } = Guid.NewGuid().ToString(); // Automatically generate a unique ID
    }
}
