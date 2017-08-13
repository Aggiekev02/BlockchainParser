namespace BlockchainParser.Parts
{
    using System.Collections.Generic;
    using System.IO;

    public class Output
    {
        public ulong Value { get; private set; }
        public byte[] Script { get; private set; }

        public static IEnumerable<Output> ParseMultiple(BinaryReader r, ulong count)
        {
            if (count == 0)
                return null;

            var list = new List<Output>((int)count);

            for (ulong i = 0; i < count; i++)
            {
                var output = Parse(r);

                list.Add(output);
            }

            return list;
        }

        public static Output Parse(BinaryReader r)
        {
            var output = new Output();

            output.Value = r.ReadUInt64();

            var length = r.ReadCompactSize();

            output.Script = r.ReadBytes((int)length);

            return output;
        }
    }
}