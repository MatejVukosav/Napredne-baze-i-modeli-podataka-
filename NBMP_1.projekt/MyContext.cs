using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Devart.Data.PostgreSql;

namespace NBMP_1.projekt
{

    public class MyContext : DbContext
    {

        public MyContext()
        {
        }

        public MyContext(PgSqlConnection connection)
          : base(connection, false)
        {
        }

        static MyContext()
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }

    }
}