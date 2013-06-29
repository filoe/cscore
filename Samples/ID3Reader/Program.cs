using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore.Tags.ID31;

namespace ID3Reader
{
    class Program
    {
        static void Main(string[] args)
        {
            ID3v2 id3 = ID3v2.FromStream(System.IO.File.OpenRead(@"C:\Temp\test.mp3"));
        }
    }
}
