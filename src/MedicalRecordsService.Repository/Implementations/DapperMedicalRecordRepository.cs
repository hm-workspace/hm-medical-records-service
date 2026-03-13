using Dapper;
using MedicalRecordsService.Data;
using MedicalRecordsService.InternalModels.Entities;

namespace MedicalRecordsService.Repository;

public class DapperMedicalRecordRepository : IMedicalRecordRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DapperMedicalRecordRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyCollection<MedicalRecordEntity>> GetAllAsync()
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "SELECT * FROM MedicalRecords";
            var rows = await connection.QueryAsync<MedicalRecordEntity>(sql);
            return rows.ToList();
        }
        catch
        {
            return new List<MedicalRecordEntity>();
        }
    }
}


