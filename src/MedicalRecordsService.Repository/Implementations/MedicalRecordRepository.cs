using Dapper;
using MedicalRecordsService.Data;
using MedicalRecordsService.InternalModels.Entities;
using System.Data;

namespace MedicalRecordsService.Repository;

public class MedicalRecordRepository : BaseRepository, IMedicalRecordRepository
{
    public MedicalRecordRepository(IDbConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public async Task<IReadOnlyCollection<MedicalRecordEntity>> GetAllAsync()
    {
        var rows = await QueryAsync<MedicalRecordEntity>(
            StoredProcedureNames.GetMedicalRecords,
            commandType: CommandType.StoredProcedure);
        return rows.ToList();
    }

    public async Task<IReadOnlyCollection<MedicalRecordEntity>> SearchAsync(string searchTerm)
    {
        var rows = await QueryAsync<MedicalRecordEntity>(
            StoredProcedureNames.SearchMedicalRecords,
            new { SearchTerm = searchTerm },
            commandType: CommandType.StoredProcedure);
        return rows.ToList();
    }

    public Task<MedicalRecordEntity?> GetByIdAsync(int id)
    {
        return QuerySingleOrDefaultAsync<MedicalRecordEntity>(
            StoredProcedureNames.GetMedicalRecordById,
            new { Id = id },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IReadOnlyCollection<MedicalRecordEntity>> GetByPatientIdAsync(int patientId)
    {
        var rows = await QueryAsync<MedicalRecordEntity>(
            StoredProcedureNames.GetMedicalRecordsByPatientId,
            new { PatientId = patientId },
            commandType: CommandType.StoredProcedure);
        return rows.ToList();
    }

    public async Task<MedicalRecordEntity> CreateAsync(MedicalRecordEntity record)
    {
        var id = await ExecuteScalarAsync<int>(
            StoredProcedureNames.CreateMedicalRecord,
            record,
            commandType: CommandType.StoredProcedure);
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
        var rowsAffected = await ExecuteAsync(
            StoredProcedureNames.UpdateMedicalRecord,
            record,
            commandType: CommandType.StoredProcedure);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var rowsAffected = await ExecuteAsync(
            StoredProcedureNames.DeleteMedicalRecord,
            new { Id = id },
            commandType: CommandType.StoredProcedure);
        return rowsAffected > 0;
    }
}


