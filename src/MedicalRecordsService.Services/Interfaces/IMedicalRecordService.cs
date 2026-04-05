using MedicalRecordsService.InternalModels.DTOs;
using MedicalRecordsService.InternalModels.Entities;

namespace MedicalRecordsService.Services;

public interface IMedicalRecordService
{
    Task<IReadOnlyCollection<MedicalRecordEntity>> GetAllAsync();
    Task<IReadOnlyCollection<MedicalRecordEntity>> SearchAsync(string searchTerm);
    Task<MedicalRecordEntity?> GetByIdAsync(int id);
    Task<IReadOnlyCollection<MedicalRecordEntity>> GetByPatientIdAsync(int patientId);
    Task<MedicalRecordEntity> CreateAsync(CreateMedicalRecordDto dto);
    Task<bool> UpdateAsync(int id, UpdateMedicalRecordDto dto);
    Task<bool> DeleteAsync(int id);
}
