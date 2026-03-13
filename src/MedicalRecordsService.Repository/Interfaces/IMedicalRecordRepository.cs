using MedicalRecordsService.InternalModels.Entities;

namespace MedicalRecordsService.Repository;

public interface IMedicalRecordRepository
{
    Task<IReadOnlyCollection<MedicalRecordEntity>> GetAllAsync();
}

