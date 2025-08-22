using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Training_ITI.ViewModels
{
    public class CourseFormVM
    {
        public int? Id { get; set; }

        [Required, StringLength(50, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        public int? InstructorId { get; set; }

        public List<SelectListItem> Instructors { get; set; } = new();
    }
}
