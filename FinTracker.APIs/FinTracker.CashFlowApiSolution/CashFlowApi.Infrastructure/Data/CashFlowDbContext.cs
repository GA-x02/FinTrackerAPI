using CashFlowApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CashFlowApi.Infrastructure.Data
{
    public class CashFlowDbContext(DbContextOptions<CashFlowDbContext> options) : DbContext(options)
    {
        public DbSet<CashFlow> СashFlows { get; set; }
    }
}
