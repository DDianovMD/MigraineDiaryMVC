using Microsoft.EntityFrameworkCore;
using MigraineDiary.Web.Data;
using MigraineDiary.Web.Data.DbModels;
using MigraineDiary.Web.Models;
using MigraineDiary.Web.Services.Contracts;

namespace MigraineDiary.Web.Services
{
    public class HeadacheService : IHeadacheService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMedicationService medicationService;

        public HeadacheService(ApplicationDbContext dbContext, IMedicationService medicationService)
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

        public async Task<PaginatedList<RegisteredHeadacheViewModel>> GetRegisteredHeadachesAsync(string userId, int pageIndex, int pageSize, string orderByDate)
        {
            IQueryable<RegisteredHeadacheViewModel> headaches = null!;

            if (orderByDate == "NewestFirst")
            {
                headaches = this.dbContext.Headaches
                                          .Where(p => p.PatientId == userId)
                                          .Include(h => h.MedicationsTaken)
                                          .OrderByDescending(h => h.Onset)
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
                                              UsedMedications = h.MedicationsTaken.Select(m => new UsedMedicationViewModel
                                              {
                                                  Name = m.Name,
                                                  Units = m.Units,
                                                  DosageTaken = this.medicationService.CalculateWholeTakenDosage(m.SinglePillDosage, m.NumberOfTakenPills),
                                                  NumberOfTakenPills = m.NumberOfTakenPills,
                                                  SinglePillDosage = m.SinglePillDosage,
                                              })
                                          });
            }
            else
            {
                headaches = this.dbContext.Headaches
                                          .Where(p => p.PatientId == userId)
                                          .Include(h => h.MedicationsTaken)
                                          .OrderBy(h => h.Onset)
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
                                              UsedMedications = h.MedicationsTaken.Select(m => new UsedMedicationViewModel
                                              {
                                                  Name = m.Name,
                                                  Units = m.Units,
                                                  DosageTaken = this.medicationService.CalculateWholeTakenDosage(m.SinglePillDosage, m.NumberOfTakenPills),
                                                  NumberOfTakenPills = m.NumberOfTakenPills,
                                                  SinglePillDosage = m.SinglePillDosage,
                                              })
                                          });
            }

            return await PaginatedList<RegisteredHeadacheViewModel>.CreateAsync(headaches, pageIndex, pageSize);
        }
    }
}
