using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace eshop_orderapi.Migrations.DbMigrations
{
    public static class MigrationExtension
    {
        public static IApplicationBuilder Migrate(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var runner = scope.ServiceProvider.GetService<IMigrationRunner>();
            runner.ListMigrations();
            //runner.MigrateUp(20220609014400);

            return app;
        }
    }
}