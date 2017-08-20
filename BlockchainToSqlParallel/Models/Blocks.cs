using System;
using System.ComponentModel.DataAnnotations;

namespace BlockchainToSqlParallel.Models
{
    public class Blocks
    {
        [Key]
        public long ID { get; set; }

        public int Length { get; set; }

        public int Version { get; set; }

        public long Nonce { get; set; }

        public int Bits { get; set; }

        public double TargetDifficulty { get; set; }

        public DateTime TimeStamp { get; set; }

        public byte[] BlockHash { get; set; }

        public byte[] PreviousBlockHash { get; set; }

        public byte[] MerkleRoot { get; set; }

        public long MetaDataID {get;set;}
    }
}
