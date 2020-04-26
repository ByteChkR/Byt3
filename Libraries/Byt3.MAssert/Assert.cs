namespace Byt3.MAssert
{
    public static class Assert
    {
        public static void True(bool value)
        {
            ThrowIfNotEqual(value, true);
        }

        public static void False(bool value)
        {
            ThrowIfNotEqual(value, false);
        }

        public static void NotNull(object value)
        {
            ThrowIfEqual(value, null);
        }

        public static void ThrowIfEqual(object actual, object expected)
        {
            if (InternalEqual(actual, expected))
            {
                throw new MAssertException(actual, expected);
            }
        }

        public static void ThrowIfNotEqual(object actual, object expected)
        {
            if (!InternalEqual(actual, expected))
            {
                throw new MAssertException(actual, expected);
            }
        }


        private static bool InternalEqual(object actual, object expected)
        {
            if (expected == null)
            {
                return actual == null;
            }

            return expected.Equals(actual);
        }
    }
}