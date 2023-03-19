using Microsoft.EntityFrameworkCore;
using MigraineDiary.Web.Data;
using MigraineDiary.Web.Models;
using MigraineDiary.Web.Services.Contracts;

namespace MigraineDiary.Web.Services
{
    public class HeadacheService : IHeadacheService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly MedicationService medicationService;

        public HeadacheService(ApplicationDbContext dbContext, MedicationService medicationService)
        {
            this.dbContext = dbContext;
            this.medicationService = medicationService;
        }

        public Dictionary<string, int> CalculateDuration(DateTime onset, DateTime endtime)
        {
            TimeSpan headacheDuration = endtime - onset;
            Dictionary<string, int> daysHoursMinutes = new Dictionary<string, int>();

            daysHoursMinutes.Add("Days", headacheDuration.Days);
            daysHoursMinutes.Add("Hours", headacheDuration.Hours);
            daysHoursMinutes.Add("Minutes", headacheDuration.Minutes);
            
            return daysHoursMinutes;
        }

        public async Task<ICollection<RegisteredHeadacheViewModel>> GetRegisteredHeadachesAsync(string userId)
        {
            var headaches =  await this.dbContext.Headaches
                .Where(p => p.PatientId == userId)
                .Select(h => new RegisteredHeadacheViewModel
                {
                    Id = h.Id,
                    Onset = h.Onset,
                    EndTime = h.EndTime,
                    DurationDays = h.DurationDays,
                    DurationHours = h.DurationHours,
                    DurationMinutes = h.DurationMinutes,
                    Severity = h.Severity,
                    LocalizationSide = h.LocalizationSide,
                    PainCharacteristics = h.PainCharacteristics,
                    Photophoby = h.Photophoby,
                    Phonophoby = h.Phonophoby,
                    Nausea = h.Nausea,
                    Vomiting = h.Vomiting,
                    Aura = h.Aura,
                    AuraDescriptionNotes = h.AuraDescriptionNotes,
                    Triggers = h.Triggers,
                    UsedMedications = new HashSet<UsedMedicationViewModel>()
                }).ToArrayAsync();

            var userUsedMedications = await medicationService.GetUsedMedicationsAsync(userId);

            foreach (var headache in headaches)
            {
                foreach (var medication in userUsedMedications.Where(m => m.HeadacheId == headache.Id))
                {
                    headache.UsedMedications.Add(medication);
                }
            }

            return headaches;
        }
    }
}
