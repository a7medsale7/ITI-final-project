using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Training_ITI.ViewModels
{
    public class GradeFormVM
    {
        public int? Id { get; set; }

        [Required]
        public int SessionId { get; set; }
        public List<SelectListItem> Sessions { get; set; } = new();

        [Required]
        public int TraineeId { get; set; }
        public List<SelectListItem> Trainees { get; set; } = new();

        [Required, Range(0, 100)]
        public decimal Value { get; set; }
    }
}
