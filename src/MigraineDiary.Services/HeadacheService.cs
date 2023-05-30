using Microsoft.EntityFrameworkCore;
using MigraineDiary.Data;
using MigraineDiary.ViewModels;
using MigraineDiary.Services.Contracts;

namespace MigraineDiary.Services
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
                                          .Include(h => h.SharedWith)
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
                                              }),
                                              Doctors = h.SharedWith.Select(d => new DoctorViewModel
                                              {
                                                  Id = d.Id,
                                                  FirstName = d.FirstName!,
                                                  LastName = d.LastName!,
                                              })
                                          });
            }
            else
            {
                headaches = this.dbContext.Headaches
                                          .Where(p => p.PatientId == userId)
                                          .Include(h => h.MedicationsTaken)
                                          .Include(h => h.SharedWith)
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
                                              }),
                                              Doctors = h.SharedWith.Select(d => new DoctorViewModel
                                              {
                                                  Id = d.Id,
                                                  FirstName = d.FirstName!,
                                                  LastName = d.LastName!,
                                              })
                                          });
            }

            return await PaginatedList<RegisteredHeadacheViewModel>.CreateAsync(headaches, pageIndex, pageSize);
        }

        public async Task<DoctorViewModel[]> GetDoctorUsersByNameAsync(string name)
        {
            DoctorViewModel[] doctorUsers = await dbContext.Roles
                                                           .Where(r => r.Name == "Doctor")
                                                           .Join(this.dbContext.UserRoles,
                                                                 r => r.Id,
                                                                 ur => ur.RoleId,
                                                                 (r, ur) => new
                                                                 {
                                                                     userId = ur.UserId,
                                                                 })
                                                           .Join(this.dbContext.Users.Where(u => EF.Functions.Like(u.FirstName!, $"%{name}%") ||
                                                                                                 EF.Functions.Like(u.LastName!, $"%{name}%")),
                                                                 ur => ur.userId,
                                                                 u => u.Id,
                                                                 (ur, u) => new DoctorViewModel
                                                                 {
                                                                     FirstName = u.FirstName!,
                                                                     LastName = u.LastName!,
                                                                 })
                                                           .AsNoTracking()
                                                           .ToArrayAsync();

            return doctorUsers;
        }
    }
}
