using System;
using System.ComponentModel.DataAnnotations;

namespace BlockchainToSql.Models
{
    public class Blocks
    {
        [Key]
        public long ID { get; set; }

        public int Length { get; set; }

        public long LockTime { get; set; }

        public long Nonce { get; set; }

        public byte[] PreviousBlockHash { get; set; }

        public long TargetDifficulty { get; set; }

        public DateTime TimeStamp { get; set; }

        public byte[] MerkleRoot {get; set;}
    }
}
