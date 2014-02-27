using System;

namespace CSCore.Utils
{
    public static class Utils
    {


#if DEBUG

        internal static unsafe void DumpPtr(int* i, int count)
        {
            for (int n = 0; n < count; n++)
            {
                System.Diagnostics.Debug.WriteLine(n + " " + *(i++));
            }
        }

#endif
    }
}