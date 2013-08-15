using CSCore;
using CSCore.Visualization;

namespace CSCoreDemo.ViewModel
{
    public class VisualizationViewModel : ViewModelBase
    {
        private FFTDataProvider _fftDataProvider;

        public FFTDataProvider FFTDataProvider
        {
            get { return _fftDataProvider; }
            set { SetProperty(value, ref _fftDataProvider, () => FFTDataProvider); }
        }

        private SampleDataProvider _sampleDataProvider;

        public SampleDataProvider SampleDataProvider
        {
            get { return _sampleDataProvider; }
            set { SetProperty(value, ref _sampleDataProvider, () => SampleDataProvider); }
        }

        public IWaveSource InitializeVisualization(IWaveSource source)
        {
            source = new FFTDataProvider(source) { Bands = 512 };
            FFTDataProvider = source as FFTDataProvider;

            var sampleDataProvier = new SampleDataProvider(source);
            sampleDataProvier.Mode = SampleDataProviderMode.LeftAndRight;
            SampleDataProvider = sampleDataProvier;

            return sampleDataProvier.ToWaveSource(16);
        }
    }
}