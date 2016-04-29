namespace Stegosaurus.Utility.Extensions
{
    public static class ByteArrayExtensions
    {
        public static int ComputeHash(this byte[] _array)
        {
            int hash = 17;

            foreach (char value in _array)
                hash = hash * 23 + value.GetHashCode();

            return hash;
        }
    }
}
