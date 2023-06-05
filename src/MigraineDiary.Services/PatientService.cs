using Microsoft.EntityFrameworkCore;
using MigraineDiary.Data;
using MigraineDiary.Services.Contracts;
using MigraineDiary.ViewModels;

namespace MigraineDiary.Services
{
    public class PatientService : IPatientService
    {
        private readonly ApplicationDbContext dbContext;

        public PatientService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<PatientViewModel[]> GetAllPatientsAsync(string doctorId)
        {
            // Get all patients that shared atleast one headache with logged User in role "Doctor".
            PatientViewModel[] headachePatients = await this.dbContext.Users
                                                                      .Where(u => u.Id == doctorId)
                                                                      .Include(u => u.SharedWithMe)
                                                                      .SelectMany(x => x.SharedWithMe.Select(x => new PatientViewModel
                                                                      {
                                                                          PatientId = x.PatientId!,
                                                                          FirstName = x.Patient.FirstName!,
                                                                          MiddleName = x.Patient.MiddleName,
                                                                           LastName = x.Patient.LastName!,
                                                                      }))
                                                                      .ToArrayAsync();

            // Get all patients that shared atleast one HIT-6 scale with logged User in role "Doctor".
            PatientViewModel[] hit6Patients = await this.dbContext.Users
                                                                  .Where(u => u.Id == doctorId)
                                                                  .Include(u => u.SharedHIT6ScalesWithMe)
                                                                  .SelectMany(x => x.SharedHIT6ScalesWithMe.Select(x => new PatientViewModel
                                                                  {
                                                                      PatientId = x.PatientId!,
                                                                      FirstName = x.Patient.FirstName!,
                                                                      MiddleName = x.Patient.MiddleName,
                                                                      LastName = x.Patient.LastName!,
                                                                  }))
                                                                  .ToArrayAsync();

            // Get all patients that shared atleast one Zung's scale for anxiety with logged User in role "Doctor".
            PatientViewModel[] ZungPatients = await this.dbContext.Users
                                                                  .Where(u => u.Id == doctorId)
                                                                  .Include(u => u.SharedZungScalesForAnxietyWithMe)
                                                                  .SelectMany(x => x.SharedZungScalesForAnxietyWithMe.Select(x => new PatientViewModel
                                                                  {
                                                                      PatientId = x.PatientId!,
                                                                      FirstName = x.Patient.FirstName!,
                                                                      MiddleName = x.Patient.MiddleName,
                                                                      LastName = x.Patient.LastName!,
                                                                  }))
                                                                  .ToArrayAsync();

            // Get distinct patients.
            PatientViewModel[] allPatients = headachePatients.Union(hit6Patients)
                                                             .Union(ZungPatients)
                                                             .DistinctBy(x => x.PatientId)
                                                             .ToArray();

            return allPatients;
        }
    }
}
