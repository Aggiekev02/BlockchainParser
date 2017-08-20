using BlockchainToSqlParallel.Models;
using Microsoft.EntityFrameworkCore;

namespace BlockchainToSqlParallel
{
    public class BlockchainContext : DbContext
    {
        public BlockchainContext(DbContextOptions<BlockchainContext> options) : base(options)
        {

        }

        public DbSet<Blocks> Blocks { get; set; }

        public DbSet<Inputs> Inputs { get; set; }

        public DbSet<Outputs> Outputs {get; set;}

        public DbSet<Transactions> Transactions { get; set; }

        public DbSet<MetaDatas> MetaDatas { get; set; }
    }
}
