using System.ComponentModel.DataAnnotations;

namespace ToDoApplication.Models
{
    public class LoginUserViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
