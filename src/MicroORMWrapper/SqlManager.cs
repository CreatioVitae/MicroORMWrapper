using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace MicroORMWrapper {
    public class SqlManager<TDatabaseConnection> : IAsyncDisposable where TDatabaseConnection : IDatabaseConnection {
        public DbConnection DbConnection { get; }

        public string ConnectionName { get; }

        public bool IsOpenedConnection => DbConnection.State == ConnectionState.Open;

        public SqlManager(IDatabaseConnection databaseConnection) {
            DbConnection = databaseConnection.DbConnection;
            ConnectionName = databaseConnection.ConnectionName;

            OpenConnection();
        }

        public void OpenConnection() {
            if (IsOpenedConnection) {
                return;
            }

            DbConnection.Open();
        }

        public async ValueTask CloseConnectionAsync() {
            if (!IsOpenedConnection) {
                return;
            }

            DbConnection.CloseAsync();
        }

        public IEnumerable<TResult> Select<TResult>(string query) where TResult : class? {
            return DbConnection.Query<TResult>(query);
        }

        public IEnumerable<TResult> Select<TResult>(string query, object prameters) where TResult : class? {
            return DbConnection.Query<TResult>(query, prameters);
        }

        public IEnumerable<TResult> Select<TResult, TInclude1>(string query, Func<TResult, TInclude1, TResult> includeFunc, object prameters, string splitOn = "Id") where TResult : class? where TInclude1 : class? {
            return DbConnection.Query(query, includeFunc, prameters, null, true, splitOn);
        }

        public IEnumerable<TResult> Select<TResult, TInclude1, TInclude2>(string query, Func<TResult, TInclude1, TInclude2, TResult> includeFunc, object prameters, string splitOn = "Id") where TResult : class? where TInclude1 : class? where TInclude2 : class? {
            return DbConnection.Query(query, includeFunc, prameters, null, true, splitOn);
        }

        public Task<IEnumerable<TResult>> SelectAsync<TResult>(string query) where TResult : class? {
            return DbConnection.QueryAsync<TResult>(query);
        }

        public Task<IEnumerable<TResult>> SelectAsync<TResult>(string query, object prameters) where TResult : class? {
            return DbConnection.QueryAsync<TResult>(query, prameters);
        }

        public Task<IEnumerable<TResult>> SelectAsync<TResult, TInclude1>(string query, Func<TResult, TInclude1, TResult> includeFunc, object prameters, string splitOn = "Id") where TResult : class? where TInclude1 : class? {
            return DbConnection.QueryAsync(query, includeFunc, prameters, null, true, splitOn);
        }

        public Task<IEnumerable<TResult>> SelectAsync<TResult, TInclude1, TInclude2>(string query, Func<TResult, TInclude1, TInclude2, TResult> includeFunc, object prameters, string splitOn = "Id") where TResult : class? where TInclude1 : class? where TInclude2 : class? {
            return DbConnection.QueryAsync(query, includeFunc, prameters, null, true, splitOn);
        }

        public List<TResult> SelectAsList<TResult>(string query) where TResult : class? {
            return DbConnection.Query<TResult>(query).AsList();
        }

        public List<TResult> SelectAsList<TResult>(string query, object prameters) where TResult : class? {
            return DbConnection.Query<TResult>(query, prameters).AsList();
        }

        public List<TResult> SelectAsList<TResult, TInclude1>(string query, Func<TResult, TInclude1, TResult> includeFunc, object prameters, string splitOn = "Id") where TResult : class? where TInclude1 : class? {
            return DbConnection.Query(query, includeFunc, prameters, null, true, splitOn).AsList();
        }

        public List<TResult> SelectAsList<TResult, TInclude1, TInclude2>(string query, Func<TResult, TInclude1, TInclude2, TResult> includeFunc, object prameters, string splitOn = "Id") where TResult : class? where TInclude1 : class? where TInclude2 : class? {
            return DbConnection.Query(query, includeFunc, prameters, null, true, splitOn).AsList();
        }

        public BuiltInType GetValue<BuiltInType>(string query) {
            return DbConnection.ExecuteScalar<BuiltInType>(query);
        }

        public BuiltInType GetValue<BuiltInType>(string query, object prameters) {
            return DbConnection.ExecuteScalar<BuiltInType>(query, prameters);
        }

        public Task<BuiltInType> GetValueAsync<BuiltInType>(string query) {
            return DbConnection.ExecuteScalarAsync<BuiltInType>(query);
        }

        public Task<BuiltInType> GetValueAsync<BuiltInType>(string query, object prameters) {
            return DbConnection.ExecuteScalarAsync<BuiltInType>(query, prameters);
        }

        public int Execute(string query) {
            return DbConnection.Execute(query);
        }

        public int Execute(string query, object prameters) {
            return DbConnection.Execute(query, prameters);
        }

        public Task<int> ExecuteAsync(string query) {
            return DbConnection.ExecuteAsync(query);
        }

        public Task<int> ExecuteAsync(string query, object prameters) {
            return DbConnection.ExecuteAsync(query, prameters);
        }

        public async ValueTask DisposeAsync() {
            await CloseConnectionAsync();
            GC.SuppressFinalize(this);
        }
    }
}