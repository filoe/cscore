using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFVisualisation
{
    /// <summary>
    /// Interaktionslogik für StreamURLSelector.xaml
    /// </summary>
    public partial class StreamURLSelector : Window
    {
        private const string oe3 = @"http://mp3stream7.apasf.apa.at:8000";
        private const string test = @"http://streamplus5.leonex.de:10686/;Lippstadt-FM.mp3";

        public StreamURLSelector()
        {
            InitializeComponent();
            DataContext = this;

            Value = oe3;
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(StreamURLSelector), new PropertyMetadata("Enter a URL: "));

        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(StreamURLSelector), new PropertyMetadata(oe3));

        private void OnOKClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (DialogResult == null)
                e.Cancel = true;
        }
    }
}