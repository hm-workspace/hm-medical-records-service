using MedicalRecordsService.InternalModels.Entities;

namespace MedicalRecordsService.Services;

public static class MedicalRecordsStore
{
    public static int RecordSeed = 1;
    public static readonly List<MedicalRecordEntity> Records = new()
    {
        new MedicalRecordEntity
        {
            Id = 1,
            RecordNumber = "MR001",
            PatientId = 1,
            DoctorId = 1,
            RecordDate = DateTime.UtcNow.Date,
            Diagnosis = "Viral fever",
            TreatmentPlan = "Hydration and rest",
            Notes = "Follow-up in 3 days"
        }
    };
}


