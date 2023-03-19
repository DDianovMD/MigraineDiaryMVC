using Microsoft.EntityFrameworkCore;
using MigraineDiary.Web.Data;
using MigraineDiary.Web.Data.DbModels;
using MigraineDiary.Web.Models;
using MigraineDiary.Web.Services.Contracts;

namespace MigraineDiary.Web.Services
{
    public class MedicationService : IMedicationService
    {
        private readonly ApplicationDbContext dbContext;

        public MedicationService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ICollection<UsedMedicationViewModel>> GetUsedMedicationsAsync(string userId)
        {
            return await this.dbContext.Medications
                .Where(mt => mt.Headaches.All(h => h.PatientId == userId))
                .Join(dbContext.Headaches,
                        m => m.Id,
                        h => h.Id,
                        (m, h) => new {Medication = m, Headache = h})
                .Select(mh => new UsedMedicationViewModel
                {
                    Name = mh.Medication.Name,
                    SinglePillDosage = mh.Medication.SinglePillDosage,
                    NumberOfTakenPills = mh.Medication.NumberOfTakenPills,
                    Units = mh.Medication.Units,
                    DosageTaken = CalculateWholeTakenDosage(mh.Medication.SinglePillDosage, mh.Medication.NumberOfTakenPills),
                    HeadacheId = mh.Headache.Id
                })
                .ToArrayAsync();
        }

        private decimal CalculateWholeTakenDosage(decimal singlePillDosage, decimal numberOfTakenPills)
        {
            return Math.Round(singlePillDosage * numberOfTakenPills, 2);
        }
    }
}
