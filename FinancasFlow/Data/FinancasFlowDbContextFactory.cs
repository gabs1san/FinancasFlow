
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FinancasFlow.Data
{
    public class FinancasFlowDbContextFactory
        : IDesignTimeDbContextFactory<FinancasFlowDbContext>
    {
        public FinancasFlowDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<FinancasFlowDbContext>()
                .UseSqlite("Data Source=financasflow-design.db3")
                .Options;

            return new FinancasFlowDbContext(options);
        }
    }
}
