using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MigraineDiary.Data.DbModels;

namespace MigraineDiary.Data.Common
{
    internal static class TestDataSeeder
    {
        // Constants.
        private const string ADMIN_ID = "ff3d52a7-7288-42aa-9955-6c4c4ad4caed";
        private const string USER_ID = "6372cb29-7b06-4510-845a-375fe51edc06";
        private const string DOCTOR_ID = "88f40769-cbdb-4c19-80b3-e43ccf48e22d";

        private const string ADMIN_ROLE_ID = "2d75eec2-411b-43d0-acb7-5ae4bf74555f";
        private const string USER_ROLE_ID = "ef68c501-9ba3-401c-b9df-23685ffafe53";
        private const string DOCTOR_ROLE_ID = "dcf214a0-ab0e-4ce9-8cf3-74bb6dc6acf9";

        // Variables.
        private static Dictionary<string, string> passwords = ReadPasswords();

        // Collections of dummy entities.
        private static IReadOnlyCollection<ApplicationUser> users = new List<ApplicationUser>
        {
            new ApplicationUser
                {
                    Id = ADMIN_ID,
                    UserName = "Admin",
                    FirstName = "Admin",
                    MiddleName = "Adminov",
                    LastName = "Adminov",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@migrainediary.com",
                    NormalizedEmail = "ADMIN@MIGRAINEDIARY.COM",
                    EmailConfirmed = true,
                    PasswordHash = HashPassword(passwords["adminPassword"]),
                    LockoutEnabled = true,
                    LockoutEnd = null,
                    AccessFailedCount = 0,
                    TwoFactorEnabled = false,
                    IsDeleted = false,
                },

                new ApplicationUser
                {
                    Id = USER_ID,
                    UserName = "TestUser",
                    FirstName = "Pesho",
                    MiddleName = "Petrov",
                    LastName = "Petrov",
                    NormalizedUserName = "TESTUSER",
                    Email = "testuser@migrainediary.com",
                    NormalizedEmail = "TESTUSER@MIGRAINEDIARY.COM",
                    EmailConfirmed = true,
                    PasswordHash = HashPassword(passwords["testUserPassword"]),
                    LockoutEnabled = true,
                    LockoutEnd = null,
                    AccessFailedCount = 0,
                    TwoFactorEnabled = false,
                    IsDeleted = false,
                },

                new ApplicationUser
                {
                    Id = DOCTOR_ID,
                    UserName = "TestDoctor",
                    FirstName = "Mincho",
                    MiddleName = "Testov",
                    LastName = "Petrov",
                    NormalizedUserName = "TESTDOCTOR",
                    Email = "testdoctor@migrainediary.com",
                    NormalizedEmail = "TESTDOCTOR@MIGRAINEDIARY.COM",
                    EmailConfirmed = true,
                    PasswordHash = HashPassword(passwords["testDoctorPassword"]),
                    LockoutEnabled = true,
                    LockoutEnd = null,
                    AccessFailedCount = 0,
                    TwoFactorEnabled = false,
                    IsDeleted = false,
                }
        }.AsReadOnly();

        private static IReadOnlyCollection<IdentityRole> roles = new List<IdentityRole>()
        {
            new IdentityRole
            {
                Id = ADMIN_ROLE_ID,
                Name = "Admin",
                NormalizedName = "ADMIN"
            },

            new IdentityRole
            {
                Id = USER_ROLE_ID,
                Name = "User",
                NormalizedName = "USER"
            },

            new IdentityRole
            {
                Id = DOCTOR_ROLE_ID,
                Name = "Doctor",
                NormalizedName = "DOCTOR"
            },
        }.AsReadOnly();

        private static IReadOnlyCollection<IdentityUserRole<string>> userRoles = new List<IdentityUserRole<string>>
        {
            new IdentityUserRole<string>()
            {
                RoleId = ADMIN_ROLE_ID,
                UserId = ADMIN_ID,
            },

            new IdentityUserRole<string>()
            {
                RoleId = USER_ROLE_ID,
                UserId = USER_ID,
            },

            new IdentityUserRole<string>()
            {
                RoleId = DOCTOR_ROLE_ID,
                UserId = DOCTOR_ID,
            },
        }.AsReadOnly();

