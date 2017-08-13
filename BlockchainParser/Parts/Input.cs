namespace Temosoft.Bitcoin.Blockchain
{
    using System.Collections.Generic;
    using System.IO;

    public class Input
    {
        public byte[] TransactionHash;
        public uint TransactionIndex;
        public byte[] Script;
        public uint SequenceNumber;

        public static IEnumerable<Input> ParseMultiple(BinaryReader r, ulong count)
        {
            if (count == 0)
                return null;

            for (ulong i = 0; i < count; i++)
            {
                var input = Parse(r);

                yield return input;
            }
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