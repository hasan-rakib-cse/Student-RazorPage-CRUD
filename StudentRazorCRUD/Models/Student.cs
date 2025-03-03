using System.ComponentModel.DataAnnotations;

namespace StudentRazorCRUD.Models
{
#nullable disable
    public class Student
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(20)]
        public string Email { get; set; }

        [Required, StringLength(15)]
        public string Phone { get; set; }

        public bool Subscribed { get; set; }
    }
}
