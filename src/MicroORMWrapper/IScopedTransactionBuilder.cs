using System.Data;
using System.Threading.Tasks;

namespace MicroORMWrapper {
    public interface IScopedTransactionBuilder {
        ValueTask<ScopedTransaction> BeginScopedTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    }
}
