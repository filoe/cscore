using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CSCore;
using CSCore.Ffmpeg;
using CSCore.SoundOut;

namespace FfmpegSample
{
    class Program
    {
        static void Main(string[] args)
        {
            const string filename = @"D:\Temp\ffmpegtest\mp3.mp3";
            File.Delete("out.wav");
            var source = new FfmpegDecoder(filename);
            source.WriteToFile("out.wav");
        }
    }
}
