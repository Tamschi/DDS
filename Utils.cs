namespace DDS
{
    static class Utils
    {
        public const int INT_BITS = 32;

        public static void IncreaseToMultiple(this ref uint n, uint d)
        { if (n % d != 0) n += d - n % d; }
    }
}
