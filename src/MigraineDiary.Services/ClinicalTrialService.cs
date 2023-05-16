using Microsoft.EntityFrameworkCore;
using MigraineDiary.Data;
using MigraineDiary.Data.DbModels;
using MigraineDiary.ViewModels;
using MigraineDiary.Services.Contracts;

namespace MigraineDiary.Services
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

        public async Task<PaginatedList<ClinicalTrialViewModel>> GetAllTrialsAsync(int pageIndex, int pageSize, string orderByDate)
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
                                                     Practicioners = t.Practicioners
                                                                      .Where(p => p.IsDeleted == false)
                                                                      .Select(p => new PracticionerViewModel
                                                                      {
                                                                          Rank = p.Rank,
                                                                          FirstName = p.FirstName,
                                                                          Lastname = p.Lastname,
                                                                          ScienceDegree = p.ScienceDegree,
                                                                      })
                                                                      .ToList(),
                                                     creatorId = t.CreatorId,
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
                                                     Practicioners = t.Practicioners
                                                                      .Where(p => p.IsDeleted == false)
                                                                      .Select(p => new PracticionerViewModel
                                                                      {
                                                                          Rank = p.Rank,
                                                                          FirstName = p.FirstName,
                                                                          Lastname = p.Lastname,
                                                                          ScienceDegree = p.ScienceDegree,
                                                                      })
                                                                      .ToList(),
                                                     creatorId = t.CreatorId,
                                                     CreatedOn = t.CreatedOn,
                                                 })
                                                 .AsNoTracking();
            }

            return await PaginatedList<ClinicalTrialViewModel>.CreateAsync(registeredTrials, pageIndex, pageSize);
        }

        public async Task<PaginatedList<ClinicalTrialViewModel>> GetAllTrialsAsync(int pageIndex, int pageSize, string orderByDate, string creatorId)
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
                                                     Practicioners = t.Practicioners
                                                                      .Where(p => p.IsDeleted == false)
                                                                      .Select(p => new PracticionerViewModel
                                                                      {
                                                                          Rank = p.Rank,
                                                                          FirstName = p.FirstName,
                                                                          Lastname = p.Lastname,
                                                                          ScienceDegree = p.ScienceDegree,
                                                                      })
                                                                      .ToList(),
                                                     creatorId = t.CreatorId,
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
                                                     Practicioners = t.Practicioners
                                                                      .Where(p => p.IsDeleted == false)
                                                                      .Select(p => new PracticionerViewModel
                                                                      {
                                                                          Rank = p.Rank,
                                                                          FirstName = p.FirstName,
                                                                          Lastname = p.Lastname,
                                                                          ScienceDegree = p.ScienceDegree,
                                                                      })
                                                                      .ToList(),
                                                     creatorId = t.CreatorId,
                                                     CreatedOn = t.CreatedOn,
                                                 })
                                                 .AsNoTracking();
            }

            return await PaginatedList<ClinicalTrialViewModel>.CreateAsync(registeredTrials, pageIndex, pageSize);
        }

        public async Task<ClinicalTrialEditModel> GetByIdAsync(string trialId, string creatorId)
        {
            ClinicalTrial? trial = await this.dbContext.ClinicalTrials
                                                       .Where(t => t.Id == trialId && t.CreatorId == creatorId)
                                                       .Include(t => t.Practicioners.Where(p => p.IsDeleted == false))
                                                       .AsNoTracking()
                                                       .FirstOrDefaultAsync();

            if (trial != null)
            {
                ClinicalTrialEditModel trialModel = new ClinicalTrialEditModel()
                {
                    Id = trialId,
                    Heading = trial.Heading,
                    City = trial.City,
                    Hospital = trial.Hospital,
                    AgreementDocumentName = trial.AgreementDocumentName,
                };

                foreach (var practicioner in trial.Practicioners)
                {
                    PracticionerEditModel currentPracticioner = new PracticionerEditModel
                    {
                        Id = practicioner.Id,
                        Rank = practicioner.Rank,
                        FirstName = practicioner.FirstName,
                        Lastname = practicioner.Lastname,
                        ScienceDegree = practicioner.ScienceDegree,
                    };

                    trialModel.Practicioners.Add(currentPracticioner);
                }

                return trialModel;
            }
            else
            {
                throw new ArgumentException("Невалидно Id на клинично проучване", nameof(trialId));
            }
        }

        public async Task EditTrialAsync(ClinicalTrialEditModel editmodel)
        {
            // Get trial which is going to be edited.
            ClinicalTrial? trial = await this.dbContext.ClinicalTrials
                                                       .Include(t => t.Practicioners)
                                                       .FirstOrDefaultAsync(t => t.Id == editmodel.Id);

            // Update properties values.
            if (trial != null)
            {
                trial.Heading = editmodel.Heading;
                trial.City = editmodel.City;
                trial.Hospital = editmodel.Hospital;
                trial.AgreementDocumentName = editmodel.AgreementDocumentName;

                // Remove deleted practicioners.
                foreach (PracticionerEditModel deletedPracticioner in editmodel.Practicioners.Where(p => p.IsDeleted == true))
                {
                    Practicioner? practicioner = trial.Practicioners.FirstOrDefault(p => p.Id == deletedPracticioner.Id);

                    // Soft delete.
                    if (practicioner != null)
                    {
                        practicioner.IsDeleted = true;
                        practicioner.DeletedOn = DateTime.UtcNow;
                    }

                    // Real delete.
                    //if (practicioner != null)
                    //{
                    //    this.dbContext.Practicioners.Remove(practicioner);
                    //}
                }

                // Update edited practicioners / Create newly added practicioners.
                foreach (var editedOrAddedPracticioner in editmodel.Practicioners.Where(p => p.IsDeleted == false))
                {
                    Practicioner? practicioner = trial.Practicioners.FirstOrDefault(p => p.Id == editedOrAddedPracticioner.Id);

                    if (practicioner != null)
                    {
                        practicioner.Rank = editedOrAddedPracticioner.Rank;
                        practicioner.FirstName = editedOrAddedPracticioner.FirstName;
                        practicioner.Lastname = editedOrAddedPracticioner.Lastname;
                        practicioner.ScienceDegree = editedOrAddedPracticioner.ScienceDegree;
                    }
                    else
                    {
                        practicioner = new Practicioner()
                        {
                            Rank = editedOrAddedPracticioner.Rank,
                            FirstName = editedOrAddedPracticioner.FirstName,
                            Lastname = editedOrAddedPracticioner.Lastname,
                            ScienceDegree = editedOrAddedPracticioner.ScienceDegree,
                        };

                        trial.Practicioners.Add(practicioner);
                    }
                }
            }

            await this.dbContext.SaveChangesAsync();
        }
    }
}
