using System.ComponentModel.DataAnnotations;

using TLP_API.Models; // This line is crucial to reference the MyItem class

namespace TLP_API.Models
{
    public class MyItem
    {
        [Required] // 'Name' must be provided
        [MaxLength(1024)]
        public required string Name { get; set; }

        [EmailAddress] // 'Email' can be null or valid email format
        public string? Email { get; set; }  // Nullable Email

        [Required] // 'id' must always be provided
        public string id { get; set; } = Guid.NewGuid().ToString(); // Automatically generate a unique ID
    }
}