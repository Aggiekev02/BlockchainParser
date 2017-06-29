namespace Temosoft.Bitcoin.Blockchain
{
    public class Transaction
    {
        public int VersionNumber;
        public Input[] Inputs;
        public Output[] Outputs;
        public uint LockTime;
    }
}