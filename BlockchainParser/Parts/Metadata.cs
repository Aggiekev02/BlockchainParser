using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainParser.Parts
{
    public class Metadata
    {
        public string FilePath { get; private set; }

        public long Position { get; private set; }

        public long BlockchainPosition { get; private set; }

        public long BlockLength { get; private set; }

        public static Metadata BuildMetadata(string filePath, long position, long blockChainPosition, long blockLength)
        {
            var data = new Metadata
            {
                FilePath = filePath,
                Position = position,
                BlockchainPosition = blockChainPosition,
                BlockLength = blockLength
            };

            return data;
        }
    }
}
