using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace MicroORMWrapper {
    public class ScopedTransaction : IAsyncDisposable {
        DbTransaction? DbTransaction { get; set; } = null;

        bool ScopeIsComplete { get; set; } = false;

        public ScopedTransaction(DbTransaction? dbTransaction) =>
            DbTransaction = dbTransaction;

        public void Complete() {
            if (DbTransaction.IsInvalid()) {
                throw new ObjectDisposedException(nameof(DbTransaction));
            }

            if (ScopeIsComplete) {
                throw new InvalidOperationException($"Already marked as completed");
            }

            ScopeIsComplete = true;
        }

        public async ValueTask DisposeAsync() {
            if (DbTransaction.IsInvalid()) {
                return;
            }

#pragma warning disable CS8602 // IsInvalid での検査でNull検査済
            if (ScopeIsComplete) {
                await DbTransaction.CommitAsync();
                return;
            }

            await DbTransaction.RollbackAsync();
#pragma warning restore CS8602
        }
    }
}
