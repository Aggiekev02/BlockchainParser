using System.ComponentModel.DataAnnotations;

namespace BlockchainToSqlParallel.Models
{
    public class Transactions
    {
        [Key]
        public long ID { get; set; }

        public long BlockID { get; set; }

        public long Version { get; set; }

        public bool Coinbase { get; set; }
    }
}
