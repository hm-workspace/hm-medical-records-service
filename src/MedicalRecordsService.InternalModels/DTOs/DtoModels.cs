using MedicalRecordsService.InternalModels.Entities;

namespace MedicalRecordsService.InternalModels.DTOs;

public class CreateMedicalRecordDto
{
    public string RecordNumber { get; set; } = string.Empty;
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTime RecordDate { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string TreatmentPlan { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

public class UpdateMedicalRecordDto
{
    public int DoctorId { get; set; }
    public DateTime RecordDate { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string TreatmentPlan { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

public class MedicalRecordDto : CreateMedicalRecordDto
{
    public int Id { get; set; }

    public static MedicalRecordDto FromEntity(MedicalRecordEntity entity) => new()
    {
        Id = entity.Id,
        RecordNumber = entity.RecordNumber,
        PatientId = entity.PatientId,
        DoctorId = entity.DoctorId,
        RecordDate = entity.RecordDate,
        Diagnosis = entity.Diagnosis,
        TreatmentPlan = entity.TreatmentPlan,
        Notes = entity.Notes
    };
}


