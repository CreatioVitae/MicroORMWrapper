using System.Data.Common;

namespace MicroORMWrapper {
    internal static class DbTransactionExtensions {
        internal static bool IsInvalid(this DbTransaction? transaction) =>
            transaction == null || transaction.Connection == null;
    }
}
