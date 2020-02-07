using System.Data.Common;

namespace MicroORMWrapper {
    public interface IDatabaseConnection {
        string ConnectionName { get; set; }

        DbConnection DbConnection { get; set; }
    }
}