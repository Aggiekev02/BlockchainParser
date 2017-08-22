using System.Collections.Generic;
using System.IO;
using System.Linq;
using BlockchainParser.Parts;

namespace BlockchainParser
{
    public class BlockchainMetadataParser : Blockchain
    {
        public override void Parse(string[] filesPath, Metadata metadata)
        {
            var streams = filesPath
                .Select(filePath => new KeyValuePair<string, Stream>(filePath, new FileStream(filePath, FileMode.Open, FileAccess.Read)))
                .ToList();

            using (var bufferedStream = new MultipleFilesStream(streams))
            {
                if (metadata != null)
                    bufferedStream.Position = metadata.BlockchainPosition + metadata.BlockLength;

                Parse(bufferedStream);
            }
        }

        protected override Block ReadBlock(Stream stream)
        {
            var block = Block.Parse(stream, true);

            return block;
        }
    }
}