namespace CSCore.Utils
{
    internal static class Memory
    {
        unsafe public static void MemorySet(int* result, int source, int count)
        {
            for (int n = count; n > 0; n--)
                *(result++) = source;
        }

        unsafe public static void MemoryCopy(int* result, int* source, int count)
        {
            for (int n = count; n > 0; n--)
            {
                *(result++) = *(source++);
            }
        }
    }
}