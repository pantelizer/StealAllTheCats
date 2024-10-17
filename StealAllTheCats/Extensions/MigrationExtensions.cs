//using Microsoft.EntityFrameworkCore;

//namespace StealAllTheCats.Extensions
//{
//    public static class MigrationExtensions
//    {
        
//            #region Public Methods

//            public static void ApplyMigrations(this IApplicationBuilder app)
//            {
//                ArgumentNullException.ThrowIfNull(app);

//                using IServiceScope scope = app.ApplicationServices.CreateScope();            

//                //Retrieve the registered db context from the DIC
//                using StealCatsDbContext dbContext =
//                    scope.ServiceProvider.GetRequiredService<StealCatsDbContext>();

//                dbContext.Database.Migrate();
//            }

//            #endregion Public Methods
        
//    }
//}
