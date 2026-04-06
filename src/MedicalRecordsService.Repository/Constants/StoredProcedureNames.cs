namespace MedicalRecordsService.Repository;

public static class StoredProcedureNames
{
    public const string GetMedicalRecords = "dbo.GetMedicalRecords";
    public const string SearchMedicalRecords = "dbo.SearchMedicalRecords";
    public const string GetMedicalRecordById = "dbo.GetMedicalRecordById";
    public const string GetMedicalRecordsByPatientId = "dbo.GetMedicalRecordsByPatientId";
    public const string CreateMedicalRecord = "dbo.CreateMedicalRecord";
    public const string UpdateMedicalRecord = "dbo.UpdateMedicalRecord";
    public const string DeleteMedicalRecord = "dbo.DeleteMedicalRecord";
}
