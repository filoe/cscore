using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using CSCore.Utils;

namespace CSCore.XAudio2.X3DAudio
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct CurveNative
    {
        public IntPtr CurvePointsPtr;
        public int PointCount;

        public static unsafe IntPtr AllocMemoryAndBuildCurve(IEnumerable<CurvePoint> points)
        {
            if (points == null)
                return IntPtr.Zero;

            var p = points.ToArray();
            int pointCount = p.Length;
            if (pointCount <= 0)
                return IntPtr.Zero;

            CurveNative* nativeCurvePtr = (CurveNative*)((void*)Marshal.AllocHGlobal(GetSizeOfCurve(pointCount)));
            nativeCurvePtr->CurvePointsPtr = new IntPtr((CurvePoint*) &nativeCurvePtr[1]);
            nativeCurvePtr->PointCount = pointCount;
            ILUtils.WriteToMemory(nativeCurvePtr->CurvePointsPtr, p, 0, nativeCurvePtr->PointCount);

            return (IntPtr)nativeCurvePtr;
        }

        private static int GetSizeOfCurve(int pointCount)
        {
            return ILUtils.SizeOf<CurvePoint>() + ILUtils.SizeOf<CurvePoint>() * pointCount;
        }
    }
}