using MicroORMWrapper;
using System.Data.Common;
using System.Data.SqlClient;

namespace Microsoft.Extensions.DependencyInjection {
    public static class ServiceCollectionExtensionLibrary {
        public static IServiceCollection AddSqlManager(this IServiceCollection serviceDescriptors, string connectionString) {
            return serviceDescriptors
                .AddScoped<DbConnection>((servicProvider) => new SqlConnection(connectionString))
                .AddScoped<SqlManager>();
        }
    }
}