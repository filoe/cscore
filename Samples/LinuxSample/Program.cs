using System;
using System.Windows.Forms;
using CSCore.SoundOut;
using CSCore.Codecs;

namespace LinuxSample
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			OpenFileDialog openfileDialog = new OpenFileDialog () {
				Filter = CodecFactory.SupportedFilesFilterEn
			};
			if (openfileDialog.ShowDialog () == DialogResult.OK) {
				using (var source = CodecFactory.Instance.GetCodec (openfileDialog.FileName)) {
					using (var soundOut = new ALSoundOut ()) {
						soundOut.Initialize (source);
						soundOut.Play ();
						Console.WriteLine ("Press any key to exit.");
						Console.ReadKey ();
					}
				}
			}
		}
	}
}
