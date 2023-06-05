using Microsoft.EntityFrameworkCore;
using MigraineDiary.Data;
using MigraineDiary.Data.DbModels;
using MigraineDiary.ViewModels;
using MigraineDiary.Services.Contracts;

namespace MigraineDiary.Services
{
    public class ZungScaleForAnxietyService : IZungScaleForAnxietyService
    {
        private readonly ApplicationDbContext dbContext;
        private const string noAnswer = "NoAnswer";
        private const string neverOrRarely = "Never or rarely";
        private const string sometimes = "Sometimes";
        private const string often = "Often";
        private const string veryOftenOrAlways = "Very often or always";

        public ZungScaleForAnxietyService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAsync(ZungScaleAddModel addModel, string userId)
        {
            string[] answers = new string[]
            {
                addModel.FirstQuestionAnswer,
                addModel.SecondQuestionAnswer,
                addModel.ThirdQuestionAnswer,
                addModel.FourthQuestionAnswer,
                addModel.FifthQuestionAnswer,
                addModel.SixthQuestionAnswer,
                addModel.SeventhQuestionAnswer,
                addModel.EighthQuestionAnswer,
                addModel.NinthQuestionAnswer,
                addModel.TenthQuestionAnswer,
                addModel.EleventhQuestionAnswer,
                addModel.TwelfthQuestionAnswer,
                addModel.ThirteenthQuestionAnswer,
                addModel.FourteenthQuestionAnswer,
                addModel.FifteenthQuestionAnswer,
                addModel.SixteenthQuestionAnswer,
                addModel.SeventeenthQuestionAnswer,
                addModel.EighteenthQuestionAnswer,
                addModel.NineteenthQuestionAnswer,
                addModel.TwentiethQuestionAnswer,
            };

            ZungScaleForAnxiety zungScale = new ZungScaleForAnxiety()
            {
                FirstQuestionAnswer = addModel.FirstQuestionAnswer,
                SecondQuestionAnswer = addModel.SecondQuestionAnswer,
                ThirdQuestionAnswer = addModel.ThirdQuestionAnswer,
                FourthQuestionAnswer = addModel.FourthQuestionAnswer,
                FifthQuestionAnswer = addModel.FifthQuestionAnswer,
                SixthQuestionAnswer = addModel.SixthQuestionAnswer,
                SeventhQuestionAnswer = addModel.SeventhQuestionAnswer,
                EighthQuestionAnswer = addModel.EighthQuestionAnswer,
                NinthQuestionAnswer = addModel.NinthQuestionAnswer,
                TenthQuestionAnswer = addModel.TenthQuestionAnswer,
                EleventhQuestionAnswer = addModel.EleventhQuestionAnswer,
                TwelfthQuestionAnswer = addModel.TwelfthQuestionAnswer,
                ThirteenthQuestionAnswer = addModel.ThirteenthQuestionAnswer,
                FourteenthQuestionAnswer = addModel.FourteenthQuestionAnswer,
                FifteenthQuestionAnswer = addModel.FifteenthQuestionAnswer,
                SixteenthQuestionAnswer = addModel.SixteenthQuestionAnswer,
                SeventeenthQuestionAnswer = addModel.SeventeenthQuestionAnswer,
                EighteenthQuestionAnswer = addModel.EighteenthQuestionAnswer,
                NineteenthQuestionAnswer = addModel.NineteenthQuestionAnswer,
                TwentiethQuestionAnswer = addModel.TwentiethQuestionAnswer,
                PatientId = userId,
                TotalScore = this.CalculateTotalScore(answers),
            };

            await this.dbContext.ZungScalesForAnxiety.AddAsync(zungScale);
            await this.dbContext.SaveChangesAsync();
        }

        public int CalculateTotalScore(string[] answers)
        {
            int totalScore = 0;

            foreach (var answer in answers)
            {
                if (answer == neverOrRarely)
                {
                    totalScore += 1;
                }
                else if (answer == sometimes)
                {
                    totalScore += 2;
                }
                else if (answer == often)
                {
                    totalScore += 3;
                }
                else if (answer == veryOftenOrAlways)
                {
                    totalScore += 4;
                }
            }

            return totalScore;
        }

