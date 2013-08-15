namespace CSCoreDemo.ViewModel
{
    public class SoundModificationViewModel : ViewModelBase
    {
        public float Volume
        {
            get
            {
                return Main.AudioPlayer.Volume;
            }
            set
            {
                Main.AudioPlayer.Volume = value;
                OnPropertyChanged(() => Volume);
            }
        }

        public float Pan
        {
            get
            {
                return Main.AudioPlayer.Pan;
            }
            set
            {
                Main.AudioPlayer.Pan = value;
                OnPropertyChanged(() => Pan);
            }
        }
    }
}