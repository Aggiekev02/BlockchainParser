
namespace BlockchainParser
{
    using System.IO;
    using System.Linq;
    using BlockchainParser.Parts;

    public abstract class Blockchain
    {
        public void Parse(string[] filesPath)
        {
#if false
            var streams = filesPath
                .Select(filePath => MemoryMappedFile.CreateFromFile(filePath, FileMode.Open, Path.GetFileName(filePath), 0, MemoryMappedFileAccess.Read))
                .Select(mmf => mmf.CreateViewStream(0, 0, MemoryMappedFileAccess.Read))
                .Cast<Stream>()
                .ToList();
#else
            var streams = filesPath
                .Select(filePath => new FileStream(filePath, FileMode.Open, FileAccess.Read))
                .ToList();
            
#endif
            var bufferedStream = new MultipleFilesStream(streams);
            Parse(bufferedStream);
        }

        private void Parse(Stream stream)
        {
            using (var reader = new BinaryReader(stream))
            {
                while(ReadMagic(reader))
                {
                    var block = ReadBlock(reader);
                    ProcessBlock(block);
                }
            }
        }

        private bool ReadMagic(BinaryReader reader)
        {
            try
            {
                ini:
                byte b0 = reader.ReadByte();
                if (b0 != 0xF9) goto ini;
                b0 = reader.ReadByte();
                if (b0 != 0xbe) goto ini;
                b0 = reader.ReadByte();
                if (b0 != 0xb4) goto ini;
                b0 = reader.ReadByte();
                if (b0 != 0xd9) goto ini;
                return true;
            }
            catch( EndOfStreamException)
            {
                return false;
            }
        }

        protected virtual void ProcessBlock(Block block)
        {
        }

        private static Block ReadBlock(BinaryReader reader)
        {
            var block = Block.Parse(reader.BaseStream);

            reader.BaseStream.Seek(block.BlockLength, SeekOrigin.Current);

            return block;
        }
    }
}
