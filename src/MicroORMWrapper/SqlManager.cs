namespace MicroORMWrapper;

public class SqlManager<TDatabaseConnection> : IAsyncDisposable where TDatabaseConnection : IDatabaseConnection {
    public DbConnection DbConnection { get; }

    DbTransaction? DbTransaction { get; set; }

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

    public async ValueTask BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) =>
        DbTransaction = await DbConnection.BeginTransactionAsync(isolationLevel);

    public async ValueTask<ScopedTransaction> BeginScopedTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) {
        DbTransaction = await DbConnection.BeginTransactionAsync(isolationLevel);

        return new ScopedTransaction(DbTransaction);
    }

    public async ValueTask CommitAsync() {
        if (DbTransaction.IsInvalid()) {
            return;
        }

        await DbTransaction.CommitAsync();
    }

    public async ValueTask RollbackIfUncommitedAsync() {
        if (DbTransaction.IsInvalid()) {
            return;
        }

        await DbTransaction.RollbackAsync();
    }

    DbTransaction? GetDbTransactionIfIsBegun() =>
        DbTransaction.IsInvalid() ? null : DbTransaction;

    void DisposeTransaction() {
        if (DbTransaction.IsInvalid()) {
            return;
        }

        DbTransaction.Rollback();
        DbTransaction.Dispose();
    }

    async ValueTask DisposeTransactionAsync() {
        if (DbTransaction.IsInvalid()) {
            return;
        }

        await DbTransaction.RollbackAsync();
        await DbTransaction.DisposeAsync();
    }

    public void CloseConnection() {
        if (!IsOpenedConnection) {
            return;
        }

        DbConnection.Close();
    }

    public async ValueTask CloseConnectionAsync() {
        if (!IsOpenedConnection) {
            return;
        }

        await DbConnection.CloseAsync();
    }

    public IEnumerable<TResult> Select<TResult>(string query) =>
        DbConnection.Query<TResult>(query, transaction: GetDbTransactionIfIsBegun());

    public IEnumerable<TResult> Select<TResult>(string query, object prameters) =>
        DbConnection.Query<TResult>(query, prameters, transaction: GetDbTransactionIfIsBegun());

    public IEnumerable<TResult> Select<TResult>((string query, object prameters) queryAndParameters) =>
        DbConnection.Query<TResult>(queryAndParameters.query, queryAndParameters.prameters, transaction: GetDbTransactionIfIsBegun());

    public IEnumerable<TResult> Select<TResult, TInclude1>(string query, Func<TResult, TInclude1, TResult> includeFunc, object prameters, string splitOn = "Id") =>
        DbConnection.Query(query, includeFunc, prameters, transaction: GetDbTransactionIfIsBegun(), true, splitOn);

    public IEnumerable<TResult> Select<TResult, TInclude1, TInclude2>(string query, Func<TResult, TInclude1, TInclude2, TResult> includeFunc, object prameters, string splitOn = "Id") =>
        DbConnection.Query(query, includeFunc, prameters, transaction: GetDbTransactionIfIsBegun(), true, splitOn);

    public Task<IEnumerable<TResult>> SelectAsync<TResult>(string query) =>
        DbConnection.QueryAsync<TResult>(query, transaction: GetDbTransactionIfIsBegun());

    public Task<IEnumerable<TResult>> SelectAsync<TResult>(string query, object prameters) =>
        DbConnection.QueryAsync<TResult>(query, prameters, transaction: GetDbTransactionIfIsBegun());

    public Task<IEnumerable<TResult>> SelectAsync<TResult>((string query, object prameters) queryAndParameters) =>
        DbConnection.QueryAsync<TResult>(queryAndParameters.query, queryAndParameters.prameters, transaction: GetDbTransactionIfIsBegun());

    public Task<IEnumerable<TResult>> SelectAsync<TResult, TInclude1>(string query, Func<TResult, TInclude1, TResult> includeFunc, object prameters, string splitOn = "Id") =>
        DbConnection.QueryAsync(query, includeFunc, prameters, transaction: GetDbTransactionIfIsBegun(), true, splitOn);

    public Task<IEnumerable<TResult>> SelectAsync<TResult, TInclude1, TInclude2>(string query, Func<TResult, TInclude1, TInclude2, TResult> includeFunc, object prameters, string splitOn = "Id") =>
        DbConnection.QueryAsync(query, includeFunc, prameters, transaction: GetDbTransactionIfIsBegun(), true, splitOn);

    public List<TResult> SelectAsList<TResult>(string query) =>
        DbConnection.Query<TResult>(query, transaction: GetDbTransactionIfIsBegun()).AsList();

    public List<TResult> SelectAsList<TResult>(string query, object prameters) =>
        DbConnection.Query<TResult>(query, prameters, transaction: GetDbTransactionIfIsBegun()).AsList();

    public List<TResult> SelectAsList<TResult>((string query, object prameters) queryAndParameters) =>
        DbConnection.Query<TResult>(queryAndParameters.query, queryAndParameters.prameters, transaction: GetDbTransactionIfIsBegun()).AsList();

    public List<TResult> SelectAsList<TResult, TInclude1>(string query, Func<TResult, TInclude1, TResult> includeFunc, object prameters, string splitOn = "Id") =>
        DbConnection.Query(query, includeFunc, prameters, transaction: GetDbTransactionIfIsBegun(), true, splitOn).AsList();

    public List<TResult> SelectAsList<TResult, TInclude1, TInclude2>(string query, Func<TResult, TInclude1, TInclude2, TResult> includeFunc, object prameters, string splitOn = "Id") =>
        DbConnection.Query(query, includeFunc, prameters, transaction: GetDbTransactionIfIsBegun(), true, splitOn).AsList();

    public BuiltInType GetValue<BuiltInType>(string query) =>
        DbConnection.ExecuteScalar<BuiltInType>(query, transaction: GetDbTransactionIfIsBegun());

    public BuiltInType GetValue<BuiltInType>(string query, object prameters) =>
        DbConnection.ExecuteScalar<BuiltInType>(query, prameters, transaction: GetDbTransactionIfIsBegun());

    public Task<BuiltInType> GetValueAsync<BuiltInType>(string query) =>
        DbConnection.ExecuteScalarAsync<BuiltInType>(query, transaction: GetDbTransactionIfIsBegun());

    public Task<BuiltInType> GetValueAsync<BuiltInType>(string query, object prameters) =>
        DbConnection.ExecuteScalarAsync<BuiltInType>(query, prameters, transaction: GetDbTransactionIfIsBegun());

    public int Execute(string command) =>
        DbConnection.Execute(command, transaction: GetDbTransactionIfIsBegun());

    public int Execute(string command, object prameters) =>
        DbConnection.Execute(command, prameters, transaction: GetDbTransactionIfIsBegun());

    public Task<int> ExecuteAsync(string command) =>
        DbConnection.ExecuteAsync(command, transaction: GetDbTransactionIfIsBegun());

    public Task<int> ExecuteAsync(string command, object prameters) =>
        DbConnection.ExecuteAsync(command, prameters, transaction: GetDbTransactionIfIsBegun());

    public Task<int> ExecuteAsync((string command, object prameters) commandAndParameters) =>
        DbConnection.ExecuteAsync(commandAndParameters.command, commandAndParameters.prameters, transaction: GetDbTransactionIfIsBegun());

    public void Dispose() {
        DisposeTransaction();
        CloseConnection();
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync() {
        await DisposeTransactionAsync();
        await CloseConnectionAsync();
        GC.SuppressFinalize(this);
    }
}
