using Dapper;
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
            const string sql = "SELECT * FROM MedicalRecords ORDER BY RecordDate DESC";
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
                SELECT * FROM MedicalRecords 
                WHERE RecordNumber LIKE @SearchTerm 
                   OR Diagnosis LIKE @SearchTerm 
                   OR TreatmentPlan LIKE @SearchTerm
                ORDER BY RecordDate DESC";
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
            const string sql = "SELECT * FROM MedicalRecords WHERE Id = @Id";
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
            const string sql = "SELECT * FROM MedicalRecords WHERE PatientId = @PatientId ORDER BY RecordDate DESC";
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
            INSERT INTO MedicalRecords (RecordNumber, PatientId, DoctorId, RecordDate, Diagnosis, TreatmentPlan, Notes)
            VALUES (@RecordNumber, @PatientId, @DoctorId, @RecordDate, @Diagnosis, @TreatmentPlan, @Notes);
            SELECT CAST(SCOPE_IDENTITY() as int)";
        var id = await connection.ExecuteScalarAsync<int>(sql, record);
        record.Id = id;
        return record;
    }

    public async Task<bool> UpdateAsync(MedicalRecordEntity record)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = @"
                UPDATE MedicalRecords 
                SET DoctorId = @DoctorId,
                    RecordDate = @RecordDate,
                    Diagnosis = @Diagnosis,
                    TreatmentPlan = @TreatmentPlan,
                    Notes = @Notes
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
            const string sql = "DELETE FROM MedicalRecords WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
        catch
        {
            return false;
        }
    }
}


