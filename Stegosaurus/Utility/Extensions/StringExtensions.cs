namespace Stegosaurus.Utility.Extensions
{
    public static class StringExtensions
    {
        public static int ComputeHash(this string _str)
        {
            if (string.IsNullOrEmpty(_str))
                return 0;

            int hash = 17;

            foreach (char value in _str)
                hash = hash * 23 + value.GetHashCode();

            return hash;
        }
    }
}
