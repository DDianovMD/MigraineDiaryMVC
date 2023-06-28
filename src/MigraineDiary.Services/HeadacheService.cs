using Microsoft.EntityFrameworkCore;
using MigraineDiary.Data;
using MigraineDiary.Data.DbModels;
using MigraineDiary.Services.Contracts;
using MigraineDiary.ViewModels;

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

        public async Task AddAsync(HeadacheAddFormModel addModel, Dictionary<string, int> headacheDuration, string currentUserId)
        {
            Headache headache = new Headache
            {
                PatientId = currentUserId,
                Aura = addModel.Aura == "yes" ? true : false,
                AuraDescriptionNotes = addModel.AuraDescriptionNotes,
                Onset = addModel.Onset,
                EndTime = addModel.EndTime,
                DurationDays = headacheDuration["Days"],
                DurationHours = headacheDuration["Hours"],
                DurationMinutes = headacheDuration["Minutes"],
                Severity = addModel.Severity,
                LocalizationSide = addModel.LocalizationSide,
                PainCharacteristics = addModel.PainCharacteristics,
                Photophoby = addModel.Photophoby == "yes" ? true : false,
                Phonophoby = addModel.Phonophoby == "yes" ? true : false,
                Nausea = addModel.Nausea == "yes" ? true : false,
                Vomiting = addModel.Vomiting == "yes" ? true : false,
                Triggers = addModel.Triggers,
            };

            if (addModel.MedicationsTaken.Count > 0)
            {
                foreach (MedicationAddFormModel medication in addModel.MedicationsTaken)
                {
                    Medication currentMedication = new Medication
                    {
                        Name = medication.Name,
                        SinglePillDosage = medication.SinglePillDosage,
                        Units = medication.Units,
                        NumberOfTakenPills = medication.NumberOfTakenPills,
                    };

                    headache.MedicationsTaken.Add(currentMedication);
                }
            }

            await this.dbContext.Headaches.AddAsync(headache);
            await this.dbContext.SaveChangesAsync();
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

        public async Task<PaginatedList<SharedHeadacheViewModel>> GetSharedHeadachesAsync(string doctorId, string patientId, int pageIndex, int pageSize, string orderByDate)
        {
            ICollection<SharedHeadacheViewModel> headaches = null!;

            if (orderByDate == "NewestFirst")
            {
                headaches = await this.dbContext.Users
                                                .Where(user => user.Id == doctorId)
                                                .Include(user => user.SharedWithMe.Where(headache => headache.PatientId == patientId))
                                                .ThenInclude(headache => headache.MedicationsTaken)
                                                .SelectMany(head => head.SharedWithMe.Select(headache => new SharedHeadacheViewModel
                                                {
                                                    HeadacheId = headache.Id,
                                                    PatientFirstName = headache.Patient.FirstName!,
                                                    PatientMiddleName = headache.Patient.MiddleName,
                                                    PatientLastName = headache.Patient.LastName!,
                                                    Onset = headache.Onset,
                                                    EndTime = headache.EndTime,
                                                    DurationDays = headache.DurationDays,
                                                    DurationHours = headache.DurationHours,
                                                    DurationMinutes = headache.DurationMinutes,
                                                    Severity = headache.Severity,
                                                    LocalizationSide = headache.LocalizationSide,
                                                    PainCharacteristics = headache.PainCharacteristics,
                                                    Photophoby = headache.Photophoby,
                                                    Phonophoby = headache.Phonophoby,
                                                    Nausea = headache.Nausea,
                                                    Vomiting = headache.Vomiting,
                                                    Aura = headache.Aura,
                                                    AuraDescriptionNotes = headache.AuraDescriptionNotes,
                                                    Triggers = headache.Triggers,
                                                    UsedMedications = headache.MedicationsTaken.Select(m => new UsedMedicationViewModel
                                                    {
                                                        Name = m.Name,
                                                        Units = m.Units,
                                                        DosageTaken = this.medicationService.CalculateWholeTakenDosage(m.SinglePillDosage, m.NumberOfTakenPills),
                                                        NumberOfTakenPills = m.NumberOfTakenPills,
                                                        SinglePillDosage = m.SinglePillDosage,
                                                    })
                                                }))
                                                .OrderByDescending(headache => headache.Onset)
                                                .ToArrayAsync();
                    
            }
            else
            {
                headaches = await this.dbContext.Users
                                                .Where(user => user.Id == doctorId)
                                                .Include(user => user.SharedWithMe.Where(headache => headache.PatientId == patientId))
                                                .ThenInclude(headache => headache.MedicationsTaken)
                                                .SelectMany(head => head.SharedWithMe.Select(headache => new SharedHeadacheViewModel
                                                {
                                                    HeadacheId = headache.Id,
                                                    PatientFirstName = headache.Patient.FirstName!,
                                                    PatientMiddleName = headache.Patient.MiddleName,
                                                    PatientLastName = headache.Patient.LastName!,
                                                    Onset = headache.Onset,
                                                    EndTime = headache.EndTime,
                                                    DurationDays = headache.DurationDays,
                                                    DurationHours = headache.DurationHours,
                                                    DurationMinutes = headache.DurationMinutes,
                                                    Severity = headache.Severity,
                                                    LocalizationSide = headache.LocalizationSide,
                                                    PainCharacteristics = headache.PainCharacteristics,
                                                    Photophoby = headache.Photophoby,
                                                    Phonophoby = headache.Phonophoby,
                                                    Nausea = headache.Nausea,
                                                    Vomiting = headache.Vomiting,
                                                    Aura = headache.Aura,
                                                    AuraDescriptionNotes = headache.AuraDescriptionNotes,
                                                    Triggers = headache.Triggers,
                                                    UsedMedications = headache.MedicationsTaken.Select(m => new UsedMedicationViewModel
                                                    {
                                                        Name = m.Name,
                                                        Units = m.Units,
                                                        DosageTaken = this.medicationService.CalculateWholeTakenDosage(m.SinglePillDosage, m.NumberOfTakenPills),
                                                        NumberOfTakenPills = m.NumberOfTakenPills,
                                                        SinglePillDosage = m.SinglePillDosage,
                                                    }),
                                                }))
                                                .OrderBy(headache => headache.Onset)
                                                .ToArrayAsync();
            }

            return PaginatedList<SharedHeadacheViewModel>.CreateAsync(headaches, pageIndex, pageSize);
        }

        public async Task<DoctorViewModel[]> GetDoctorUsersByNameAsync(string name)
        {
            string firstName = string.Empty;
            string lastName = string.Empty;

            if (name.Contains(' '))
            {
                string[] names = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                firstName = names[0];
                lastName = names[1];
            }
            else
            {
                firstName = name;
            }

            DoctorViewModel[] doctorUsers;

            if (lastName == string.Empty)
            {
                doctorUsers = await dbContext.Roles
                                             .Where(r => r.Name == "Doctor")
                                             .Join(this.dbContext.UserRoles,
                                                   r => r.Id,
                                                   ur => ur.RoleId,
                                                   (r, ur) => new
                                                   {
                                                       userId = ur.UserId,
                                                   })
                                             .Join(this.dbContext.Users.Where(u => EF.Functions.Like(u.FirstName!, $"%{firstName}%")),
                                                   ur => ur.userId,
                                                   u => u.Id,
                                                   (ur, u) => new DoctorViewModel
                                                   {
                                                       Id = u.Id,
                                                       FirstName = u.FirstName!,
                                                       LastName = u.LastName!,
                                                   })
                                             .AsNoTracking()
                                             .ToArrayAsync();
            }
            else
            {
                doctorUsers = await dbContext.Roles
                                             .Where(r => r.Name == "Doctor")
                                             .Join(this.dbContext.UserRoles,
                                                   r => r.Id,
                                                   ur => ur.RoleId,
                                                   (r, ur) => new
                                                   {
                                                       userId = ur.UserId,
                                                   })
                                             .Join(this.dbContext.Users.Where(u => EF.Functions.Like(u.FirstName!, $"%{firstName}%") &&
                                                                                   EF.Functions.Like(u.LastName!, $"%{lastName}%")),
                                                   ur => ur.userId,
                                                   u => u.Id,
                                                   (ur, u) => new DoctorViewModel
                                                   {
                                                       Id = u.Id,
                                                       FirstName = u.FirstName!,
                                                       LastName = u.LastName!,
                                                   })
                                             .AsNoTracking()
                                             .ToArrayAsync();
            }
             

            return doctorUsers;
        }

        public async Task Share(string headacheId, string doctorID)
        {
            // Get Doctor user.
            ApplicationUser? doctor = await this.dbContext.Users
                                                          .Include(u => u.SharedWithMe)
                                                          .FirstOrDefaultAsync(u => u.Id == doctorID);

            // Get all role's names which are assigned to the user.
            var userRoles = await this.dbContext.UserRoles.Where(ur => ur.UserId == doctorID)
                                                          .Join(this.dbContext.Roles,
                                                                ur => ur.RoleId,
                                                                r => r.Id,
                                                                (ur, r) => new
                                                                {
                                                                    UserId = ur.UserId,
                                                                    Name = r.Name,
                                                                })
                                                          .AsNoTracking()
                                                          .ToArrayAsync();

            // Check if user exists and is in role "Doctor".
            if (doctor != null && userRoles.Any(x => x.Name == "Doctor"))
            {
                // Get headache which is going to be shared with the chosen doctor.
                var headache = this.dbContext.Headaches.FirstOrDefault(h => h.Id == headacheId);

                // Check if headache exists.
                if (headache != null)
                {
                    if (doctor.SharedWithMe.Contains(headache) == false)
                    {
                        // Add existing headache to doctor user's collection of shared headaches.
                        doctor.SharedWithMe.Add(headache);

                        // Save changes.
                        await this.dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        throw new ArgumentException("Headache is already shared.");
                    }

                }
                else
                {
                    throw new ArgumentException("Headache doesn't exist.");
                }
            }
            else
            {
                throw new ArgumentException("User doesn't exists or isn't in role \"Doctor\"");
            }
        }
    }
}
