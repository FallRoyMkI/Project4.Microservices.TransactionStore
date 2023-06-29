using Microsoft.EntityFrameworkCore;
using TransactionStore.Models.Entities;

namespace TransactionStore.DAL;

public class Context : DbContext
{
    public DbSet<TransactionEntity> Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.UseSqlServer(Environment.GetEnvironmentVariable("TStoreConnectionString"), builder => builder.EnableRetryOnFailure());
    }
}