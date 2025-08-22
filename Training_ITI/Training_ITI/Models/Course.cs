using System.ComponentModel.DataAnnotations;

namespace Training_ITI.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required, StringLength(50, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        public int? InstructorId { get; set; }
        public User? Instructor { get; set; }

        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
