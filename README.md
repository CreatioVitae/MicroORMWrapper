MicroORMWrapper</br>
MicroORMWrapper.Extensions.DependencyInjection
===

Getting Started(.NET 5 / ASP.NET Core)
---
Create DatabaseConnection Object (Implement IDatabaseConnection)
```csharp
using MicroORMWrapper;
using System.Data.Common;

namespace WebApi.Kashilog.Repositories.DatabaseConnections {
    public class KashilogConnection : IDatabaseConnection {

        public string ConnectionName { get; set; } = null!;

        public DbConnection DbConnection { get; set; } = null!;
    }
}

```
```csharp
using MicroORMWrapper;
using System.Data.Common;

namespace WebApi.Kashilog.Repositories.DatabaseConnections {
    public class AccountConnection : IDatabaseConnection {
        public string ConnectionName { get; set; } = null!;
        public DbConnection DbConnection { get; set; } = null!;
    }
}

```

DI Settings(ServiceDescriptor Configuration)
---
```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WebApi.Kashilog.ConstantValues.Kashi;
using WebApi.Kashilog.Repositories.DatabaseConnections;

namespace WebApi.Kashilog {
    public static class StartupExtensionLibrary {
        internal static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services, IConfiguration configuration) {
            //SqlManager<TDatabaseConnection>
            services
                .AddSqlManager<KashilogConnection>((connectionName: DatabaseNameResource.KashilogDatabase, connectionString: configuration[$"kashilogDatabaseConfig:connectionString"]))
                .AddSqlManager<AccountConnection>((connectionName: DatabaseNameResource.AccountDatabase, connectionString: configuration[$"accountDatabaseConfig:connectionString"]));

            //Services,Repositories
            services.AddDefaultScopedServices(Assembly.GetExecutingAssembly().GetTypes());

            // HttpClientFactory

            //Logger.

            return services;
        }
    }
}
```

Constructor Injection
---
```csharp
using MicroORMWrapper;
using Service.Extensions.DependencyInjection.Markers;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Kashilog.DomainObjects.Kashi;
using WebApi.Kashilog.Repositories.DatabaseConnections;
using WebApi.Kashilog.Repositories.Kashi.Products.Sqls;

namespace WebApi.Kashilog.Repositories.Kashi.Products {
    public class ProductRepository : IRepository {

        private SqlManager<KashilogConnection> KashilogSqlManager { get; }

        private SqlManager<AccountConnection> AccountSqlManager { get; }

        public ProductRepository(SqlManager<KashilogConnection> kashilogSqlManager, SqlManager<AccountConnection> accountSqlManager) {
            KashilogSqlManager = kashilogSqlManager;
            AccountSqlManager = accountSqlManager;
        }

        // ...
    }
}
```

Use Transaction
---
```csharp
await using var scopedTransaction = await KashilogSqlManager.BeginScopedTransactionAsync();

// Any Process...

// If it is marked Complete, it commits when DisposeAsync fires. If not, roll back...
scopedTransaction.Complete();
```