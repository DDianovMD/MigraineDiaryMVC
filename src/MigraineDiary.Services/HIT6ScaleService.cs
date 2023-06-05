using Microsoft.EntityFrameworkCore;
using MigraineDiary.Data;
using MigraineDiary.Data.DbModels;
using MigraineDiary.ViewModels;
using MigraineDiary.Services.Contracts;

namespace MigraineDiary.Services
{
    public class HIT6ScaleService : IHIT6ScaleService
    {
        private readonly ApplicationDbContext dbContext;
        private const string noAnswer = "NoAnswer";
        private const string never = "Never";
        private const string rarely = "Rarely";
        private const string sometimes = "Sometimes";
        private const string veryOften = "Very often";
        private const string always = "Always";

        public HIT6ScaleService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAsync(HIT6ScaleAddModel addModel, string userId)
        {
            string[] answers = new string[]
            {
                addModel.FirstQuestionAnswer,
                addModel.SecondQuestionAnswer,
                addModel.ThirdQuestionAnswer,
                addModel.FourthQuestionAnswer,
                addModel.FifthQuestionAnswer,
                addModel.SixthQuestionAnswer,
            };

            HIT6Scale HIT6Scale = new HIT6Scale()
            {
                FirstQuestionAnswer = addModel.FirstQuestionAnswer,
                SecondQuestionAnswer = addModel.SecondQuestionAnswer,
                ThirdQuestionAnswer = addModel.ThirdQuestionAnswer,
                FourthQuestionAnswer = addModel.FourthQuestionAnswer,
                FifthQuestionAnswer = addModel.FifthQuestionAnswer,
                SixthQuestionAnswer = addModel.SixthQuestionAnswer,
                PatientId = userId,
                TotalScore = this.CalculateTotalScore(answers),
            };

            await this.dbContext.HIT6Scales.AddAsync(HIT6Scale);
            await this.dbContext.SaveChangesAsync();
        }

        public int CalculateTotalScore(string[] answers)
        {
            int totalScore = 0;

            foreach (var answer in answers)
            {
                if (answer == never)
                {
                    totalScore += 6;
                }
                else if (answer == rarely)
                {
                    totalScore += 8;
                }
                else if (answer == sometimes)
                {
                    totalScore += 10;
                }
                else if (answer == veryOften)
                {
                    totalScore += 11;
                }
                else if (answer == always)
                {
                    totalScore += 13;
                }
            }

            return totalScore;
        }

        public async Task EditAsync(string scaleId, string userId, string[] editedAnswers)
        {
            // Get HIT-6 scale that is going to be edited.
            HIT6Scale? hit6scale = await this.dbContext.HIT6Scales
                                                       .FirstOrDefaultAsync(x => x.Id == scaleId);

            // Boolean flag evaluating if scale belongs to currently logged user.
            bool loggedUserHasScale = this.dbContext.Users
                                          .Where(u => u.Id == userId)
                                          .Include(s => s.HIT6Scales)
                                          .Any(u => u.HIT6Scales.Any(s => s.Id == scaleId));

            // Check if scale exists and belogs to user.
            if (hit6scale != null && loggedUserHasScale)
            {
                // Assign new values to edit the HIT-6 scale record in the database.
                hit6scale.FirstQuestionAnswer = editedAnswers[0];
                hit6scale.SecondQuestionAnswer = editedAnswers[1];
                hit6scale.ThirdQuestionAnswer = editedAnswers[2];
                hit6scale.FourthQuestionAnswer = editedAnswers[3];
                hit6scale.FifthQuestionAnswer = editedAnswers[4];
                hit6scale.SixthQuestionAnswer = editedAnswers[5];
                hit6scale.TotalScore = this.CalculateTotalScore(editedAnswers);
                hit6scale.CreatedOn = DateTime.UtcNow;

                // Save changes in database.
                await this.dbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("HIT-6 скалата не съществува или не принадлежи на потребителя.");
            }
        }

        public async Task<PaginatedList<HIT6ScaleViewModel>> GetAllAsync(string userId, int pageIndex, int pageSize, string orderByDate)
        {
            IQueryable<HIT6ScaleViewModel> registeredScales = null!;

            if (orderByDate == "NewestFirst")
            {
                registeredScales = this.dbContext.HIT6Scales
                                                 .Where(s => s.IsDeleted == false && s.PatientId == userId)
                                                 .OrderByDescending(s => s.CreatedOn)
                                                 .Select(s => new HIT6ScaleViewModel
                                                 {
                                                     Id = s.Id,
                                                     FirstQuestionAnswer = s.FirstQuestionAnswer,
                                                     SecondQuestionAnswer = s.SecondQuestionAnswer,
                                                     ThirdQuestionAnswer = s.ThirdQuestionAnswer,
                                                     FourthQuestionAnswer = s.FourthQuestionAnswer,
                                                     FifthQuestionAnswer = s.FifthQuestionAnswer,
                                                     SixthQuestionAnswer = s.SixthQuestionAnswer,
                                                     TotalScore = s.TotalScore,
                                                     CreatedOn = s.CreatedOn,
                                                     Doctors = s.SharedWith.Select(d => new DoctorViewModel
                                                     {
                                                         Id = d.Id,
                                                         FirstName = d.FirstName!,
                                                         LastName = d.LastName!,
                                                     }),
                                                 })
                                                 .AsNoTracking();
            }
            else
            {
                registeredScales = this.dbContext.HIT6Scales
                                                 .Where(s => s.IsDeleted == false && s.PatientId == userId)
                                                 .OrderBy(s => s.CreatedOn)
                                                 .Select(s => new HIT6ScaleViewModel
                                                 {
                                                     Id = s.Id,
                                                     FirstQuestionAnswer = s.FirstQuestionAnswer,
                                                     SecondQuestionAnswer = s.SecondQuestionAnswer,
                                                     ThirdQuestionAnswer = s.ThirdQuestionAnswer,
                                                     FourthQuestionAnswer = s.FourthQuestionAnswer,
                                                     FifthQuestionAnswer = s.FifthQuestionAnswer,
                                                     SixthQuestionAnswer = s.SixthQuestionAnswer,
                                                     TotalScore = s.TotalScore,
                                                     CreatedOn = s.CreatedOn,
                                                     Doctors = s.SharedWith.Select(d => new DoctorViewModel
                                                     {
                                                         Id = d.Id,
                                                         FirstName = d.FirstName!,
                                                         LastName = d.LastName!,
                                                     }),
                                                 })
                                                 .AsNoTracking();
            }

            return await PaginatedList<HIT6ScaleViewModel>.CreateAsync(registeredScales, pageIndex, pageSize);
        }

