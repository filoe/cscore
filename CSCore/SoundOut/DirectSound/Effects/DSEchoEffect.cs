using System;

namespace CSCore.SoundOut.DirectSound
{
    public class DSEchoEffect : DSEffectBase
    {
        public static DSEffectDesc GetDefaultDescription()
        {
            return GetDefaultDescription(DSEffectFlags.Default);
        }

        public static DSEffectDesc GetDefaultDescription(DSEffectFlags flags)
        {
            return new DSEffectDesc(StandartEcho, flags);
        }

        public static DSEchoEffect Create(DirectSoundSecondaryBuffer secondaryBuffer)
        {
            var desc = GetDefaultDescription();

            return null;
        }

        public DSEchoEffect(IntPtr ptr)
            : base(ptr)
        {
        }
    }
}
