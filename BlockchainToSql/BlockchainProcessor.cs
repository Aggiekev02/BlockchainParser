using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using BlockchainToSql.Models;
using Temosoft.Bitcoin.Blockchain;

namespace BlockchainToSql
{
    internal class BlockchainProcessor : BlockchainParser
    {
        private long _records;

        protected override void ProcessBlock(Block block)
        {
            var dbOptions = new DbContextOptionsBuilder<BlockchainContext>();
            dbOptions.UseSqlServer(@"Server=(local)\sql2016;Database=blockchain;Trusted_Connection=True;");

            using (var context = new BlockchainContext(dbOptions.Options))
            {
                var blockEntity = new Blocks
                {
                    Length = (int)block.HeaderLength,
                    LockTime = block.LockTime,
                    MerkleRoot = block.MerkleRoot,
                    Nonce = block.Nonce,
                    PreviousBlockHash = block.PreviousBlockHash,
                    TargetDifficulty = block.Difficulty,
                    TimeStamp = block.TimeStamp
                };
                _records++;

                context.Blocks.Add(blockEntity);
                context.SaveChanges();

                foreach (var transaction in block.Transactions)
                {
                    var transactionEntity = new Transactions
                    {
                        Version = transaction.VersionNumber,
                        BlockID = blockEntity.ID
                    };
                    _records++;
                    context.Transactions.Add(transactionEntity);
                    context.SaveChanges();

                    if (transaction.Inputs != null)
                        foreach (var input in transaction.Inputs)
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

                    if (transaction.Outputs != null)
                        foreach (var output in transaction.Outputs)
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
            }
        }
    }
}