        public async Task<HIT6ScaleViewModel> GetByIdAsync(string scaleId, string userId)
        {
            HIT6Scale? hit6scale = await this.dbContext.HIT6Scales
                                                       .FirstOrDefaultAsync(s => s.Id == scaleId);

            bool loggedUserHasScale = this.dbContext.Users
                                          .Where(u => u.Id == userId)
                                          .Include(s => s.HIT6Scales)
                                          .Any(u => u.HIT6Scales.Any(s => s.Id == scaleId));

            // Check if hacker has tried to change HIT6Scale's Id through page's HTML.
            // If it has been changed, HIT6Scale eventually is going to be null (returned by FirstOrDefaultAsync method, line 63).

            // If it has been changed and somehow there is HIT6Scale in database with given GUID primary key (it's not null)
            // we check aswell if logged user is owner of the existing scale to reduce even more the chance that
            // hacker could get/edit/delete other user's registered HIT-6 scale. If these two validations fail - HTTP 404 is returned.
            if (hit6scale != null && loggedUserHasScale)
            {
                HIT6ScaleViewModel viewModel = new HIT6ScaleViewModel()
                {
                    Id = hit6scale!.Id,
                    FirstQuestionAnswer = hit6scale.FirstQuestionAnswer,
                    SecondQuestionAnswer = hit6scale.SecondQuestionAnswer,
                    ThirdQuestionAnswer = hit6scale.ThirdQuestionAnswer,
                    FourthQuestionAnswer = hit6scale.FourthQuestionAnswer,
                    FifthQuestionAnswer = hit6scale.FifthQuestionAnswer,
                    SixthQuestionAnswer = hit6scale.SixthQuestionAnswer,
                };

                return viewModel;
            }
            else
            {
                return null!;
            }
        }

        public async Task SoftDeleteAsync(string scaleId, string userId)
        {
            // Get HIT-6 scale that is going to be deleted.
            HIT6Scale? hit6scale = await this.dbContext.HIT6Scales
                                                       .FirstOrDefaultAsync(x => x.Id == scaleId);

            // Boolean flag evaluating if scale belongs to currently logged user.
            bool loggedUserHasScale = this.dbContext.Users
                                          .Where(u => u.Id == userId)
                                          .Include(s => s.HIT6Scales)
                                          .Any(u => u.HIT6Scales.Any(s => s.Id == scaleId));

            // Check if scale exists and belogs to user.
            if (hit6scale != null && loggedUserHasScale)
            {
                hit6scale.IsDeleted = true;
                hit6scale.DeletedOn = DateTime.UtcNow;

                // Save changes in database.
                await this.dbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("HIT-6 скалата не съществува или не принадлежи на потребителя.");
            }
        }

        public async Task Share(string scaleId, string doctorID)
        {
            // Get Doctor user.
            ApplicationUser? doctor = await this.dbContext.Users
                                                          .Include(u => u.SharedHIT6ScalesWithMe)
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
            if (doctor != null && userRoles.Any(r => r.Name == "Doctor"))
            {
                // Get scale which is going to be shared with the chosen doctor.
                var scale = this.dbContext.HIT6Scales.FirstOrDefault(s => s.Id == scaleId);

                // Check if scale exists.
                if (scale != null)
                {
                    // Check if scale is not shared already.
                    if (doctor.SharedHIT6ScalesWithMe.Contains(scale) == false)
                    {
                        // Add existing scale to doctor user's collection of shared headaches.
                        doctor.SharedHIT6ScalesWithMe.Add(scale);

                        // Save changes.
                        await this.dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        throw new ArgumentException("Scale is already shared.");
                    }
                }
                else
                {
                    throw new ArgumentException("Scale doesn't exist.");
                }
            }
            else
            {
                throw new ArgumentException("User doesn't exists or isn't in role \"Doctor\"");
            }
        }

        public bool ValidateAnswer(string answer)
        {
            if (answer == noAnswer ||
               (answer != never &&
                answer != rarely &&
                answer != sometimes &&
                answer != veryOften &&
                answer != always))
            {
                return true;
            }

            return false;
        }
    }
}