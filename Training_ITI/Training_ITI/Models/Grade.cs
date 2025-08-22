using System.ComponentModel.DataAnnotations;

namespace Training_ITI.Models
{
    public class Grade
    {
        public int Id { get; set; }

        [Required]
        public int SessionId { get; set; }
        public Session? Session { get; set; }

        [Required]
        public int TraineeId { get; set; }
        public User? Trainee { get; set; }

        [Required, Range(0, 100)]
        public decimal Value { get; set; }
    }
}
