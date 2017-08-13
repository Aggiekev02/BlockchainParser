namespace Temosoft.Bitcoin.Blockchain
{
    using System.Collections.Generic;
    using System.IO;

    public class Transaction
    {
        public int VersionNumber;
        public IEnumerable<Input> Inputs;
        public IEnumerable<Output> Outputs;
        public uint LockTime;

        public static IEnumerable<Transaction> ParseMultiple(BinaryReader r, ulong transactionCount)
        {
            for (ulong ti = 0; ti < transactionCount; ti++)
            {
                var t = Parse(r);

                yield return t;
            }
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