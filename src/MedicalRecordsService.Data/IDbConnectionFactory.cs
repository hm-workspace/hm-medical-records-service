using System.Data;

namespace MedicalRecordsService.Data;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}

