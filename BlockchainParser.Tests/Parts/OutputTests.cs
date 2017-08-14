namespace BlockchainParser.Tests.Parts
{
    using BlockchainParser.Parts;
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class OutputTests
    {
        [TestMethod]
        public void OutputTestParse_Output1()
        {
            long satoshis = 2504275756;

            using (var stream = new MemoryStream(TestData.Output1))
            using (var reader = new BinaryReader(stream))
            {
                Output output = Output.Parse(reader);
                Assert.IsNotNull(output);
                Assert.AreEqual(satoshis, output.Value);
                Assert.AreEqual(0x19, output.Script.Length);
                Assert.IsTrue(TestUtilities.EqualByteArrays(output.Script, new byte[] { 0x76, 0xa9, 0x14, 0xa0, 0x9b, 0xe8, 0x04, 0x0c, 0xbf, 0x39, 0x99, 0x26, 0xae, 0xb1, 0xf4, 0x70, 0xc3, 0x7d, 0x13, 0x41, 0xf3, 0xb4, 0x65, 0x88, 0xac }));
            }
        }
    }
}
