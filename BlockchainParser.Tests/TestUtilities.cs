namespace BlockchainParser.Tests
{
    using System.Linq;

    internal static class TestUtilities
    {
        public static bool EqualByteArrays(byte[] a, byte[] b)
        {
            if (a == null && b == null)
                return true;

            if (a == null || b == null)
                return false;

            if (a.Length != b.Length)
                return false;

            for (var i = 0; i < a.Length; i++)
                if (a[i] != b[i])
                    return false;

            return true;
        }

        public static byte[] ReverseByteArray(byte[] a)
        {
            return a.Reverse().ToArray();
        }
    }
}
