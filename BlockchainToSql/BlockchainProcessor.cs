namespace BlockchainToSql
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using BlockchainToSql.Models;
    using BlockchainParser;
    using BlockchainParser.Parts;

    internal class BlockchainProcessor : Blockchain
    {
        private long _records;

        protected override void ProcessBlock(Block block)
        {
            var dbOptions = new DbContextOptionsBuilder<BlockchainContext>();
            dbOptions.UseSqlServer(@"Server=(local)\sql2016;Database=blockchain;Trusted_Connection=True;");

            using (var context = new BlockchainContext(dbOptions.Options))
            using(var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var blockEntity = new Blocks
                    {
                        Length = (int)block.BlockLength,
                        Version = block.VersionNumber,
                        MerkleRoot = block.MerkleRoot,
                        Nonce = block.Nonce,
                        BlockHash = block.BlockHash,
                        PreviousBlockHash = block.PreviousBlockHash,

                        Bits = (int)(block.Bits + int.MinValue),
                        TargetDifficulty = block.Difficulty,
                        TimeStamp = block.TimeStamp
                    };
                    _records++;

                    context.Blocks.Add(blockEntity);
                    context.SaveChanges();

                    foreach (var trans in block.Transactions)
                    {
                        var transactionEntity = new Transactions
                        {
                            Version = trans.VersionNumber,
                            BlockID = blockEntity.ID
                        };
                        _records++;
                        context.Transactions.Add(transactionEntity);
                        context.SaveChanges();

                        if (trans.Inputs != null)
                            foreach (var input in trans.Inputs)
                            {
                                if (input == null)
                                    continue;

                                context.Inputs.Add(new Inputs
                                {
                                    Script = input.Script,
                                    SequenceNumber = input.SequenceNumber,
                                    TransactionHash = input.TransactionHash,
                                    TransactionIndex = input.TransactionIndex,
                                    TransactionID = transactionEntity.ID
                                });
                                _records++;
                            }

                        if (trans.Outputs != null)
                            foreach (var output in trans.Outputs)
                            {
                                if (output == null)
                                    continue;

                                context.Outputs.Add(new Outputs
                                {
                                    Script = output.Script,
                                    Value = (long)output.Value,
                                    TransactionID = transactionEntity.ID
                                });
                                _records++;
                            }

                        context.SaveChanges();
                    }

                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                }
            }
        }
    }
}