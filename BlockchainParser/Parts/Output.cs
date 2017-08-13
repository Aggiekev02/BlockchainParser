namespace Temosoft.Bitcoin.Blockchain
{
    using System.Collections.Generic;
    using System.IO;

    public class Output
    {
        public ulong Value;
        public byte[] Script;

        public static IEnumerable<Output> ParseMultiple(BinaryReader r, ulong count)
        {
            if (count == 0)
                return null;

            for (ulong i = 0; i < count; i++)
            {
                var output = Parse(r);

                yield return output;
            }
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