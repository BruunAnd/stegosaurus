namespace Stegosaurus.Utility.Extensions
{
    public static class ByteArrayExtensions
    {
        public static int ComputeHash(this byte[] _array)
        {
            if (_array == null)
                return 0;

            int hash = 17;

            foreach (byte value in _array)
                hash = hash * 23 + value.GetHashCode();

            return hash;
        }
    }
}