        private static IReadOnlyCollection<Medication> medications = new List<Medication>
        {
            new Medication
            {
                    Id = "dummy-data-medication-1",
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = false,
                    DeletedOn = null,
                    Name = "Dummy-Data-Medication-1",
                    GenericName = "Dummy-Generic-Name-1",
                    NumberOfTakenPills = 1,
                    SinglePillDosage = 200,
                    Units = "mg",
                    HeadacheId = "dummy-data-headache-1",
            },

            new Medication
            {
                Id = "dummy-data-medication-2",
                CreatedOn = DateTime.UtcNow.AddMinutes(1),
                IsDeleted = false,
                DeletedOn = null,
                Name = "Dummy-Data-Medication-2",
                GenericName = "Dummy-Generic-Name-2",
                NumberOfTakenPills = 1,
                SinglePillDosage = 500,
                Units = "mg",
                HeadacheId = "dummy-data-headache-1",
            },

            new Medication
            {
                Id = "dummy-data-medication-3",
                CreatedOn = DateTime.UtcNow.AddMinutes(2),
                IsDeleted = true,
                DeletedOn = DateTime.UtcNow.AddMinutes(5),
                Name = "Dummy-Data-Medication-3",
                GenericName = "Dummy-Generic-Name-3",
                NumberOfTakenPills = 1,
                SinglePillDosage = 500,
                Units = "mg",
                HeadacheId = "dummy-data-headache-1",
            },

            new Medication
            {
                Id = "dummy-data-medication-4",
                CreatedOn = DateTime.UtcNow.AddMinutes(11),
                IsDeleted = false,
                DeletedOn = null,
                Name = "Dummy-Data-Medication-4",
                GenericName = "Dummy-Generic-Name-4",
                NumberOfTakenPills = 1,
                SinglePillDosage = 200,
                Units = "mg",
                HeadacheId = "dummy-data-headache-2",
            },

            new Medication
            {
                Id = "dummy-data-medication-5",
                CreatedOn = DateTime.UtcNow.AddMinutes(12),
                IsDeleted = false,
                DeletedOn = null,
                Name = "Dummy-Data-Medication-5",
                GenericName = "Dummy-Generic-Name-5",
                NumberOfTakenPills = 1,
                SinglePillDosage = 500,
                Units = "mg",
                HeadacheId = "dummy-data-headache-2",
            },

            new Medication
            {
                Id = "dummy-data-medication-6",
                CreatedOn = DateTime.UtcNow.AddMinutes(13),
                IsDeleted = true,
                DeletedOn = DateTime.UtcNow.AddMinutes(16),
                Name = "Dummy-Data-Medication-6",
                GenericName = "Dummy-Generic-Name-6",
                NumberOfTakenPills = 1,
                SinglePillDosage = 500,
                Units = "mg",
                HeadacheId = "dummy-data-headache-2",
            }
        }.AsReadOnly();

        private static IReadOnlyCollection<Headache> headaches = new List<Headache>
        {
            new Headache
            {
                    Id = "dummy-data-headache-1",
                    Onset = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow.AddDays(0).AddHours(2).AddMinutes(13),
                    DurationDays = 0,
                    DurationHours = 2,
                    DurationMinutes = 13,
                    LocalizationSide = "left side",
                    PainCharacteristics = "pulsating",
                    Severity = 9,
                    Nausea = true,
                    Vomiting = true,
                    Photophoby = true,
                    Phonophoby = false,
                    Aura = false,
                    AuraDescriptionNotes = null,
                    Triggers = "none",
                    PatientId = USER_ID,
            },

            new Headache
            {
                Id = "dummy-data-headache-2",
                Onset = DateTime.UtcNow.AddMinutes(10),
                EndTime = DateTime.UtcNow.AddDays(0).AddHours(6).AddMinutes(10),
                DurationDays = 0,
                DurationHours = 6,
                DurationMinutes = 10,
                LocalizationSide = "right side",
                PainCharacteristics = "pulsating",
                Severity = 9,
                Nausea = true,
                Vomiting = false,
                Photophoby = true,
                Phonophoby = true,
                Aura = true,
                AuraDescriptionNotes = "Flashing lights",
                Triggers = "Alcohol usage",
                PatientId = USER_ID,
            },
        }.AsReadOnly();

