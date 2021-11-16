using MicroORMWrapper;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensionLibrary {
    public static IServiceCollection AddSqlManager<TDatabaseConnection>(this IServiceCollection serviceDescriptors, (string connectionName, string connectionString) connectionSetting) where TDatabaseConnection : class, IDatabaseConnection, new() =>
        serviceDescriptors
            .AddScoped((serviceProvider) => new TDatabaseConnection { ConnectionName = connectionSetting.connectionName, DbConnection = new SqlConnection(connectionSetting.connectionString) })
            .AddScoped(serviceProvider => new SqlManager<TDatabaseConnection>(serviceProvider.GetRequiredService<TDatabaseConnection>()));

    public static IServiceCollection AddSqlManager<TDatabaseConnection>(this IServiceCollection serviceDescriptors, (string connectionName, DbConnection dbConnection) connectionSetting) where TDatabaseConnection : class, IDatabaseConnection, new() =>
        serviceDescriptors
            .AddScoped((serviceProvider) => new TDatabaseConnection { ConnectionName = connectionSetting.connectionName, DbConnection = connectionSetting.dbConnection })
            .AddScoped(serviceProvider => new SqlManager<TDatabaseConnection>(serviceProvider.GetRequiredService<TDatabaseConnection>()));
}
