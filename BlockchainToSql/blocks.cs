//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BlockchainToSql
{
    using System;
    using System.Collections.Generic;
    
    public partial class blocks
    {
        public blocks()
        {
            this.Transactions = new HashSet<Transactions>();
        }
    
        public long ID { get; set; }
        public int Length { get; set; }
        public long LockTime { get; set; }
        public long Nonce { get; set; }
        public byte[] PreviousBlockHash { get; set; }
        public long TargetDifficulty { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public byte[] MerkleRoot { get; set; }
    
        public virtual ICollection<Transactions> Transactions { get; set; }
    }
}
