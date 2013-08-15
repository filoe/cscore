using System;
using System.Runtime.InteropServices;

namespace CSCore.SoundOut.DirectSound
{
    [Guid("8bd28edf-50db-4e92-a2bd-445488d1ed42")]
    public class DSEchoEffect : DSEffectBase
    {
        //for details see dsound.h

        #region constants

        /// <summary>
        /// Default percentage of output fed back into input.
        /// </summary>
        public const float FeedbackDefault = 50f;

        /// <summary>
        /// Maximum percentage of output fed back into input.
        /// </summary>
        public const float FeedbackMax = 100f;

        /// <summary>
        /// Minimum percentage of output fed back into input.
        /// </summary>
        public const float FeedbackMin = 0f;

        /// <summary>
        /// Default delay for left channel, in milliseconds.
        /// </summary>
        public const float LeftDelayDefault = 500f;

        /// <summary>
        /// Maximum delay for left channel, in milliseconds.
        /// </summary>
        public const float LeftDelayMax = 2000f;

        /// <summary>
        /// Minimum delay for left channel, in milliseconds.
        /// </summary>
        public const float LeftDelayMin = 1f;

        /// <summary>
        /// Default value that specifies whether to swap left and right delays with each successive
        /// echo. The default value is zero, meaning no swap.
        /// </summary>
        public const int PanDelayDefault = 0;

        /// <summary>
        /// Maximum value that specifies whether to swap left and right delays with each successive
        /// echo. The default value is zero, meaning no swap.
        /// </summary>
        public const int PanDelayMax = 1;

        /// <summary>
        /// Minimum value that specifies whether to swap left and right delays with each successive
        /// echo. The default value is zero, meaning no swap.
        /// </summary>
        public const int PanDelayMin = 0;

        /// <summary>
        /// Default delay for right channel, in milliseconds.
        /// </summary>
        public const float RightDelayDefault = 500f;

        /// <summary>
        /// Maximum delay for right channel, in milliseconds.
        /// </summary>
        public const float RightDelayMax = 2000f;

        /// <summary>
        /// Minimum delay for right channel, in milliseconds.
        /// </summary>
        public const float RightDelayMin = 1f;

        /// <summary>
        /// Default ratio of wet (processed) signal to dry (unprocessed) signal.
        /// </summary>
        public const float WetDryMixDefault = 50f;

        /// <summary>
        /// Maximum ratio of wet (processed) signal to dry (unprocessed) signal.
        /// </summary>
        public const float WetDryMixMax = 100f;

        /// <summary>
        /// Minimum ratio of wet (processed) signal to dry (unprocessed) signal.
        /// </summary>
        public const float WetDryMixMin = 0f;

        #endregion constants

        public static DSEffectDesc GetDefaultDescription()
        {
            return GetDefaultDescription(DSEffectFlags.Default);
        }

        public static DSEffectDesc GetDefaultDescription(DSEffectFlags flags)
        {
            return new DSEffectDesc(StandartEcho, flags);
        }

        public DSFXEcho Parameters
        {
            get
            {
                DSFXEcho settings;
                DirectSoundException.Try(GetAllParameters(out settings), "IDirectSoundFXEcho8", "GetAllParameters");
                return settings;
            }
            set
            {
                DirectSoundException.Try(SetAllParameters(ref value), "IDirectSoundFXEcho8", "SetAllParameters");
            }
        }

        /// <summary>
        /// Ratio of wet (processed) signal to dry (unprocessed) signal. Must be in the range from
        /// DSFXECHO_WETDRYMIX_MIN through DSFXECHO_WETDRYMIX_MAX (all wet). The default value is
        /// 50.
        /// </summary>
        public float WetDryMix
        {
            get
            {
                return Parameters.WetDryMix;
            }
            set
            {
                if (value < WetDryMixMin || value > WetDryMixMax)
                    throw new ArgumentOutOfRangeException("value", "See WetDryMixMin and WetDryMixMay.");
                var parameters = Parameters;
                parameters.WetDryMix = value;
                Parameters = parameters;
            }
        }

        /// <summary>
        /// Percentage of output fed back into input, in the range from DSFXECHO_FEEDBACK_MIN
        /// through DSFXECHO_FEEDBACK_MAX. The default value is 50.
        /// </summary>
        public float Feedback
        {
            get
            {
                return Parameters.Feedback;
            }
            set
            {
                if (value < FeedbackMin || value > FeedbackMax)
                    throw new ArgumentOutOfRangeException("value", "See FeedbackMin and FeedbackMax.");
                var parameters = Parameters;
                parameters.Feedback = value;
                Parameters = parameters;
            }
        }

        /// <summary>
        /// Delay for left channel, in milliseconds, in the range from DSFXECHO_LEFTDELAY_MIN
        /// through DSFXECHO_LEFTDELAY_MAX. The default value is 500 ms.
        /// </summary>
        public float LeftDelay
        {
            get
            {
                return Parameters.LeftDelay;
            }
            set
            {
                if (value < LeftDelayMin || value > LeftDelayMax)
                    throw new ArgumentOutOfRangeException("value", "See LeftDelayMin and LeftDelayMax.");
                var parameters = Parameters;
                parameters.LeftDelay = value;
                Parameters = parameters;
            }
        }

        /// <summary>
        /// Delay for right channel, in milliseconds, in the range from DSFXECHO_RIGHTDELAY_MIN
        /// through DSFXECHO_RIGHTDELAY_MAX. The default value is 500 ms.
        /// </summary>
        public float RightDelay
        {
            get
            {
                return Parameters.RightDelay;
            }
            set
            {
                if (value < RightDelayMin || value > RightDelayMax)
                    throw new ArgumentOutOfRangeException("value", "See RightDelayMin and RightDelayMax.");
                var parameters = Parameters;
                parameters.RightDelay = value;
                Parameters = parameters;
            }
        }

        /// <summary>
        /// Value that specifies whether to swap left and right delays with each successive echo.
        /// The default value is zero, meaning no swap. Possible values are defined as
        /// DSFXECHO_PANDELAY_MIN (equivalent to FALSE) and DSFXECHO_PANDELAY_MAX (equivalent to
        /// TRUE).
        /// </summary>
        public int PanDelay
        {
            get
            {
                return Parameters.PanDelay;
            }
            set
            {
                if (value < PanDelayMin || value > PanDelayMax)
                    throw new ArgumentOutOfRangeException("value", "See PanDelayMin and PanDelayMax.");
                var parameters = Parameters;
                parameters.PanDelay = value;
                Parameters = parameters;
            }
        }

        public DSEchoEffect(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// The SetAllParameters method sets the echo parameters of a buffer.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe DSResult SetAllParameters(ref DSFXEcho settings)
        {
            fixed (void* ptr = &settings)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, new IntPtr(ptr), ((void**)(*(void**)_basePtr))[3]);
            }
        }

        /// <summary>
        /// The GetAllParameters method retrieves the echo parameters of a buffer.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe DSResult GetAllParameters(out DSFXEcho settings)
        {
            settings = default(DSFXEcho);
            fixed (void* ptr = &settings)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, new IntPtr(ptr), ((void**)(*(void**)_basePtr))[4]);
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DSFXEcho
    {
        public float WetDryMix;
        public float Feedback;
        public float LeftDelay;
        public float RightDelay;
        public int PanDelay;
    }
}