using MigraineDiary.ViewModels;

namespace MigraineDiary.Services.Contracts
{
    public interface IPatientService
    {
        public Task<PatientViewModel[]> GetAllPatientsAsync(string doctorId);
    }
}
