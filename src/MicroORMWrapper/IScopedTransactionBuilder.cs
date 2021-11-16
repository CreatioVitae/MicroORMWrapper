using System.Threading;

namespace MicroORMWrapper;

public interface IScopedTransactionBuilder {
    ValueTask<ScopedTransaction> BeginScopedTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

    ValueTask ExecutePooledCommandsAsync(CancellationToken cancellationToken = default);
}
