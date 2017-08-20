using System;
using System.Diagnostics;
using System.IO;

namespace BlockchainToSqlParallel
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopWatch = new Stopwatch();

            try
            {
                var blocksFolder = Environment.ExpandEnvironmentVariables(@"%AppData%\Bitcoin\blocks");
                var filesPath = Directory.GetFiles(blocksFolder, "blk*.dat", SearchOption.TopDirectoryOnly);
                var parser = new BlockchainMetadataProcessor();

                stopWatch.Start();
                parser.Parse(filesPath);
                stopWatch.Stop();

                Console.WriteLine($"Done\nBlockchain Metadata parser ran for {stopWatch.ElapsedMilliseconds} ms.");

                stopWatch.Reset();
                stopWatch.Start();
                parser.SaveAllMetadata();
                stopWatch.Stop();

                Console.WriteLine($"Done\nSaved all Metadata ran for {stopWatch.ElapsedMilliseconds} ms.");
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
