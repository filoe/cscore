using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace SoundTouchPitchAndTempo
{
    public partial class MainWindow : Window
    {
        private IMainWindowViewModel _mainWindowViewModel;

        public MainWindow()
            : this(new MainWindowViewModel())
        {
        }

        public MainWindow(IMainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();
            DataContext = mainWindowViewModel;
            _mainWindowViewModel = mainWindowViewModel;
        }

        private void PositionSliderMouseDown(object sender, MouseButtonEventArgs e)
        {
            _mainWindowViewModel.PositionSliderMouseDown();
        }

        private void PositionSliderMouseUp(object sender, MouseButtonEventArgs e)
        {
            _mainWindowViewModel.PositionSliderMouseUp();
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            Hide();
            _mainWindowViewModel.Close();
        }
    }
}