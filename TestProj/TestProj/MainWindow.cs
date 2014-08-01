using CSCore.Tags.ID3;
using CSCore.Tags.ID3.Frames;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestProj
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TestClick(object sender, EventArgs e)
        {
            OpenFileDialog ofn = new OpenFileDialog();
            if (ofn.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ID3v2 id3 = ID3v2.FromStream(ofn.OpenFile());
                if (id3 != null)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendFormat("Title: {0}\n", id3.QuickInfo.Title);
                    builder.AppendFormat("Artist: {0}\n", id3.QuickInfo.Artist);
                    builder.AppendFormat("LeadPerformers: {0}\n", id3.QuickInfo.LeadPerformers);
                    builder.AppendFormat("Album: {0}\n", id3.QuickInfo.Album);
                    builder.AppendFormat("Year: {0}\n", id3.QuickInfo.Year);

                    picturebox1.Image = id3.QuickInfo.Image;
                    lblInfo.Text = builder.ToString();
                }
                else
                {
                    MessageBox.Show("Kein Tag vorhanden");
                }
            }
        }
    }
}