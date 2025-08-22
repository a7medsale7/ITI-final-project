using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Training_ITI.Models
{
    public class Session
    {
        public int Id { get; set; }

        [Required]
        public int CourseId { get; set; }
        public Course? Course { get; set; }

        [Required, DataType(DataType.Date)]
        [StartNotInPast]
        public DateTime StartDate { get; set; }

        [Required, DataType(DataType.Date)]
        [EndAfter(nameof(StartDate))]
        public DateTime EndDate { get; set; }

        public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}