        private static IReadOnlyCollection<HIT6Scale> hit6scales = new List<HIT6Scale>
        {
            new HIT6Scale
            {
               Id = "dummy-data-hit6scale-1",
               CreatedOn = DateTime.UtcNow.AddHours(2),
               IsDeleted = false,
               DeletedOn = null,
               PatientId = USER_ID,
               FirstQuestionAnswer = "Never",
               SecondQuestionAnswer = "Rarely",
               ThirdQuestionAnswer = "Sometimes",
               FourthQuestionAnswer = "Very often",
               FifthQuestionAnswer = "Always",
               SixthQuestionAnswer = "Always",
               TotalScore = 61,
            },

            new HIT6Scale
            {
               Id = "dummy-data-hit6scale-2",
               CreatedOn = DateTime.UtcNow.AddHours(3),
               IsDeleted = false,
               DeletedOn = null,
               PatientId = USER_ID,
               FirstQuestionAnswer = "Never",
               SecondQuestionAnswer = "Never",
               ThirdQuestionAnswer = "Never",
               FourthQuestionAnswer = "Never",
               FifthQuestionAnswer = "Never",
               SixthQuestionAnswer = "Never",
               TotalScore = 96,
            },

        }.AsReadOnly();

        private static IReadOnlyCollection<ZungScaleForAnxiety> zungScalesForAnxiety = new List<ZungScaleForAnxiety>
        {
            new ZungScaleForAnxiety
            {
                Id = "dummy-data-zungscaleforanxiety-1",
                CreatedOn = DateTime.UtcNow.AddHours(3),
                IsDeleted = false,
                DeletedOn = null,
                PatientId = USER_ID,
                FirstQuestionAnswer = "Sometimes",
                SecondQuestionAnswer = "Sometimes",
                ThirdQuestionAnswer = "Sometimes",
                FourthQuestionAnswer = "Sometimes",
                FifthQuestionAnswer = "Sometimes",
                SixthQuestionAnswer = "Sometimes",
                SeventhQuestionAnswer = "Sometimes",
                EighthQuestionAnswer = "Sometimes",
                NinthQuestionAnswer = "Sometimes",
                TenthQuestionAnswer = "Sometimes",
                EleventhQuestionAnswer = "Sometimes",
                TwelfthQuestionAnswer = "Sometimes",
                ThirteenthQuestionAnswer = "Sometimes",
                FourteenthQuestionAnswer = "Sometimes",
                FifteenthQuestionAnswer = "Sometimes",
                SixteenthQuestionAnswer = "Sometimes",
                SeventeenthQuestionAnswer = "Sometimes",
                EighteenthQuestionAnswer = "Sometimes",
                NineteenthQuestionAnswer = "Sometimes",
                TwentiethQuestionAnswer = "Sometimes",
                TotalScore = 40,
            },

            new ZungScaleForAnxiety
            {
                Id = "dummy-data-zungscaleforanxiety-2",
                CreatedOn = DateTime.UtcNow.AddHours(3),
                IsDeleted = false,
                DeletedOn = null,
                PatientId = USER_ID,
                FirstQuestionAnswer = "Never or rarely",
                SecondQuestionAnswer = "Never or rarely",
                ThirdQuestionAnswer = "Never or rarely",
                FourthQuestionAnswer = "Never or rarely",
                FifthQuestionAnswer = "Never or rarely",
                SixthQuestionAnswer = "Never or rarely",
                SeventhQuestionAnswer = "Never or rarely",
                EighthQuestionAnswer = "Never or rarely",
                NinthQuestionAnswer = "Never or rarely",
                TenthQuestionAnswer = "Never or rarely",
                EleventhQuestionAnswer = "Never or rarely",
                TwelfthQuestionAnswer = "Never or rarely",
                ThirteenthQuestionAnswer = "Never or rarely",
                FourteenthQuestionAnswer = "Never or rarely",
                FifteenthQuestionAnswer = "Never or rarely",
                SixteenthQuestionAnswer = "Never or rarely",
                SeventeenthQuestionAnswer = "Never or rarely",
                EighteenthQuestionAnswer = "Never or rarely",
                NineteenthQuestionAnswer = "Never or rarely",
                TwentiethQuestionAnswer = "Never or rarely",
                TotalScore = 20,
            },

        }.AsReadOnly();

