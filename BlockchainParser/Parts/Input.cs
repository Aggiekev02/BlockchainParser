namespace BlockchainParser.Parts
{
    using System.Collections.Generic;
    using System.IO;

    public class Input
    {
        public byte[] TransactionHash { get; private set; }

        public uint TransactionIndex { get; private set; }

        public byte[] Script { get; private set; }

        public uint SequenceNumber { get; private set; }

        public bool Coinbase { get { return TransactionIndex == uint.MaxValue; } }

        public static List<Input> ParseMultiple(BinaryReader r, ulong count)
        {
            if (count == 0)
                return null;

            var list = new List<Input>((int)count);

            for (ulong i = 0; i < count; i++)
            {
                var input = Parse(r);

                list.Add(input);
            }

            return list;
        }

        public static Input Parse(BinaryReader r)
        {
            var input = new Input
            {
                TransactionHash = r.ReadHashAsByteArray(),
                TransactionIndex = r.ReadUInt32()
            };

            var scriptLength = r.ReadCompactSize();

            input.Script = r.ReadBytes((int)scriptLength);

            input.SequenceNumber = r.ReadUInt32();

            return input;
        }
    }
}