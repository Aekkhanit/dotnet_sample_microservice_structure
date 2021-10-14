using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Infrastructure.Context_Models;

namespace UserService.Infrastructure
{
    /// you can generate any context with Scaffolding
    /// In this conext just manual from code
    public class MyDB_Context : DbContext
    {
        public MyDB_Context(string connection_string) : base(GetOptions(connection_string))
        {
        }

        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString
                 ).Options;
        }

        public MyDB_Context(DbContextOptions<MyDB_Context> options): base(options)
        { }

        public virtual DbSet<TB_User> TB_User { get; set; }

    }

}
