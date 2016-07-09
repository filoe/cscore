using System;
using System.IO;
using CSCore;
using CSCore.Ffmpeg;

namespace FfmpegSample
{
    class Program
    {
        static void Main(string[] args)
        {
            const string filename = @"D:\Temp\ffmpegtest\mp3.mp3";
            File.Delete("out.wav");
            var source = new FfmpegDecoder(new Uri(filename));
            source.WriteToFile("out.wav");
        }
    }
}