        private static IReadOnlyCollection<Practicioner> practicioners = new List<Practicioner>
        {
            new Practicioner
            {
                Id = "dummy-data-practicioner-1",
                CreatedOn = DateTime.UtcNow.AddDays(2),
                IsDeleted = false,
                DeletedOn = null,
                ClinicalTrialId = "dummy-data-clinicaltrial-1",
                Rank = "docent",
                FirstName = "Десислава",
                Lastname = "Богданова",
                ScienceDegree = "д.м."
            },

            new Practicioner
            {
                Id = "dummy-data-practicioner-2",
                CreatedOn = DateTime.UtcNow.AddDays(2),
                IsDeleted = false,
                DeletedOn = null,
                ClinicalTrialId = "dummy-data-clinicaltrial-1",
                Rank = "doctor",
                FirstName = "Васил",
                Lastname = "Тодоров",
                ScienceDegree = "д.м."
            },
        }.AsReadOnly();

        private static IReadOnlyCollection<ClinicalTrial> clinicalTrials = new List<ClinicalTrial>
        {
            new ClinicalTrial
            {
                Id = "dummy-data-clinicaltrial-1",
                CreatedOn = DateTime.UtcNow.AddDays(2),
                CreatorId = DOCTOR_ID,
                IsDeleted = false,
                DeletedOn = null,
                Heading = "Evaluation of Peripheral Nerve Stimulation for Acute Treatment of Migraine Pain",
                City = "София",
                Hospital = "МБАЛНП \"Св. Наум\" ЕАД",
                AgreementDocumentName = "ClinicalTrialInformation.pdf",
            },

        }.AsReadOnly();

