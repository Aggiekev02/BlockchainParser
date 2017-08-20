using System.IO;
using BlockchainParser.Parts;

namespace BlockchainParser
{
    public class BlockchainMetadataParser : Blockchain
    {
        protected override Block ReadBlock(Stream stream)
        {
            var block = Block.Parse(stream, true);

            return block;
        }
    }
}