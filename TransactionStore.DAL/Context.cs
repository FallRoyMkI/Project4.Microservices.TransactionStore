using Microsoft.EntityFrameworkCore;
using TransactionStore.Models.Entities;

namespace TransactionStore.DAL;

public class Context : DbContext
{
    public DbSet<TransactionEntity> Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.UseSqlServer(@"Data Source= 194.87.210.5;Initial Catalog = TStoreBourseDB;
                                TrustServerCertificate=True;User ID = student;Password=qwe!23;", builder => builder.EnableRetryOnFailure());
    }
}