        private static IReadOnlyCollection<Article> articles = new List<Article>
        {
            new Article
            {
                Id = "dummy-data-article-1",
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                DeletedOn = null,
                CreatorId = ADMIN_ID,
                Title = "FDA одобри назалния спрей Zavegepant за лечение на мигренозен пристъп",
                Content = "<p style=\"line-height: 1.15;\">Американската Агенция по храните и лекарствата" +
                " (FDA) одобри <strong>zavegepant</strong> (<em>Zavzpret <sup>&reg;</sup>, Pfizer</em>)" +
                " - първият калцитонин ген-свързан пептид (<strong><em>CGRP</em></strong>)" +
                " рецепторен антагонист под формата на назален спрей за лечение при възрастни на остър пристъп" +
                " от мигрена с или без аура.</p><p style=\"line-height: 1.15;\">Лекарството е одобрено на" +
                " базата на резултатите от две рандомизирани, двойно-заслепени, плацебо-контролирании" +
                " изследвания.</p><p style=\"line-height: 1.15;\">В трета фаза на" +
                " <a href=\"https://www.thelancet.com/journals/laneur/article/PIIS1474-4422%2822%2900517-8/fulltext\"" +
                " target=\"_blank\" rel=\"noopener noreferrer\" class=\"fr-strong\">клинично изследване</a>," +
                " публикувано през февруари 2023г. в <em>The Lancet Neurology</em>, интраназалното приложение на" +
                " <em>zavegepant</em> показва статистически значими резултати спрямо плацебо по отношение на" +
                " облекчаването на болката и облекчаването на най-тревожещия пациентите симптом на 2-рия час след" +
                " приложението.</p><p style=\"line-height: 1.15;\">Допълнително <em>zavegepant</em> води до" +
                " облекчение на болката в рамките на 15 минути след приложение, като ефектът се наблюдава до" +
                " 48 часа след приложението при много от пациентите.</p><blockquote><p style=\"line-height: 1.15;" +
                " margin-left: 20px;\"><em>&quot;Сред пациентите ми с мигрена, едно от най-важните неща при избора" +
                " на метод за лечение е колко бързо започва да действа лекарството.&quot; - споделя д-р Kathleen Mullin" +
                " от New England Institute of Neurology and Headache Стамфорд, Кънектикът в публикация от" +
                " Pfizer.</em></p></blockquote><blockquote><p style=\"line-height: 1.15; margin-left:" +
                " 20px;\"><em>&quot;Като назален спрей с бърза абсорбция, Zavzpret<sup>&nbsp;&reg;</sup> предлага" +
                " алтернативна опция за лечение при пациенти, които имат нужда от облекчение на болката и не могат" +
                " да приемат лекарства през устата поради гадене или повръщане. Така те могат бързо да се върнат към" +
                " нормалното функциониране&quot;, добавя д-р Mullin.</em></p></blockquote><p style=\"line-height:" +
                " 1.15;\"><em>Zavegepant</em><em>&nbsp;</em>е бил добре толериран от пациентите, участващи в клиничните" +
                " проучвания. Не са докладвани сериозни странични ефекти от приложението му.</p><p style=\"line-height:" +
                " 1.15;\">Най-честите странични ефекти, които се наблюдават с по-голяма честота при около 2% от" +
                " пациентите, лекувани с трета генерация <em>CGRP</em> рецепторен антагонист, спрямо при пациенти," +
                " лекувани с плацебо, са нарушения на вкуса, гадене, дискомфорт в носа и повръщане.</p><p style=\"line-height:" +
                " 1.15;\"><em>Zavegepant</em> е контраиндикиран при пациенти, които имат анамнестични данни за" +
                " свръхчувствителност към <em>zavegepant</em> или към някой от другите компоненти в лекарството." +
                " Наблюдаваните реакции на свръхчувствителност по време на клиничните изпитвания на <em>zavegepant</em>" +
                " включват подуване на лицето и уртикария.</p><p style=\"line-height: 1.15;\">Назалният спрей се очаква" +
                " да бъде наличен на пазара от юли 2023г.</p><p style=\"line-height: 1.15;\">Източник: FDA Approves" +
                " Zavegepant Nasal Spray for Acute Migraine - Medscape - Mar 14, 2023.</p>\r\n",
                Author = "Megan Brooks",
                SourceUrl = "FDA Approves Zavegepant Nasal Spray for Acute Migraine - Medscape - Mar 14, 2023.",
            },

        }.AsReadOnly();

        internal static void SeedUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
                .HasData(users);
        }

        internal static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>()
               .HasData(roles);
        }

        internal static void AssignRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserRole<string>>()
                .HasData(userRoles);
        }

        internal static void SeedData(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<Medication>()
                .HasData(medications);

            modelbuilder.Entity<Headache>()
                .HasData(headaches);

            modelbuilder.Entity<HIT6Scale>()
                .HasData(hit6scales);

            modelbuilder.Entity<ZungScaleForAnxiety>()
                .HasData(zungScalesForAnxiety);

            modelbuilder.Entity<ClinicalTrial>()
                .HasData(clinicalTrials);

            modelbuilder.Entity<Practicioner>()
                .HasData(practicioners);

            modelbuilder.Entity<Article>()
                .HasData(articles);
        }

        private static string HashPassword(string password)
        {
            PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();

            return passwordHasher.HashPassword(null!, password);
        }

        private static Dictionary<string, string> ReadPasswords()
        {
            string adminPasswordFilepath = Path.Combine(AppContext.BaseDirectory, "adminpassword.txt");
            string adminPassword = File.ReadAllText(adminPasswordFilepath);

            string testUserPasswordFilepath = Path.Combine(AppContext.BaseDirectory, "testuserpassword.txt");
            string testUserPassword = File.ReadAllText(testUserPasswordFilepath);

            string testDoctorPasswordFilepath = Path.Combine(AppContext.BaseDirectory, "testdoctorpassword.txt");
            string testDoctorPassword = File.ReadAllText(testDoctorPasswordFilepath);

            return new Dictionary<string, string>
            {
                {"adminPassword", adminPassword},
                {"testUserPassword", testUserPassword},
                {"testDoctorPassword", testDoctorPassword},
            };
        }
    }
}
