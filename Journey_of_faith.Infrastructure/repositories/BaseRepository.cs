using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Infrastructure.common;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Journey_of_faith.Infrastructure.repositories
{
    public class BaseRepository
    {
        protected readonly IDbConnectionFactory _dbConnection;
        protected readonly TableSchemaName _schemaName;
        public BaseRepository(IDbConnectionFactory dbConnection, IOptions<TableSchemaName> schemaName)
        {
            _dbConnection = dbConnection;
            _schemaName = schemaName.Value;
        }
        protected async Task<T> ExecuteAsync<T>(Func<IDbConnection, Task<T>> query)
        {
            using var connection = _dbConnection.CreateConnection();
            return await query(connection);
        }

        protected async Task<T> QueryAsync<T>(Func<IDbConnection, Task<T>> query)
        {
            using var connection = _dbConnection.CreateConnection();
            return await query(connection);
        }
    }
}