        public async Task EditAsync(string scaleId, string userId, string[] editedAnswers)
        {
            // Get Zung's scale that is going to be edited.
            ZungScaleForAnxiety? zungScale = await this.dbContext.ZungScalesForAnxiety
                                                                 .FirstOrDefaultAsync(x => x.Id == scaleId);

            // Boolean flag evaluating if scale belongs to currently logged user.
            bool loggedUserHasScale = this.dbContext.Users
                                                    .Where(x => x.Id == userId)
                                                    .Include(x => x.ZungScalesForAnxiety)
                                                    .Any(x => x.ZungScalesForAnxiety.Any(x => x.Id == scaleId));

            // Check if scale exists and belogs to user.
            if (zungScale != null && loggedUserHasScale)
            {
                // Assign new values to edit the Zung scale record in the database.
                zungScale.FirstQuestionAnswer = editedAnswers[0];
                zungScale.SecondQuestionAnswer = editedAnswers[1];
                zungScale.ThirdQuestionAnswer = editedAnswers[2];
                zungScale.FourthQuestionAnswer = editedAnswers[3];
                zungScale.FifthQuestionAnswer = editedAnswers[4];
                zungScale.SixthQuestionAnswer = editedAnswers[5];
                zungScale.SeventhQuestionAnswer = editedAnswers[6];
                zungScale.EighthQuestionAnswer = editedAnswers[7];
                zungScale.NinthQuestionAnswer = editedAnswers[8];
                zungScale.TenthQuestionAnswer = editedAnswers[9];
                zungScale.EleventhQuestionAnswer = editedAnswers[10];
                zungScale.TwelfthQuestionAnswer = editedAnswers[11];
                zungScale.ThirteenthQuestionAnswer = editedAnswers[12];
                zungScale.FourteenthQuestionAnswer = editedAnswers[13];
                zungScale.FifteenthQuestionAnswer = editedAnswers[14];
                zungScale.SixteenthQuestionAnswer = editedAnswers[15];
                zungScale.SeventeenthQuestionAnswer = editedAnswers[16];
                zungScale.EighteenthQuestionAnswer = editedAnswers[17];
                zungScale.NineteenthQuestionAnswer = editedAnswers[18];
                zungScale.TwentiethQuestionAnswer = editedAnswers[19];
                zungScale.TotalScore = this.CalculateTotalScore(editedAnswers);
                zungScale.CreatedOn = DateTime.UtcNow;

                // Save changes in database.
                await this.dbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("Скалата на Zung не съществува или не принадлежи на потребителя.");
            }
        }

