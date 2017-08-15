namespace BlockchainParser.Tests.Parts
{
    using BlockchainParser.Parts;
    using System.Linq;
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TransactionTests
    {
        [TestMethod]
        public void TransactionTestParse_Coinbase()
        {
            using (var stream = new MemoryStream(TestData.Transaction_Coinbase_Output1()))
            using (var reader = new BinaryReader(stream))
            {
                Transaction trans = Transaction.Parse(reader);
                Assert.IsNotNull(trans);
                Assert.AreEqual(1, trans.VersionNumber);
                Assert.AreEqual(1, trans.Inputs.ToList().Count);
                Assert.AreEqual(1, trans.Outputs.ToList().Count);
                Assert.AreEqual((uint)0, trans.LockTime);
                Assert.IsTrue(trans.Coinbase);
            }
        }
    }
}
