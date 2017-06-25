using BlockchainToSql.Models;
using Microsoft.EntityFrameworkCore;

namespace BlockchainToSql
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
    }
}
