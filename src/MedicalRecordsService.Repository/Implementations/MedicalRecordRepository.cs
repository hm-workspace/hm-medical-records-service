using Dapper;
using MedicalRecordsService.Data;
using MedicalRecordsService.InternalModels.Entities;

namespace MedicalRecordsService.Repository;

public class MedicalRecordRepository : IMedicalRecordRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public MedicalRecordRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyCollection<MedicalRecordEntity>> GetAllAsync()
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = @"
                SELECT
                    Id,
                    CONCAT('MR-', Id) AS RecordNumber,
                    PatientId,
                    DoctorId,
                    COALESCE(UpdatedAt, CreatedAt) AS RecordDate,
                    Diagnosis,
                    TreatmentPlan,
                    Description AS Notes
                FROM medical_records
                ORDER BY COALESCE(UpdatedAt, CreatedAt) DESC";
            var rows = await connection.QueryAsync<MedicalRecordEntity>(sql);
            return rows.ToList();
        }
        catch
        {
            return new List<MedicalRecordEntity>();
        }
    }

    public async Task<IReadOnlyCollection<MedicalRecordEntity>> SearchAsync(string searchTerm)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = @"
                SELECT
                    Id,
                    CONCAT('MR-', Id) AS RecordNumber,
                    PatientId,
                    DoctorId,
                    COALESCE(UpdatedAt, CreatedAt) AS RecordDate,
                    Diagnosis,
                    TreatmentPlan,
                    Description AS Notes
                FROM medical_records
                WHERE CONCAT('MR-', Id) LIKE @SearchTerm
                   OR Diagnosis LIKE @SearchTerm 
                   OR TreatmentPlan LIKE @SearchTerm
                   OR Description LIKE @SearchTerm
                ORDER BY COALESCE(UpdatedAt, CreatedAt) DESC";
            var rows = await connection.QueryAsync<MedicalRecordEntity>(sql, new { SearchTerm = $"%{searchTerm}%" });
            return rows.ToList();
        }
        catch
        {
            return new List<MedicalRecordEntity>();
        }
    }

    public async Task<MedicalRecordEntity?> GetByIdAsync(int id)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = @"
                SELECT
                    Id,
                    CONCAT('MR-', Id) AS RecordNumber,
                    PatientId,
                    DoctorId,
                    COALESCE(UpdatedAt, CreatedAt) AS RecordDate,
                    Diagnosis,
                    TreatmentPlan,
                    Description AS Notes
                FROM medical_records
                WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<MedicalRecordEntity>(sql, new { Id = id });
        }
        catch
        {
            return null;
        }
    }

    public async Task<IReadOnlyCollection<MedicalRecordEntity>> GetByPatientIdAsync(int patientId)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = @"
                SELECT
                    Id,
                    CONCAT('MR-', Id) AS RecordNumber,
                    PatientId,
                    DoctorId,
                    COALESCE(UpdatedAt, CreatedAt) AS RecordDate,
                    Diagnosis,
                    TreatmentPlan,
                    Description AS Notes
                FROM medical_records
                WHERE PatientId = @PatientId
                ORDER BY COALESCE(UpdatedAt, CreatedAt) DESC";
            var rows = await connection.QueryAsync<MedicalRecordEntity>(sql, new { PatientId = patientId });
            return rows.ToList();
        }
        catch
        {
            return new List<MedicalRecordEntity>();
        }
    }

    public async Task<MedicalRecordEntity> CreateAsync(MedicalRecordEntity record)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = @"
            INSERT INTO medical_records
                (PatientId, DoctorId, AppointmentId, RecordType, Title, Description, Diagnosis, TreatmentPlan, FollowUpRequired, FollowUpDate, CreatedAt, UpdatedAt)
            VALUES
                (@PatientId, @DoctorId, NULL, 'General', COALESCE(NULLIF(@RecordNumber, ''), 'Medical Record'), @Notes, @Diagnosis, @TreatmentPlan, 0, NULL, SYSUTCDATETIME(), SYSUTCDATETIME());
            SELECT CAST(SCOPE_IDENTITY() as int)";
        var id = await connection.ExecuteScalarAsync<int>(sql, record);
        record.Id = id;
        record.RecordNumber = $"MR-{id}";
        if (record.RecordDate == default)
        {
            record.RecordDate = DateTime.UtcNow;
        }
        return record;
    }

    public async Task<bool> UpdateAsync(MedicalRecordEntity record)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = @"
                UPDATE medical_records 
                SET DoctorId = @DoctorId,
                    Description = @Notes,
                    Diagnosis = @Diagnosis,
                    TreatmentPlan = @TreatmentPlan,
                    UpdatedAt = SYSUTCDATETIME()
                WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, record);
            return rowsAffected > 0;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "DELETE FROM medical_records WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
        catch
        {
            return false;
        }
    }
}


