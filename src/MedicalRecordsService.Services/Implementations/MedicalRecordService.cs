using MedicalRecordsService.InternalModels.DTOs;
using MedicalRecordsService.InternalModels.Entities;
using MedicalRecordsService.Repository;

namespace MedicalRecordsService.Services;

public class MedicalRecordService : IMedicalRecordService
{
    private readonly IMedicalRecordRepository _repository;

    public MedicalRecordService(IMedicalRecordRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<MedicalRecordEntity>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<IReadOnlyCollection<MedicalRecordEntity>> SearchAsync(string searchTerm)
    {
        return await _repository.SearchAsync(searchTerm);
    }

    public async Task<MedicalRecordEntity?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<IReadOnlyCollection<MedicalRecordEntity>> GetByPatientIdAsync(int patientId)
    {
        return await _repository.GetByPatientIdAsync(patientId);
    }

    public async Task<MedicalRecordEntity> CreateAsync(CreateMedicalRecordDto dto)
    {
        var record = new MedicalRecordEntity
        {
            RecordNumber = dto.RecordNumber,
            PatientId = dto.PatientId,
            DoctorId = dto.DoctorId,
            RecordDate = dto.RecordDate,
            Diagnosis = dto.Diagnosis,
            TreatmentPlan = dto.TreatmentPlan,
            Notes = dto.Notes
        };

        return await _repository.CreateAsync(record);
    }

    public async Task<bool> UpdateAsync(int id, UpdateMedicalRecordDto dto)
    {
        var record = await _repository.GetByIdAsync(id);
        if (record is null)
        {
            return false;
        }

        record.DoctorId = dto.DoctorId;
        record.RecordDate = dto.RecordDate;
        record.Diagnosis = dto.Diagnosis;
        record.TreatmentPlan = dto.TreatmentPlan;
        record.Notes = dto.Notes;

        return await _repository.UpdateAsync(record);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}
