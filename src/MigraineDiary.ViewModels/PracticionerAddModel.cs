using System.ComponentModel.DataAnnotations;

namespace MigraineDiary.ViewModels
{
    public class PracticionerAddModel
    {
        /// <summary>
        /// Practicioners rank.
        /// </summary>
        [Required]
        public string Rank { get; set; } = null!;

        /// <summary>
        /// Practicioner's first name.
        /// </summary>
        [Required]
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// Practicioner's last name.
        /// </summary>
        [Required]
        public string Lastname { get; set; } = null!;

        /// <summary>
        /// Practicioner's science degree.
        /// </summary>
        [MaxLength(30)]
        public string? ScienceDegree { get; set; }
    }
}
