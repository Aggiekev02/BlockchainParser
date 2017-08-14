namespace BlockchainParser.Tests.Parts
{
    using BlockchainParser.Parts;
    using System;
    using System.Linq;
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BlockTests
    {
        [TestMethod]
        public void BlockTestParse_Coinbase()
        {
            uint bits = 818092824;
            uint nonce = 4271835236;
            var timestamp = DateTime.Parse("11/6/2014 2:12:52 AM");

            using (var stream = new MemoryStream(TestData.Block_Transaction_Coinbase_Output1()))
            {
                Block block = Block.Parse(stream);
                Assert.IsNotNull(block);
                Assert.AreEqual(2, block.VersionNumber);
                Assert.IsTrue(TestUtilities.EqualByteArrays(TestUtilities.ReverseByteArray(block.PreviousBlockHash), new byte[] { 0xb6, 0xff, 0x0b, 0x1b, 0x16, 0x80, 0xa2, 0x86, 0x2a, 0x30, 0xca, 0x44, 0xd3, 0x46, 0xd9, 0xe8, 0x91, 0x0d, 0x33, 0x4b, 0xeb, 0x48, 0xca, 0x0c, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                Assert.IsTrue(TestUtilities.EqualByteArrays(TestUtilities.ReverseByteArray(block.MerkleRoot), new byte[] { 0x9d, 0x10, 0xaa, 0x52, 0xee, 0x94, 0x93, 0x86, 0xca, 0x93, 0x85, 0x69, 0x5f, 0x04, 0xed, 0xe2, 0x70, 0xdd, 0xa2, 0x08, 0x10, 0xde, 0xcd, 0x12, 0xbc, 0x9b, 0x04, 0x8a, 0xaa, 0xb3, 0x14, 0x71 }));
                Assert.AreEqual(timestamp, block.TimeStamp);
                //Assert.AreEqual(bits, block.Bits);
                Assert.AreEqual(Block.CalculateDifficulty(0x30c31b18), block.Difficulty);
                Assert.AreEqual(nonce, block.Nonce);
                Assert.AreEqual(1, block.Transactions.ToList().Count);
            }
        }

        [TestMethod]
        public void CaculateDifficulty_Block0()
        {
            Assert.AreEqual(1.0, Block.CalculateDifficulty(486604799));
        }

        [TestMethod]
        public void CaculateDifficulty_Block100000()
        {
            Assert.AreEqual(14484.16, Math.Round(Block.CalculateDifficulty(453281356), 2));
        }

        [TestMethod]
        public void CaculateDifficulty_Block200000()
        {
            Assert.AreEqual(2864140.51, Math.Round(Block.CalculateDifficulty(436591499), 2));
        }

        [TestMethod]
        public void CaculateDifficulty_Block300000()
        {
            Assert.AreEqual(8000872135.97, Math.Round(Block.CalculateDifficulty(419465580), 2));
        }

        [TestMethod]
        public void CaculateDifficulty_Block400000()
        {
            Assert.AreEqual(163491654908.96, Math.Round(Block.CalculateDifficulty(403093919), 2));
        }

        [TestMethod]
        public void CaculateDifficulty_Block480000()
        {
            Assert.AreEqual(923233068448.9, Math.Round(Block.CalculateDifficulty(402731232), 2));
        }
    }
}