        public async Task<PaginatedList<ZungScaleViewModel>> GetAllAsync(string userId, int pageIndex, int pageSize, string orderByDate)
        {
            IQueryable<ZungScaleViewModel> registeredScales = null!;

            if (orderByDate == "NewestFirst")
            {
                registeredScales = this.dbContext.ZungScalesForAnxiety
                                                 .Where(s => s.IsDeleted == false && s.PatientId == userId)
                                                 .OrderByDescending(s => s.CreatedOn)
                                                 .Select(s => new ZungScaleViewModel
                                                 {
                                                     Id = s.Id,
                                                     FirstQuestionAnswer = s.FirstQuestionAnswer,
                                                     SecondQuestionAnswer = s.SecondQuestionAnswer,
                                                     ThirdQuestionAnswer = s.ThirdQuestionAnswer,
                                                     FourthQuestionAnswer = s.FourthQuestionAnswer,
                                                     FifthQuestionAnswer = s.FifthQuestionAnswer,
                                                     SixthQuestionAnswer = s.SixthQuestionAnswer,
                                                     SeventhQuestionAnswer = s.SeventhQuestionAnswer,
                                                     EighthQuestionAnswer = s.EighthQuestionAnswer,
                                                     NinthQuestionAnswer = s.NinthQuestionAnswer,
                                                     TenthQuestionAnswer = s.TenthQuestionAnswer,
                                                     EleventhQuestionAnswer = s.EleventhQuestionAnswer,
                                                     TwelfthQuestionAnswer = s.TwelfthQuestionAnswer,
                                                     ThirteenthQuestionAnswer = s.ThirteenthQuestionAnswer,
                                                     FourteenthQuestionAnswer = s.FourteenthQuestionAnswer,
                                                     FifteenthQuestionAnswer = s.FifteenthQuestionAnswer,
                                                     SixteenthQuestionAnswer = s.SixteenthQuestionAnswer,
                                                     SeventeenthQuestionAnswer = s.SeventeenthQuestionAnswer,
                                                     EighteenthQuestionAnswer = s.EighteenthQuestionAnswer,
                                                     NineteenthQuestionAnswer = s.NineteenthQuestionAnswer,
                                                     TwentiethQuestionAnswer = s.TwentiethQuestionAnswer,
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
                registeredScales = this.dbContext.ZungScalesForAnxiety
                                                 .Where(s => s.IsDeleted == false && s.PatientId == userId)
                                                 .OrderBy(s => s.CreatedOn)
                                                 .Select(s => new ZungScaleViewModel
                                                 {
                                                     Id = s.Id,
                                                     FirstQuestionAnswer = s.FirstQuestionAnswer,
                                                     SecondQuestionAnswer = s.SecondQuestionAnswer,
                                                     ThirdQuestionAnswer = s.ThirdQuestionAnswer,
                                                     FourthQuestionAnswer = s.FourthQuestionAnswer,
                                                     FifthQuestionAnswer = s.FifthQuestionAnswer,
                                                     SixthQuestionAnswer = s.SixthQuestionAnswer,
                                                     SeventhQuestionAnswer = s.SeventhQuestionAnswer,
                                                     EighthQuestionAnswer = s.EighthQuestionAnswer,
                                                     NinthQuestionAnswer = s.NinthQuestionAnswer,
                                                     TenthQuestionAnswer = s.TenthQuestionAnswer,
                                                     EleventhQuestionAnswer = s.EleventhQuestionAnswer,
                                                     TwelfthQuestionAnswer = s.TwelfthQuestionAnswer,
                                                     ThirteenthQuestionAnswer = s.ThirteenthQuestionAnswer,
                                                     FourteenthQuestionAnswer = s.FourteenthQuestionAnswer,
                                                     FifteenthQuestionAnswer = s.FifteenthQuestionAnswer,
                                                     SixteenthQuestionAnswer = s.SixteenthQuestionAnswer,
                                                     SeventeenthQuestionAnswer = s.SeventeenthQuestionAnswer,
                                                     EighteenthQuestionAnswer = s.EighteenthQuestionAnswer,
                                                     NineteenthQuestionAnswer = s.NineteenthQuestionAnswer,
                                                     TwentiethQuestionAnswer = s.TwentiethQuestionAnswer,
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

            return await PaginatedList<ZungScaleViewModel>.CreateAsync(registeredScales, pageIndex, pageSize);
        }

        public async Task<ZungScaleViewModel> GetByIdAsync(string scaleId, string userId)
        {
            // Get Zung's scale.
            ZungScaleForAnxiety? zungScale = await this.dbContext.ZungScalesForAnxiety
                                                                 .FirstOrDefaultAsync(s => s.Id == scaleId);

            // Boolean flag evaluating if scale belongs to currently logged user.
            bool loggedUserHasScale = this.dbContext.Users
                                                    .Where(u => u.Id == userId)
                                                    .Include(s => s.ZungScalesForAnxiety)
                                                    .Any(u => u.ZungScalesForAnxiety.Any(s => s.Id == scaleId));

            // Check if hacker has tried to change Zung's scale Id through page's HTML.
            // If it has been changed, Zung's scale eventually is going to be null (returned by FirstOrDefaultAsync method, line 231).

            // If it has been changed and somehow there is Zung's scale in database with given GUID primary key (it's not null)
            // we check aswell if logged user is owner of the existing scale to reduce even more the chance that
            // hacker could get/edit/delete other user's registered Zung's scale. If these two validations fail - HTTP 404 is returned.
            if (zungScale != null && loggedUserHasScale)
            {
                ZungScaleViewModel viewModel = new ZungScaleViewModel()
                {
                    Id = zungScale.Id,
                    FirstQuestionAnswer = zungScale.FirstQuestionAnswer,
                    SecondQuestionAnswer = zungScale.SecondQuestionAnswer,
                    ThirdQuestionAnswer = zungScale.ThirdQuestionAnswer,
                    FourthQuestionAnswer = zungScale.FourthQuestionAnswer,
                    FifthQuestionAnswer = zungScale.FifthQuestionAnswer,
                    SixthQuestionAnswer = zungScale.SixthQuestionAnswer,
                    SeventhQuestionAnswer = zungScale.SeventhQuestionAnswer,
                    EighthQuestionAnswer = zungScale.EighthQuestionAnswer,
                    NinthQuestionAnswer = zungScale.NinthQuestionAnswer,
                    TenthQuestionAnswer = zungScale.TenthQuestionAnswer,
                    EleventhQuestionAnswer = zungScale.EleventhQuestionAnswer,
                    TwelfthQuestionAnswer = zungScale.TwelfthQuestionAnswer,
                    ThirteenthQuestionAnswer = zungScale.ThirteenthQuestionAnswer,
                    FourteenthQuestionAnswer = zungScale.FourteenthQuestionAnswer,
                    FifteenthQuestionAnswer = zungScale.FifteenthQuestionAnswer,
                    SixteenthQuestionAnswer = zungScale.SixteenthQuestionAnswer,
                    SeventeenthQuestionAnswer = zungScale.SeventeenthQuestionAnswer,
                    EighteenthQuestionAnswer = zungScale.EighteenthQuestionAnswer,
                    NineteenthQuestionAnswer = zungScale.NineteenthQuestionAnswer,
                    TwentiethQuestionAnswer = zungScale.TwentiethQuestionAnswer,
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
            // Get Zung scale that is going to be deleted.
            ZungScaleForAnxiety? zungScale = await this.dbContext.ZungScalesForAnxiety
                                                                 .FirstOrDefaultAsync(x => x.Id == scaleId);

            // Boolean flag evaluating if logged user has the scale that is going to be deleted.
            bool loggedUserHasScale = this.dbContext.Users
                                                    .Where(x => x.Id == userId)
                                                    .Include(x => x.ZungScalesForAnxiety)
                                                    .Any(x => x.ZungScalesForAnxiety.Any(x => x.Id == scaleId));

            // Check if scale exists and belogs to user.
            if (zungScale != null && loggedUserHasScale)
            {
                zungScale.IsDeleted = true;
                zungScale.DeletedOn = DateTime.UtcNow;

                // Save changes in database.
                await this.dbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("Скалата на Zung не съществува или не принадлежи на потребителя.");
            }
        }

        public async Task Share(string scaleId, string doctorID)
        {
            // Get Doctor user.
            ApplicationUser? doctor = await this.dbContext.Users
                                                          .Include(u => u.SharedZungScalesForAnxietyWithMe)
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
                var scale = this.dbContext.ZungScalesForAnxiety.FirstOrDefault(s => s.Id == scaleId);

                // Check if scale exists.
                if (scale != null)
                {
                    // Check if scale is not shared already.
                    if (doctor.SharedZungScalesForAnxietyWithMe.Contains(scale) == false)
                    {
                        // Add existing scale to doctor user's collection of shared headaches.
                        doctor.SharedZungScalesForAnxietyWithMe.Add(scale);

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
               (answer != neverOrRarely &&
                answer != sometimes &&
                answer != often &&
                answer != veryOftenOrAlways))
            {
                return true;
            }

            return false;
        }
    }
}
