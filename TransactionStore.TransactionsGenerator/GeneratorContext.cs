using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionStore.Models.Entities;

namespace TransactionStore.TransactionsGenerator
{
    public class GeneratorContext : DbContext
    {
        public DbSet<Accounts> Accounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(@"Data Source= 194.87.210.5;Initial Catalog = CrmBourseDB;
                                TrustServerCertificate=True;User ID = student;Password=qwe!23;", builder => builder.EnableRetryOnFailure());
        }
    }
}
