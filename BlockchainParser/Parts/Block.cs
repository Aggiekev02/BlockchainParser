using System;
using System.Collections.Generic;
using System.IO;

namespace BlockchainParser.Parts
{
    public class Block
    {
        private static DateTime _epochBaseDate = new DateTime(1970,1,1);

        private long Position { get; set; }

        private Stream Stream { get; set; }

        private BinaryReader Reader { get; set; }

        public uint HeaderLength { get; private set; }

        public int VersionNumber { get; private set; }

        public byte[] PreviousBlockHash { get; private set; }

        public byte[] MerkleRoot { get; private set; }

        public DateTime TimeStamp { get; private set; }

        public uint Bits { get; private set; }

        public uint Nonce { get; private set; }

        public double Difficulty { get; private set; }

        public IEnumerable<Transaction> Transactions { get; private set; }

        public Block()
        {

        }

        public static Block Parse(Stream stream)
        {
            var block = new Block();

            block.Position = stream.Position;
            block.Stream = stream;
            block.Reader = new BinaryReader(stream);

            block.HeaderLength = block.Reader.ReadUInt32();
            block.VersionNumber = block.Reader.ReadInt32();
            block.PreviousBlockHash = block.Reader.ReadHashAsByteArray();
            block.MerkleRoot = block.Reader.ReadHashAsByteArray();
            block.TimeStamp = _epochBaseDate.AddSeconds(block.Reader.ReadUInt32());
            block.Bits = block.Reader.ReadUInt32();
            block.Nonce = block.Reader.ReadUInt32();
            var transactionCount = block.Reader.ReadCompactSize();

            block.Transactions = Transaction.ParseMultiple(block.Reader, transactionCount);

            block.Difficulty = CalculateDifficulty(block.Bits);

            return block;
        }

        //static double max_body = fast_log(0x00ffff), scaland = fast_log(256);
        private static readonly double max_body = Math.Log(0x00ffff);
        private static readonly double scaland = Math.Log(256);

        private static double CalculateDifficulty(uint bits)
        {
            //return exp(max_body - fast_log(bits & 0x00ffffff) + scaland * (0x1d - ((bits & 0xff000000) >> 24)));
            var part1 = Math.Log(bits & 0x00ffffff);
            var part2 = 0x1d - ((bits & 0xff000000) >> 24);
            var exp = Math.Exp(max_body - part1 + scaland * part2);

            return exp;
        }
    }
}