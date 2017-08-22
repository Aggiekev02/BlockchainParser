using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BlockchainParser.Parts;
using BlockchainParser;
using Microsoft.EntityFrameworkCore;

namespace BlockchainToSqlParallel
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopWatch = new Stopwatch();

            try
            {
                var dbOptions = new DbContextOptionsBuilder<BlockchainContext>();
                dbOptions.UseSqlServer(@"Server=(local)\sql2016;Database=blockchain;Trusted_Connection=True;");

                Metadata metaData = null;

                using (var context = new BlockchainContext(dbOptions.Options))
                {
                    var metaDatas = context.MetaDatas.OrderByDescending(t => t.BlockchainPosition).Take(1)
                        .FirstOrDefault();

                    if (metaDatas != null)
                        metaData = Metadata.BuildMetadata(metaDatas.FilePath, metaDatas.Position,
                            metaDatas.BlockchainPosition, metaDatas.BlockLength);
                }

                var blocksFolder = Environment.ExpandEnvironmentVariables(@"%AppData%\Bitcoin\blocks");
                var filesPath = Directory.GetFiles(blocksFolder, "blk*.dat", SearchOption.TopDirectoryOnly);
                var parser = new BlockchainMetadataProcessor();

                stopWatch.Start();
                parser.Parse(filesPath, metaData);
                stopWatch.Stop();

                Console.WriteLine($"Done\nBlockchain Metadata parser ran for {stopWatch.ElapsedMilliseconds} ms.");
            }
            catch (Exception ex)
            {
                if (stopWatch.IsRunning)
                    stopWatch.Stop();

                Console.WriteLine($"Blockchain parse ran for {stopWatch.ElapsedMilliseconds} ms before erroring.\n{ex.Message}\n{ex.StackTrace}");
            }

            Console.ReadLine();
        }
    }
}
