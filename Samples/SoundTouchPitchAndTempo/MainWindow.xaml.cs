using System.Windows;

namespace SoundTouchPitchAndTempo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
            : this(new MainWindowViewModel())
        {
        }

        public MainWindow(IMainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();
            DataContext = mainWindowViewModel;
        }
    }
}