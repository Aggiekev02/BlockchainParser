using System.ComponentModel.DataAnnotations;

namespace BlockchainToSqlParallel.Models
{
    public class Outputs
    {
        [Key]
        public long ID { get; set; }

        public long TransactionID { get; set; }

        public long Value { get; set; }

        public byte[] Script { get; set; }
    }
}
