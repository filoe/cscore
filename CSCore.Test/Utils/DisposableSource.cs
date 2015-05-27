using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Test.Utils
{
    internal class DisposableSource : WaveAggregatorBase
    {
        public bool IsDisposed { get; private set; }

        public DisposableSource(IWaveSource source)
            : base(source)
        {
            IsDisposed = false;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            IsDisposed = true;
        }
    }
}
