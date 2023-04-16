using Microsoft.AspNetCore.Identity;
using MigraineDiary.Web.Data.Common.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigraineDiary.Web.Data.DbModels
{
    public class ApplicationUser : IdentityUser, ISoftDeletable
    {
        //TODO: add phonenumber and email in register view
        public ApplicationUser()
        {
            this.Headaches = new List<Headache>();
            this.SharedWithMe = new List<Headache>();
            this.HIT6Scales = new List<HIT6Scale>();
            this.SharedHIT6ScalesWithMe = new List<HIT6Scale>();
        }

        /// <summary>
        /// Patient's first name.
        /// </summary>
        //[Required]
        [MaxLength(30)]
        public string? FirstName { get; set; }

        //[Required]
        [MaxLength(30)]
        public string? MiddleName { get; set; }

        /// <summary>
        /// Patient's last name.
        /// </summary>
        //[Required]
        [MaxLength(30)]
        public string? LastName { get; set; }

        /// <summary>
        /// Boolean flag for soft delete.
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Headaches registered by user in role "Patient".
        /// </summary>
        [InverseProperty("Patient")]
        public ICollection<Headache> Headaches { get; set; }

        /// <summary>
        /// Patient's registered headaches shared with user in role "Doctor".
        /// </summary>
        [InverseProperty("SharedWith")]
        public ICollection<Headache> SharedWithMe { get; set;}

        /// <summary>
        /// HIT6 scales registered by user in role "Patient".
        /// </summary>
        [InverseProperty("Patient")]
        public ICollection<HIT6Scale> HIT6Scales { get; set;}

        /// <summary>
        /// Patient's registered HIT6 scales shared with user in role "Doctor".
        /// </summary>
        [InverseProperty("SharedWith")]
        public ICollection<HIT6Scale> SharedHIT6ScalesWithMe { get; set; }

        /// <summary>
        /// Zung's scales for anxiety registered by user in role "Patient".
        /// </summary>
        [InverseProperty("Patient")]
        public ICollection<ZungScaleForAnxiety> ZungScalesForAnxiety { get; set; }

        /// <summary>
        /// Patient's registered Zung's scales for anxiety shared with user in role "Doctor".
        /// </summary>
        [InverseProperty("SharedWith")]
        public ICollection<ZungScaleForAnxiety> SharedZungScalesForAnxietyWithMe { get; set; }

        /// <summary>
        /// Articles published by user.
        /// </summary>
        public ICollection<Article> Articles { get; set; }
    }
}
