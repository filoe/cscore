using System;
using CSCore;
using CSCore.SoundOut;
using CSCore.Codecs.WAV;
using System.Threading;
using System.Linq;

namespace OpenALSample
{
	class MainClass
	{
		static ISoundOut _soundOut;
		static IWaveSource _waveSource;
		public static void Main (string[] args)
		{
			string path = "";
			if (args.Length > 0) path = args [0];
			else
			{
				Console.WriteLine ("Enter the wave file to play:");
				path = Console.ReadLine();
			}

			_waveSource = new WaveFileReader (path);
			_soundOut = new ALSoundOut ();
			_soundOut.Initialize (_waveSource);

			_soundOut.Play ();

			while (true)
			{
				if (Console.KeyAvailable)
				{
					var key = Console.ReadKey ();
					if (key.Key == ConsoleKey.Escape)
						break;
				}

				var str = String.Format (@"{0:mm\:ss\.f}/{1:mm\:ss\.f}",
					          TimeConverterFactory.Instance.GetTimeConverterForSource (_waveSource)
					.ToTimeSpan (_waveSource.WaveFormat, _waveSource.Position),
					          TimeConverterFactory.Instance.GetTimeConverterForSource (_waveSource)
					.ToTimeSpan (_waveSource.WaveFormat, _waveSource.Length));
				str += String.Concat (Enumerable.Repeat (" ", Console.BufferWidth - 1 - str.Length));
				Console.SetCursorPosition (0, Console.CursorTop);
				Console.Write (str);
				Thread.Sleep (100);
			}
			_soundOut.Stop ();
			_soundOut.Dispose ();
			_waveSource.Dispose ();
			CSCore.SoundOut.AL.ALDevice.DefaultDevice.Dispose ();
		}
	}
}
