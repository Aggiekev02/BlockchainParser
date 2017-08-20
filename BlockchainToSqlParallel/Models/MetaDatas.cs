﻿using System.ComponentModel.DataAnnotations;

namespace BlockchainToSqlParallel.Models
{
    public class MetaDatas
    {
        [Key]
        public long ID { get; set; }

        public string FilePath { get; set; }

        public long Position { get; set; }

        public long BlockchainPosition { get; set; }

        public long BlockLength { get; set; }
    }
}
