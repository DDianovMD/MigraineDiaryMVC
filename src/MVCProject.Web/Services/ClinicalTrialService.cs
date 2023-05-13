using Microsoft.EntityFrameworkCore;
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

        public async Task<PaginatedList<ClinicalTrialViewModel>> GetAllTrials(int pageIndex, int pageSize, string orderByDate)
        {
            IQueryable<ClinicalTrialViewModel> registeredTrials = null!;

            if (orderByDate == "NewestFirst")
            {
                registeredTrials = this.dbContext.ClinicalTrials
                                                 .Where(t => t.IsDeleted == false)
                                                 .OrderByDescending(t => t.CreatedOn)
                                                 .Select(t => new ClinicalTrialViewModel
                                                 {
                                                     Id = t.Id,
                                                     Heading = t.Heading,
                                                     City = t.City,
                                                     Hospital = t.Hospital,
                                                     AgreementDocumentName = t.AgreementDocumentName,
                                                     Practicioners = t.Practicioners.Select(p => new PracticionerViewModel
                                                     {
                                                         Rank = p.Rank,
                                                         FirstName = p.FirstName,
                                                         Lastname = p.Lastname,
                                                         ScienceDegree = p.ScienceDegree,
                                                     }).ToList(),
                                                     CreatedOn = t.CreatedOn,
                                                 })
                                                 .AsNoTracking();
            }
            else
            {
                registeredTrials = this.dbContext.ClinicalTrials
                                                 .Where(t => t.IsDeleted == false)
                                                 .OrderBy(t => t.CreatedOn)
                                                 .Select(t => new ClinicalTrialViewModel
                                                 {
                                                     Id = t.Id,
                                                     Heading = t.Heading,
                                                     City = t.City,
                                                     Hospital = t.Hospital,
                                                     Practicioners = t.Practicioners.Select(p => new PracticionerViewModel
                                                     {
                                                         Rank = p.Rank,
                                                         FirstName = p.FirstName,
                                                         Lastname = p.Lastname,
                                                         ScienceDegree = p.ScienceDegree,
                                                     }).ToList(),
                                                     CreatedOn = t.CreatedOn,
                                                 })
                                                 .AsNoTracking();
            }

            return await PaginatedList<ClinicalTrialViewModel>.CreateAsync(registeredTrials, pageIndex, pageSize);
        }

        public async Task<PaginatedList<ClinicalTrialViewModel>> GetAllTrials(int pageIndex, int pageSize, string orderByDate, string creatorId)
        {
            IQueryable<ClinicalTrialViewModel> registeredTrials = null!;

            if (orderByDate == "NewestFirst")
            {
                registeredTrials = this.dbContext.ClinicalTrials
                                                 .Where(t => t.IsDeleted == false && t.CreatorId == creatorId)
                                                 .OrderByDescending(t => t.CreatedOn)
                                                 .Select(t => new ClinicalTrialViewModel
                                                 {
                                                     Id = t.Id,
                                                     Heading = t.Heading,
                                                     City = t.City,
                                                     Hospital = t.Hospital,
                                                     AgreementDocumentName = t.AgreementDocumentName,
                                                     Practicioners = t.Practicioners.Select(p => new PracticionerViewModel
                                                     {
                                                         Rank = p.Rank,
                                                         FirstName = p.FirstName,
                                                         Lastname = p.Lastname,
                                                         ScienceDegree = p.ScienceDegree,
                                                     }).ToList(),
                                                     CreatedOn = t.CreatedOn,
                                                 })
                                                 .AsNoTracking();
            }
            else
            {
                registeredTrials = this.dbContext.ClinicalTrials
                                                 .Where(t => t.IsDeleted == false && t.CreatorId == creatorId)
                                                 .OrderBy(t => t.CreatedOn)
                                                 .Select(t => new ClinicalTrialViewModel
                                                 {
                                                     Id = t.Id,
                                                     Heading = t.Heading,
                                                     City = t.City,
                                                     Hospital = t.Hospital,
                                                     Practicioners = t.Practicioners.Select(p => new PracticionerViewModel
                                                     {
                                                         Rank = p.Rank,
                                                         FirstName = p.FirstName,
                                                         Lastname = p.Lastname,
                                                         ScienceDegree = p.ScienceDegree,
                                                     }).ToList(),
                                                     CreatedOn = t.CreatedOn,
                                                 })
                                                 .AsNoTracking();
            }

            return await PaginatedList<ClinicalTrialViewModel>.CreateAsync(registeredTrials, pageIndex, pageSize);
        }
    }
}
