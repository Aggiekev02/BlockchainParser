using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Temosoft.Bitcoin.Blockchain
{
    public static class BinaryReaderExtensions
    {
        public static ulong ReadCompactSize(this BinaryReader reader)
        {
            var t = reader.ReadByte();
            if (t < 0xfd) return t;
            if (t == 0xfd) return reader.ReadUInt16();
            if (t == 0xfe) return reader.ReadUInt32();
            if (t == 0xff) return reader.ReadUInt64();

            throw new InvalidDataException("Reading Var Int");
        }

        public static byte[] ReadHashAsByteArray(this BinaryReader reader)
        {
            var bytes = new List<byte>(reader.ReadBytes(32));

            bytes.Reverse();

            return bytes.ToArray();
        }
    }

    public static class HashExtensions
    {
        public static string ToHashString(this byte[] byteArray)
        {
            return Encoding.UTF8.GetString(byteArray);
        }
    }

}