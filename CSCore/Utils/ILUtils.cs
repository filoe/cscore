using System;

namespace CSCore.Utils
{
	internal static class ILUtils
	{
	    public static int SizeOf<T>() where T : struct
		{
#if IL
			sizeof !!T
			ret
#endif
			throw new NotImplementedException();
		}

		public static void WriteToMemory<T>(IntPtr dest, ref T data) where T : struct
		{
#if IL
			ldarg.0
			call void* [mscorlib]System.IntPtr::op_Explicit(native int)
			ldarg.1
			cpobj !!T
			ret
#endif
			throw new NotImplementedException();
		}

		public static T Read<T>(IntPtr src) where T : struct
		{
#if IL
			ldarg.0
			call void* [mscorlib]System.IntPtr::op_Explicit(native int)
			ldobj !!T
			ret
#endif
			throw new NotImplementedException();
		}

		public static T CastUnsafe<T>(ref T ptr) where T : struct
		{
#if IL
			.maxstack 1

			ldarg.0
			ret
#endif

			throw new NotImplementedException();
		}

		public static TDest[] CastArrayUnsafe<TSource, TDest>(TSource[] array) 
			where TSource : struct where TDest : struct
		{
#if IL
			ldarg.0
			ret
#endif

			throw new NotImplementedException();
		}

		public unsafe static void MemoryCopy(void* dest, void* src, int byteCount)
		{
#if IL
			ldarg.0
			ldarg.1
			ldarg.2
			unaligned. 1
			cpblk
			ret
#endif
			throw new NotImplementedException();
		}

		public unsafe static void MemoryCopy(IntPtr dest, IntPtr src, int byteCount)
		{
			MemoryCopy(dest.ToPointer(), src.ToPointer(), byteCount);
		}

		public static void MemoryCopy<TSource, TDest>(TDest[] dest, TSource[] src, int byteCount)
		{
#if IL
			ldarg dest
			ldc.i4 0
			ldelema !!TDest

			ldarg src
			ldc.i4 0
			ldelema !!TSource

			sizeof !!TSource
			ldarg byteCount
			mul

			cpblk
			ret
#endif
			throw new NotImplementedException();
		}

		public static unsafe void MemSet(IntPtr dest, byte value, int count)
		{
#if IL
			ldarg.0
			ldarg.1
			ldarg.2
			unaligned. 1
			initblk
			ret
#endif
			throw new NotImplementedException();
		}
	}
}