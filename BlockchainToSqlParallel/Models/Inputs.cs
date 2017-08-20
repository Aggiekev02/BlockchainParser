﻿using System.ComponentModel.DataAnnotations;

namespace BlockchainToSqlParallel.Models
{
    public class Inputs
    {
        [Key]
        public long ID { get; set; }

        public long TransactionID { get; set; }

        public byte[] TransactionHash {get; set;}

        public long TransactionIndex { get; set; }

        public byte[] Script { get; set; }

        public long SequenceNumber { get; set; }

        public bool Coinbase { get; set; }
    }
}