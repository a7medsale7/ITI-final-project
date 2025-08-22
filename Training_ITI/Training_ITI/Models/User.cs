using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Training_ITI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, StringLength(50, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; }

        public ICollection<Course> CoursesAsInstructor { get; set; } = new List<Course>();
        public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}
