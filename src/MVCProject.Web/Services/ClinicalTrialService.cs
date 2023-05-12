using MigraineDiary.Web.Data;
using MigraineDiary.Web.Data.DbModels;
using MigraineDiary.Web.Models;
using MigraineDiary.Web.Services.Contracts;

namespace MigraineDiary.Web.Services
{
    public class ClinicalTrialService : IClinicalTrialService
    {
        private readonly ApplicationDbContext dbContext;

        public ClinicalTrialService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAsync(ClinicalTrialAddModel addModel, string uploadedFileName, string uploaderID)
        {
            ClinicalTrial trial = new ClinicalTrial()
            {
                Heading = addModel.Heading,
                City = addModel.City,
                Hospital = addModel.Hospital,
                CreatorId = uploaderID,
                AgreementDocumentName = uploadedFileName,
            };

            foreach (var practicioner in addModel.Practicioners)
            {
                Practicioner currentPracticioner = new Practicioner()
                {
                    Rank = practicioner.Rank,
                    FirstName = practicioner.FirstName,
                    Lastname = practicioner.Lastname,
                    ScienceDegree = practicioner.ScienceDegree,
                };

                trial.Practicioners.Add(currentPracticioner);
            }

            await this.dbContext.ClinicalTrials.AddAsync(trial);
            await this.dbContext.SaveChangesAsync();
        }

        public string GenerateUniqueFileName(string fileExtension)
        {
            string guid = Guid.NewGuid().ToString();
            string uploadDate = DateTime.UtcNow.ToShortDateString();

            return $"{guid}_{uploadDate}.{fileExtension}";
        }

        public ClinicalTrialViewModel GetAllTrials()
        {
            throw new NotImplementedException();
        }
    }
}
