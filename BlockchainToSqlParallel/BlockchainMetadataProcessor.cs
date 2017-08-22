using BlockchainToSqlParallel.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlockchainParser.Parts;
using BlockchainParser;

namespace BlockchainToSqlParallel
{
    internal class BlockchainMetadataProcessor : BlockchainMetadataParser
    {
        protected override void ProcessBlock(Block block)
        {
            var dbOptions = new DbContextOptionsBuilder<BlockchainContext>();
            dbOptions.UseSqlServer(@"Server=(local)\sql2016;Database=blockchain;Trusted_Connection=True;");

            using (var context = new BlockchainContext(dbOptions.Options))
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var meta = new MetaDatas
                    {
                        FilePath = block.Metadata.FilePath,
                        Position = block.Metadata.Position,
                        BlockchainPosition = block.Metadata.BlockchainPosition,
                        BlockLength = block.Metadata.BlockLength
                    };

                    context.MetaDatas.Add(meta);

                    context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
        }

        public void ParallelParse(string[] filesPath)
        {
            var metadatas = GetMetadataList();

            var result = Parallel.ForEach(metadatas, (metadata, state) =>
            {
                var streams = filesPath
                    .Select(filePath => new KeyValuePair<string, Stream>(filePath, new FileStream(filePath, FileMode.Open, FileAccess.Read)))
                    .ToList();

                using (var stream = new MultipleFilesStream(streams))
                {
                    stream.Position = metadata.BlockchainPosition;

                    var block = Block.Parse(stream, (uint)metadata.BlockLength, false );

                    SaveBlockToSql(metadata.ID, block);
                }
            });
        }

        private List<MetaDatas> GetMetadataList()
        {
            var dbOptions = new DbContextOptionsBuilder<BlockchainContext>();
            dbOptions.UseSqlServer(@"Server=(local)\sql2016;Database=blockchain;Trusted_Connection=True;");

            using (var context = new BlockchainContext(dbOptions.Options))
                return context.MetaDatas.Where(t => !context.Blocks.Select(x => x.MetaDataID).Contains(t.ID)).ToList();
        }

        private void SaveBlockToSql(long metadataId, Block block)
        {
            var dbOptions = new DbContextOptionsBuilder<BlockchainContext>();
            dbOptions.UseSqlServer(@"Server=(local)\sql2016;Database=blockchain;Trusted_Connection=True;");

            using (var context = new BlockchainContext(dbOptions.Options))
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var blockEntity = new Blocks
                    {
                        Length = (int)block.Metadata.BlockLength,
                        Version = block.VersionNumber,
                        MerkleRoot = block.MerkleRoot,
                        Nonce = block.Nonce,
                        BlockHash = block.BlockHash,
                        PreviousBlockHash = block.PreviousBlockHash,

                        Bits = (int)(block.Bits + int.MinValue),
                        TargetDifficulty = block.Difficulty,
                        TimeStamp = block.TimeStamp,
                        MetaDataID = metadataId
                    };

                    context.Blocks.Add(blockEntity);
                    context.SaveChanges();

                    foreach (var trans in block.Transactions)
                    {
                        var transactionEntity = new Transactions
                        {
                            Version = trans.VersionNumber,
                            BlockID = blockEntity.ID
                        };

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
                            }

                        context.SaveChanges();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
        }
    }
}