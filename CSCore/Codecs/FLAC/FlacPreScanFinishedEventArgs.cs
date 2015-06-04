using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CSCore.Codecs.FLAC
{
    /// <summary>
    /// Provides data for a FlacPreScan.
    /// </summary>
    public class FlacPreScanFinishedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the a list of found frames by the scan.
        /// </summary>
        public ReadOnlyCollection<FlacFrameInformation> Frames { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FlacPreScanFinishedEventArgs"/> class.
        /// </summary>
        /// <param name="frames">Found frames.</param>
        public FlacPreScanFinishedEventArgs(List<FlacFrameInformation> frames)
        {
            Frames = frames.AsReadOnly();
        }
    }
}