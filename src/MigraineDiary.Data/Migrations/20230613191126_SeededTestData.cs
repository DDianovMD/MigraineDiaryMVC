using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MigraineDiary.Data.Migrations
{
    public partial class SeededTestData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "Author", "Content", "CreatedOn", "CreatorId", "DeletedOn", "IsDeleted", "SourceUrl", "Title" },
                values: new object[] { "dummy-data-article-1", "Megan Brooks", "<p style=\"line-height: 1.15;\">Американската Агенция по храните и лекарствата (FDA) одобри <strong>zavegepant</strong> (<em>Zavzpret <sup>&reg;</sup>, Pfizer</em>) - първият калцитонин ген-свързан пептид (<strong><em>CGRP</em></strong>) рецепторен антагонист под формата на назален спрей за лечение при възрастни на остър пристъп от мигрена с или без аура.</p><p style=\"line-height: 1.15;\">Лекарството е одобрено на базата на резултатите от две рандомизирани, двойно-заслепени, плацебо-контролирании изследвания.</p><p style=\"line-height: 1.15;\">В трета фаза на <a href=\"https://www.thelancet.com/journals/laneur/article/PIIS1474-4422%2822%2900517-8/fulltext\" target=\"_blank\" rel=\"noopener noreferrer\" class=\"fr-strong\">клинично изследване</a>, публикувано през февруари 2023г. в <em>The Lancet Neurology</em>, интраназалното приложение на <em>zavegepant</em> показва статистически значими резултати спрямо плацебо по отношение на облекчаването на болката и облекчаването на най-тревожещия пациентите симптом на 2-рия час след приложението.</p><p style=\"line-height: 1.15;\">Допълнително <em>zavegepant</em> води до облекчение на болката в рамките на 15 минути след приложение, като ефектът се наблюдава до 48 часа след приложението при много от пациентите.</p><blockquote><p style=\"line-height: 1.15; margin-left: 20px;\"><em>&quot;Сред пациентите ми с мигрена, едно от най-важните неща при избора на метод за лечение е колко бързо започва да действа лекарството.&quot; - споделя д-р Kathleen Mullin от New England Institute of Neurology and Headache Стамфорд, Кънектикът в публикация от Pfizer.</em></p></blockquote><blockquote><p style=\"line-height: 1.15; margin-left: 20px;\"><em>&quot;Като назален спрей с бърза абсорбция, Zavzpret<sup>&nbsp;&reg;</sup> предлага алтернативна опция за лечение при пациенти, които имат нужда от облекчение на болката и не могат да приемат лекарства през устата поради гадене или повръщане. Така те могат бързо да се върнат към нормалното функциониране&quot;, добавя д-р Mullin.</em></p></blockquote><p style=\"line-height: 1.15;\"><em>Zavegepant</em><em>&nbsp;</em>е бил добре толериран от пациентите, участващи в клиничните проучвания. Не са докладвани сериозни странични ефекти от приложението му.</p><p style=\"line-height: 1.15;\">Най-честите странични ефекти, които се наблюдават с по-голяма честота при около 2% от пациентите, лекувани с трета генерация <em>CGRP</em> рецепторен антагонист, спрямо при пациенти, лекувани с плацебо, са нарушения на вкуса, гадене, дискомфорт в носа и повръщане.</p><p style=\"line-height: 1.15;\"><em>Zavegepant</em> е контраиндикиран при пациенти, които имат анамнестични данни за свръхчувствителност към <em>zavegepant</em> или към някой от другите компоненти в лекарството. Наблюдаваните реакции на свръхчувствителност по време на клиничните изпитвания на <em>zavegepant</em> включват подуване на лицето и уртикария.</p><p style=\"line-height: 1.15;\">Назалният спрей се очаква да бъде наличен на пазара от юли 2023г.</p><p style=\"line-height: 1.15;\">Източник: FDA Approves Zavegepant Nasal Spray for Acute Migraine - Medscape - Mar 14, 2023.</p>\r\n", new DateTime(2023, 6, 13, 19, 11, 25, 498, DateTimeKind.Utc).AddTicks(879), "ff3d52a7-7288-42aa-9955-6c4c4ad4caed", null, false, "FDA Approves Zavegepant Nasal Spray for Acute Migraine - Medscape - Mar 14, 2023.", "FDA одобри назалния спрей Zavegepant за лечение на мигренозен пристъп" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2d75eec2-411b-43d0-acb7-5ae4bf74555f",
                column: "ConcurrencyStamp",
                value: "dc4da88c-abcb-4050-b677-9e9327e7f1a6");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "dcf214a0-ab0e-4ce9-8cf3-74bb6dc6acf9", "37867dd9-dfa8-42a3-92fb-f503a0e27c27", "Doctor", "DOCTOR" },
                    { "ef68c501-9ba3-401c-b9df-23685ffafe53", "7b553f94-92e3-4694-9c7f-b1ae1a731e0c", "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ff3d52a7-7288-42aa-9955-6c4c4ad4caed",
                columns: new[] { "ConcurrencyStamp", "FirstName", "LastName", "MiddleName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4691a13e-c0e4-4ef6-9671-622607d30a38", "Admin", "Adminov", "Adminov", "AQAAAAEAACcQAAAAEMssP0Ob1FXF2v0qN8Mt+Qd3NMXQBZLJ1wG1j2V2jfJ8lKYB7m2HPCEZq4dkwjLvcQ==", "1e02e076-dae8-4cbb-9b05-4641cd36afdf" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsDeleted", "LastName", "LockoutEnabled", "LockoutEnd", "MiddleName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "6372cb29-7b06-4510-845a-375fe51edc06", 0, "2979b8e6-4aa7-4f4d-9fe7-7d92d6d01b6a", "testuser@migrainediary.com", true, "Pesho", false, "Petrov", true, null, "Petrov", "TESTUSER@MIGRAINEDIARY.COM", "TESTUSER", "AQAAAAEAACcQAAAAEHl5pMIfa040UFZmQoRpdYMY4ea2vUz0D0RKEknFpASuMJRGkEvig5lPrpLuXbenqg==", null, false, "68d3f1b3-64f0-49bb-9c5a-61ebec469e5c", false, "TestUser" },
                    { "88f40769-cbdb-4c19-80b3-e43ccf48e22d", 0, "f2e3d42e-5d6c-4164-8d3d-beb539735b68", "testdoctor@migrainediary.com", true, "Mincho", false, "Petrov", true, null, "Testov", "TESTDOCTOR@MIGRAINEDIARY.COM", "TESTDOCTOR", "AQAAAAEAACcQAAAAEAaC7RINmnWQnKirG6UjQ7XGE62t4SLUywbr224NDJXGlBUIjWpI1w1yXEj5AuQoUg==", null, false, "a862795f-0f93-46ec-882b-bdcf52b058ef", false, "TestDoctor" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "ef68c501-9ba3-401c-b9df-23685ffafe53", "6372cb29-7b06-4510-845a-375fe51edc06" },
                    { "dcf214a0-ab0e-4ce9-8cf3-74bb6dc6acf9", "88f40769-cbdb-4c19-80b3-e43ccf48e22d" }
                });

            migrationBuilder.InsertData(
                table: "ClinicalTrials",
                columns: new[] { "Id", "AgreementDocumentName", "City", "CreatedOn", "CreatorId", "DeletedOn", "Heading", "Hospital", "IsDeleted" },
                values: new object[] { "dummy-data-clinicaltrial-1", "ClinicalTrialInformation.pdf", "София", new DateTime(2023, 6, 15, 19, 11, 25, 497, DateTimeKind.Utc).AddTicks(8909), "88f40769-cbdb-4c19-80b3-e43ccf48e22d", null, "Evaluation of Peripheral Nerve Stimulation for Acute Treatment of Migraine Pain", "МБАЛНП \"Св. Наум\" ЕАД", false });

            migrationBuilder.InsertData(
                table: "HIT6Scales",
                columns: new[] { "Id", "CreatedOn", "DeletedOn", "FifthQuestionAnswer", "FirstQuestionAnswer", "FourthQuestionAnswer", "IsDeleted", "PatientId", "SecondQuestionAnswer", "SixthQuestionAnswer", "ThirdQuestionAnswer", "TotalScore" },
                values: new object[,]
                {
                    { "dummy-data-hit6scale-1", new DateTime(2023, 6, 13, 21, 11, 25, 496, DateTimeKind.Utc).AddTicks(9207), null, "Always", "Never", "Very often", false, "6372cb29-7b06-4510-845a-375fe51edc06", "Rarely", "Always", "Sometimes", 61 },
                    { "dummy-data-hit6scale-2", new DateTime(2023, 6, 13, 22, 11, 25, 497, DateTimeKind.Utc).AddTicks(757), null, "Never", "Never", "Never", false, "6372cb29-7b06-4510-845a-375fe51edc06", "Never", "Never", "Never", 96 }
                });

            migrationBuilder.InsertData(
                table: "Headaches",
                columns: new[] { "Id", "Aura", "AuraDescriptionNotes", "DurationDays", "DurationHours", "DurationMinutes", "EndTime", "LocalizationSide", "Nausea", "Onset", "PainCharacteristics", "PatientId", "Phonophoby", "Photophoby", "Severity", "Triggers", "Vomiting" },
                values: new object[,]
                {
                    { "dummy-data-headache-1", false, null, 0, 2, 13, new DateTime(2023, 6, 13, 21, 24, 25, 496, DateTimeKind.Utc).AddTicks(5912), "left side", true, new DateTime(2023, 6, 13, 19, 11, 25, 496, DateTimeKind.Utc).AddTicks(5738), "pulsating", "6372cb29-7b06-4510-845a-375fe51edc06", false, true, 9, "none", true },
                    { "dummy-data-headache-2", true, "Flashing lights", 0, 6, 10, new DateTime(2023, 6, 14, 1, 21, 25, 496, DateTimeKind.Utc).AddTicks(8073), "right side", true, new DateTime(2023, 6, 13, 19, 21, 25, 496, DateTimeKind.Utc).AddTicks(8072), "pulsating", "6372cb29-7b06-4510-845a-375fe51edc06", true, true, 9, "Alcohol usage", false }
                });

            migrationBuilder.InsertData(
                table: "ZungScalesForAnxiety",
                columns: new[] { "Id", "CreatedOn", "DeletedOn", "EighteenthQuestionAnswer", "EighthQuestionAnswer", "EleventhQuestionAnswer", "FifteenthQuestionAnswer", "FifthQuestionAnswer", "FirstQuestionAnswer", "FourteenthQuestionAnswer", "FourthQuestionAnswer", "IsDeleted", "NineteenthQuestionAnswer", "NinthQuestionAnswer", "PatientId", "SecondQuestionAnswer", "SeventeenthQuestionAnswer", "SeventhQuestionAnswer", "SixteenthQuestionAnswer", "SixthQuestionAnswer", "TenthQuestionAnswer", "ThirdQuestionAnswer", "ThirteenthQuestionAnswer", "TotalScore", "TwelfthQuestionAnswer", "TwentiethQuestionAnswer" },
                values: new object[,]
                {
                    { "dummy-data-zungscaleforanxiety-1", new DateTime(2023, 6, 13, 22, 11, 25, 497, DateTimeKind.Utc).AddTicks(2194), null, "Sometimes", "Sometimes", "Sometimes", "Sometimes", "Sometimes", "Sometimes", "Sometimes", "Sometimes", false, "Sometimes", "Sometimes", "6372cb29-7b06-4510-845a-375fe51edc06", "Sometimes", "Sometimes", "Sometimes", "Sometimes", "Sometimes", "Sometimes", "Sometimes", "Sometimes", 40, "Sometimes", "Sometimes" },
                    { "dummy-data-zungscaleforanxiety-2", new DateTime(2023, 6, 13, 22, 11, 25, 497, DateTimeKind.Utc).AddTicks(5592), null, "Never or rarely", "Never or rarely", "Never or rarely", "Never or rarely", "Never or rarely", "Never or rarely", "Never or rarely", "Never or rarely", false, "Never or rarely", "Never or rarely", "6372cb29-7b06-4510-845a-375fe51edc06", "Never or rarely", "Never or rarely", "Never or rarely", "Never or rarely", "Never or rarely", "Never or rarely", "Never or rarely", "Never or rarely", 20, "Never or rarely", "Never or rarely" }
                });

            migrationBuilder.InsertData(
                table: "Medications",
                columns: new[] { "Id", "CreatedOn", "DeletedOn", "GenericName", "HeadacheId", "IsDeleted", "Name", "NumberOfTakenPills", "SinglePillDosage", "Units" },
                values: new object[,]
                {
                    { "dummy-data-medication-1", new DateTime(2023, 6, 13, 19, 11, 25, 496, DateTimeKind.Utc).AddTicks(2668), null, "Dummy-Generic-Name-1", "dummy-data-headache-1", false, "Dummy-Data-Medication-1", 1m, 200m, "mg" },
                    { "dummy-data-medication-2", new DateTime(2023, 6, 13, 19, 12, 25, 496, DateTimeKind.Utc).AddTicks(4306), null, "Dummy-Generic-Name-2", "dummy-data-headache-1", false, "Dummy-Data-Medication-2", 1m, 500m, "mg" },
                    { "dummy-data-medication-3", new DateTime(2023, 6, 13, 19, 13, 25, 496, DateTimeKind.Utc).AddTicks(4386), new DateTime(2023, 6, 13, 19, 16, 25, 496, DateTimeKind.Utc).AddTicks(4389), "Dummy-Generic-Name-3", "dummy-data-headache-1", true, "Dummy-Data-Medication-3", 1m, 500m, "mg" },
                    { "dummy-data-medication-4", new DateTime(2023, 6, 13, 19, 22, 25, 496, DateTimeKind.Utc).AddTicks(4446), null, "Dummy-Generic-Name-4", "dummy-data-headache-2", false, "Dummy-Data-Medication-4", 1m, 200m, "mg" },
                    { "dummy-data-medication-5", new DateTime(2023, 6, 13, 19, 23, 25, 496, DateTimeKind.Utc).AddTicks(4449), null, "Dummy-Generic-Name-5", "dummy-data-headache-2", false, "Dummy-Data-Medication-5", 1m, 500m, "mg" },
                    { "dummy-data-medication-6", new DateTime(2023, 6, 13, 19, 24, 25, 496, DateTimeKind.Utc).AddTicks(4458), new DateTime(2023, 6, 13, 19, 27, 25, 496, DateTimeKind.Utc).AddTicks(4460), "Dummy-Generic-Name-6", "dummy-data-headache-2", true, "Dummy-Data-Medication-6", 1m, 500m, "mg" }
                });

            migrationBuilder.InsertData(
                table: "Practicioners",
                columns: new[] { "Id", "ClinicalTrialId", "CreatedOn", "DeletedOn", "FirstName", "IsDeleted", "Lastname", "Rank", "ScienceDegree" },
                values: new object[,]
                {
                    { "dummy-data-practicioner-1", "dummy-data-clinicaltrial-1", new DateTime(2023, 6, 15, 19, 11, 25, 497, DateTimeKind.Utc).AddTicks(6513), null, "Десислава", false, "Богданова", "docent", "д.м." },
                    { "dummy-data-practicioner-2", "dummy-data-clinicaltrial-1", new DateTime(2023, 6, 15, 19, 11, 25, 497, DateTimeKind.Utc).AddTicks(7702), null, "Васил", false, "Тодоров", "doctor", "д.м." }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "dummy-data-article-1");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "ef68c501-9ba3-401c-b9df-23685ffafe53", "6372cb29-7b06-4510-845a-375fe51edc06" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "dcf214a0-ab0e-4ce9-8cf3-74bb6dc6acf9", "88f40769-cbdb-4c19-80b3-e43ccf48e22d" });

            migrationBuilder.DeleteData(
                table: "HIT6Scales",
                keyColumn: "Id",
                keyValue: "dummy-data-hit6scale-1");

            migrationBuilder.DeleteData(
                table: "HIT6Scales",
                keyColumn: "Id",
                keyValue: "dummy-data-hit6scale-2");

            migrationBuilder.DeleteData(
                table: "Medications",
                keyColumn: "Id",
                keyValue: "dummy-data-medication-1");

            migrationBuilder.DeleteData(
                table: "Medications",
                keyColumn: "Id",
                keyValue: "dummy-data-medication-2");

            migrationBuilder.DeleteData(
                table: "Medications",
                keyColumn: "Id",
                keyValue: "dummy-data-medication-3");

            migrationBuilder.DeleteData(
                table: "Medications",
                keyColumn: "Id",
                keyValue: "dummy-data-medication-4");

            migrationBuilder.DeleteData(
                table: "Medications",
                keyColumn: "Id",
                keyValue: "dummy-data-medication-5");

            migrationBuilder.DeleteData(
                table: "Medications",
                keyColumn: "Id",
                keyValue: "dummy-data-medication-6");

            migrationBuilder.DeleteData(
                table: "Practicioners",
                keyColumn: "Id",
                keyValue: "dummy-data-practicioner-1");

            migrationBuilder.DeleteData(
                table: "Practicioners",
                keyColumn: "Id",
                keyValue: "dummy-data-practicioner-2");

            migrationBuilder.DeleteData(
                table: "ZungScalesForAnxiety",
                keyColumn: "Id",
                keyValue: "dummy-data-zungscaleforanxiety-1");

            migrationBuilder.DeleteData(
                table: "ZungScalesForAnxiety",
                keyColumn: "Id",
                keyValue: "dummy-data-zungscaleforanxiety-2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dcf214a0-ab0e-4ce9-8cf3-74bb6dc6acf9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ef68c501-9ba3-401c-b9df-23685ffafe53");

            migrationBuilder.DeleteData(
                table: "ClinicalTrials",
                keyColumn: "Id",
                keyValue: "dummy-data-clinicaltrial-1");

            migrationBuilder.DeleteData(
                table: "Headaches",
                keyColumn: "Id",
                keyValue: "dummy-data-headache-1");

            migrationBuilder.DeleteData(
                table: "Headaches",
                keyColumn: "Id",
                keyValue: "dummy-data-headache-2");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6372cb29-7b06-4510-845a-375fe51edc06");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "88f40769-cbdb-4c19-80b3-e43ccf48e22d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2d75eec2-411b-43d0-acb7-5ae4bf74555f",
                column: "ConcurrencyStamp",
                value: "d6671852-1e94-4cd8-b552-60d087df128a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ff3d52a7-7288-42aa-9955-6c4c4ad4caed",
                columns: new[] { "ConcurrencyStamp", "FirstName", "LastName", "MiddleName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c14baa9f-ca6e-4d5a-bea7-f5c0913332b3", null, null, null, "AQAAAAEAACcQAAAAEH9prb9jVYCpVWzVpeif4qHj6crcRXBXQNbyY6yYsnIrV49cO/hGVKRuh0TVg9Z+kg==", "da93050b-c7c9-48ea-b4e3-fda9e0688857" });
        }
    }
}
