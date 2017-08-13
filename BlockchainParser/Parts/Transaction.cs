namespace BlockchainParser.Parts
{
    using System.Collections.Generic;
    using System.IO;

    public class Transaction
    {
        public int VersionNumber { get; private set; }
        public IEnumerable<Input> Inputs { get; private set; }
        public IEnumerable<Output> Outputs { get; private set; }
        public uint LockTime { get; private set; }

        public static IEnumerable<Transaction> ParseMultiple(BinaryReader r, ulong count)
        {
            var list = new List<Transaction>((int)count);

            for (ulong ti = 0; ti < count; ti++)
            {
                var t = Parse(r);

                list.Add(t);
            }

            return list;
        }

        public static Transaction Parse(BinaryReader r)
        {
            var t = new Transaction();
            t.VersionNumber = r.ReadInt32();

            var inputCount = r.ReadCompactSize();
            t.Inputs = Input.ParseMultiple(r, inputCount);

            var outputCount = r.ReadCompactSize();
            t.Outputs = Output.ParseMultiple(r, outputCount);

            t.LockTime = r.ReadUInt32();

            return t;
        }
    }
}