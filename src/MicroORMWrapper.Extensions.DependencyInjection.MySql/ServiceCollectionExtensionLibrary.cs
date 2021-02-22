using MicroORMWrapper;
using MySqlConnector;

namespace Microsoft.Extensions.DependencyInjection {
    public static class ServiceCollectionExtensionLibrary {
        public static IServiceCollection AddSqlManager<TDatabaseConnection>(this IServiceCollection serviceDescriptors, (string connectionName, string connectionString) connectionSetting) where TDatabaseConnection : class, IDatabaseConnection, new() =>
       serviceDescriptors
           .AddScoped((serviceProvider) => new TDatabaseConnection { ConnectionName = connectionSetting.connectionName, DbConnection = new MySqlConnection(connectionSetting.connectionString) })
           .AddScoped(serviceProvider => new SqlManager<TDatabaseConnection>(serviceProvider.GetRequiredService<TDatabaseConnection>()));
    }
}
