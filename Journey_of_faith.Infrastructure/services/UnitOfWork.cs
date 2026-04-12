using Journey_of_faith.Application.common.interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Journey_of_faith.Infrastructure.services
{
    public sealed class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly IDbConnectionFactory _factory;
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        public IDbConnection Connection => _connection;
        public IDbTransaction Transaction => _transaction;

        public UnitOfWork(IDbConnectionFactory factory)
        {
            _factory = factory; 
        }

        public void BeginTransaction()
        {
            // tạo kết nối
            _connection = _factory.CreateConnection();
            // kiểm tra trạng thái kết nối
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
            // bắt đầu transaction
            _transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                _transaction.Commit();
            } finally
            {
                Dispose();
            }
        }
        public void RollBack()
        {
            try
            {
                _transaction.Rollback();
            } finally
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            _connection?.Dispose();
            _transaction?.Dispose();
        }
    }
}
