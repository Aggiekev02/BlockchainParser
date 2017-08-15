﻿namespace BlockchainParser.Tests.Parts
{
    using BlockchainParser.Parts;
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class InputTests
    {
        [TestMethod]
        public void InputTestParse_Coinbase()
        {
            using (var stream = new MemoryStream(TestData.CoinbaseInput))
            using( var reader = new BinaryReader(stream))
            {
                Input input = Input.Parse(reader);
                Assert.IsNotNull(input);
                Assert.IsTrue(TestUtilities.EqualByteArrays(input.TransactionHash, new byte[]{ 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}));
                Assert.AreEqual(uint.MaxValue, input.TransactionIndex);
                Assert.IsTrue(input.Coinbase);
                Assert.AreEqual(41, input.Script.Length);
                Assert.IsTrue(TestUtilities.EqualByteArrays(input.Script, new byte[] { 0x03, 0x4e, 0x01, 0x05, 0x06, 0x2f, 0x50, 0x32, 0x53, 0x48, 0x2f, 0x04, 0x72, 0xd3, 0x54, 0x54, 0x08, 0x5f, 0xff, 0xed, 0xf2, 0x40, 0x00, 0x00, 0xf9, 0x0f, 0x54, 0x69, 0x6d, 0x65, 0x20, 0x26, 0x20, 0x48, 0x65, 0x61, 0x6c, 0x74, 0x68, 0x20, 0x21 }));
                Assert.AreEqual((uint)0, input.SequenceNumber);
                Assert.IsTrue(input.Coinbase);
            }
        }
    }
}
