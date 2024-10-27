using Discount.Grpc.Data;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc;

    public static  class Extensions
    {
    public async static Task<IApplicationBuilder> UseMigration(this IApplicationBuilder app)
    {
        using var scope=app.ApplicationServices.CreateScope();//AS DiscountContext is scoped service injected to DI ,so we need scope to access it

        using var dbContext = scope.ServiceProvider.GetRequiredService<DiscountContext>();
        await dbContext.Database.MigrateAsync();
        return app;

    }
    }

