using System.Data.Entity.Migrations;

namespace NBMP_1.projekt.Migrations
{
    

    internal sealed class Configuration : DbMigrationsConfiguration<NBMP_1.projekt.MyContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
      
         //   SetSqlGenerator(Devart.Data.PostgreSql.Entity.Migrations.PgSqlConnectionInfo.InvariantName,
         //new Devart.Data.PostgreSql.Entity.Migrations.PgSqlEntityMigrationSqlGenerator());

        }

        protected override void Seed(NBMP_1.projekt.MyContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
