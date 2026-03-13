namespace MedicalRecordsService.InternalModels.Entities;

public class MedicalRecordEntity
{
    public int Id { get; set; }
    public string RecordNumber { get; set; } = string.Empty;
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTime RecordDate { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string TreatmentPlan { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}


