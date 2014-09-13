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

	    public static unsafe IntPtr WriteToMemory<T>(IntPtr dest, T[] data, int offset, int count) where T : struct
	    {
	        return new IntPtr(WriteToMemory(dest.ToPointer(), data, offset, count));
	    }

	    public unsafe static void* WriteToMemory<T>(void* dest, T[] data, int offset, int count) where T : struct
		{
#if IL
			.maxstack 4
			.locals init([0] int32, [1] !!T& pinned)

			ldarg.0
			ldarg.1
			ldarg.2     //offset
			ldelema !!T //push the address of a specified array index (offset -> ldarg.2) onto the stack
			stloc.1     //save the value on top of the stack to index 1 (see !!T& pinned)
			ldloc.1     //push the previously stored value (index 1 of the local variables) onto the stack
			sizeof !!T  //sizeof(T)
			conv.i4     //cast the sizeof(T) to int -> (int)sizeof(T)
			ldarg.3     //count
			mul         //count * (int)sizeof(T)
			stloc.0     //save the result to index 0 of the local variables
			ldloc.0     //push the just calculated value onto the stack again
			unaligned. 1 
			cpblk       //copy the specified number of bytes (see the count * sizeof(T)) from a source to a dst address
			ldloc.0     //push the local variable of index 0 onto the stack
			conv.i      //convert the value to native int
			ldarg.0     //dest
			add         
			ret         //return

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

			ldarg byteCount

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