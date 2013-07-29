using CSCore.Win32;
using System;
using System.Runtime.InteropServices;

namespace CSCore.SoundOut.DirectSound
{
    public abstract class DSEffectBase : ComObject
    {
        public static readonly Guid StandardGargle =       new Guid("DAFD8210-5711-4B91-9FE3-F75B7AE279BF");
        public static readonly Guid StandartChorus =       new Guid("EFE6629C-81F7-4281-BD91-C9D604A95AF6");
        public static readonly Guid StandartFlanger =      new Guid("EFCA3D92-DFD8-4672-A603-7420894BAD98");
        public static readonly Guid StandartEcho = new Guid("ef3e932c-d40b-4f51-8ccf-3f98f1b29d5d");//new Guid("EF3E932C-D40B-4F51-8CCF-3F98F1B29D5D");
        public static readonly Guid StandartDistortion = new Guid("EF114C90-CD1D-484E-96E5-09CFAF912A21");
        public static readonly Guid StandartCompressor = new Guid("EF011F79-4000-406D-87AF-BFFB3FC39D57");
        public static readonly Guid StandartParamEQ = new Guid("120CED89-3BF4-4173-A132-3CB406CF3231");
        public static readonly Guid StandartI3DL2Reverb = new Guid("EF985E71-D5C7-42D4-BA4D-2D073E2E96F4");
        public static readonly Guid StandartWavesReverb = new Guid("87FC0268-9A55-4360-95AA-004A1D9DE26C");

        public DSEffectBase(IntPtr ptr)
            : base(ptr)
        {
        }
    }
}
