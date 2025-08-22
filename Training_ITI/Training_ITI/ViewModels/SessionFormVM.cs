using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Training_ITI.Models;

namespace Training_ITI.ViewModels
{
    public class SessionFormVM
    {
        public int? Id { get; set; }

        [Required]
        public int CourseId { get; set; }
        public List<SelectListItem> Courses { get; set; } = new();

        [Required, DataType(DataType.Date)]
        [StartNotInPast]
        public DateTime StartDate { get; set; }

        [Required, DataType(DataType.Date)]
        [EndAfter(nameof(StartDate))]
        public DateTime EndDate { get; set; }
    }
}
