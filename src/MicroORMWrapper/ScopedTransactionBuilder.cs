using System.Data;
using System.Threading.Tasks;

namespace MicroORMWrapper {
    public class ScopedTransactionBuilder<TSqlManager> where TSqlManager : SqlManager<IDatabaseConnection> {

        SqlManager<IDatabaseConnection> SqlManager { get; }

        public ScopedTransactionBuilder(SqlManager<IDatabaseConnection> sqlManager) =>
            SqlManager = sqlManager;

        public async ValueTask<ScopedTransaction> BeginScopedTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) =>
            await SqlManager.BeginScopedTransactionAsync(isolationLevel);
    }
}
