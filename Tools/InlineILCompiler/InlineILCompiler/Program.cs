using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Diagnostics;

namespace InlineILCompiler
{
    class Program
    {
        static bool Run(string cmd, string args)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo(cmd, args);
            processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            processStartInfo.UseShellExecute = true;
            //processStartInfo.RedirectStandardOutput = true;
            Process process = Process.Start(processStartInfo);
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                Console.WriteLine(string.Format("ERROR: Process '{0}' failed with exit code '{1}", cmd, process.ExitCode));
                return false;
            }

            return true;
        }
        static string[] GetSourceFilesFromProject(string proj)
        {
            var result = new List<string>();
            string proj_dir = Path.GetDirectoryName(proj);
            var rdr = new XmlTextReader(proj);
            while (rdr.Read())
            {
                if (rdr.NodeType == XmlNodeType.Element && rdr.Name == "Compile")
                {
                    while (rdr.MoveToNextAttribute())
                    {
                        if (rdr.Name == "Include")
                        {
                            result.Add(Path.Combine(proj_dir, rdr.Value));
                        }
                    }
                }
            }
            return result.ToArray();
        }
        static bool PreprocessCode(string[] source, out List<Tuple<string, string, int>> exports, out Dictionary<string, List<CodeSegment>> segments)
        {
            segments = new Dictionary<string, List<CodeSegment>>();
            exports = new List<Tuple<string, string, int>>();

            foreach (var _src in source)
            {
                string src = _src.ToLower();
                string[] contents = File.ReadAllLines(src);
                for (int i = 0; i < contents.Length; i++)
                {
                    string trimmed = contents[i].Trim();
                    if (trimmed == "#if IL" || trimmed == "#If IL Then") // found an IL segment...
                    {
                        CodeSegment seg = new CodeSegment();
                        seg.Start = ++i;
                        do
                        {
                            trimmed = contents[i].TrimStart();
                            if (trimmed.StartsWith(".exportnative ", StringComparison.OrdinalIgnoreCase))
                            {
                                // .exportnative (custom directive for exporting functions)
                                string exportName = trimmed.Remove(0, 14);
                                if (exports.Find((x) => x.Item1 == exportName) != null)
                                {
                                    Console.WriteLine("ERROR: Found duplicate-name export entries for \"" + exportName + "\"");
                                    return false; // failed
                                }
                                exports.Add(Tuple.Create(exportName, src, i));
                            }
                            seg.ILCode.Add(contents[i++]);
                            trimmed = contents[i].TrimStart();

                        } while (trimmed != "#endif" && trimmed != "#End If"); // end of IL statement

                        seg.End = i;
                        seg.SrcName = Path.GetFileName(src).ToLower(); // not case sensitive

                        List<CodeSegment> segCollection;
                        if (!segments.TryGetValue(src, out segCollection))
                            segments.Add(src, (segCollection = new List<CodeSegment>()));

                        Console.WriteLine("Found IL {0} from line {1} to line {2}", seg.SrcName, seg.Start + 1, seg.End);
                        segCollection.Add(seg);
                    }
                }
            }
            return true; // success
        }
        static bool PreprocessIL(List<string> ilCode, List<Tuple<string, string, int>> exports, Dictionary<string, List<CodeSegment>> segments)
        {
            int nExports = 1;
            int pnLine = -1; // previous nLine
            string csf = null; // current source file
            for (int i = 0; i < ilCode.Count; i++)
            {
                string line = ilCode[i].Trim();
                if (line.StartsWith(".maxstack"))
                {
                    // http://blogs.msdn.com/b/jmstall/archive/2005/07/20/ilasm-maxstack.aspx?Redirected=true
                    //
                    // apparently the default is .maxstack 8 if you remove the decl. and it's not recomputed
                    //
                    int nStack = int.Parse(line.Remove(0, 10));
                    ilCode[i] = ".maxstack " + Math.Max(nStack, 8);
                }
                else if (line.StartsWith(".corflags")) // change the corflags 
                {
                    if (exports.Count > 0)
                    {
                        // add support to the vtable for our native exports
                        // there's probably a better way to do this, I'm just copying how Delphi.NET does it...

                        string[] vtfixups = new string[2];
                        vtfixups[0] = string.Format(".vtfixup [{0}] int32 fromunmanaged at VT0", exports.Count);
                        vtfixups[1] = ".data VT0 = bytearray(";
                        for (int p = 0; p < exports.Count; p++)
                            vtfixups[1] = vtfixups[1] + " 00 00 00 00";
                        vtfixups[1] = vtfixups[1] + " )";

                        ilCode[i] = ".corflags 0x00000002"; // adjust .corflags to what Delphi.NET sets it too when you export
                        ilCode.InsertRange(i + 1, vtfixups);
                    }
                }
                else if (line.StartsWith(".line"))
                {
                    bool newFile = false;
                    // try to detect if there is a new source file we are now processing...
                    if (line.Count((x) => x == '\'') >= 2)
                    {
                        int start = line.IndexOf('\'') + 1;
                        string tcsf = line.Substring(start, line.LastIndexOf('\'') - start);
                        while (tcsf.Contains(@"\\"))
                            tcsf = tcsf.Replace(@"\\", @"\");

                        if (tcsf != string.Empty) // new source file?
                            csf = tcsf.ToLower();

                        if(csf == @"D:\vsproj\CSCore\source\CSCore\CSCore\Utils\ILUtils.cs".ToLower())
                            Debugger.Break();
                        newFile = true;
                    }

                    // this is the line number the below IL is relative to in the source...
                    int nLine = int.Parse(line.Remove(0, 6).Substring(0, line.IndexOf(',') - 6));
                    if (newFile)
                        pnLine = 1;
                    List<CodeSegment> cs;
                    if (segments.TryGetValue(csf, out cs))
                    {
                        foreach (var codeSegment in cs)
                        {
                            // try to find a code segment that goes before this line
                            // which has yet to be inserted into our IL code
                            if (nLine > -1 && !codeSegment.Inserted && pnLine <= codeSegment.Start && nLine >= codeSegment.Start)
                            {
                                codeSegment.Inserted = true;

                                Console.WriteLine("Inserted segment from line {0} to {1}.", codeSegment.Start, codeSegment.End);

                                for (int k = 0; k < codeSegment.ILCode.Count; k++)
                                {
                                    string trimmedIl = codeSegment.ILCode[k].Trim();
                                    if (trimmedIl.StartsWith(".maxstack "))
                                    {
                                        for (int j = i - 1; j >= 0; j--) // search for .maxstack decleration
                                        {
                                            string trimmedIL2 = ilCode[j].TrimStart();
                                            if (trimmedIL2.StartsWith(".maxstack"))
                                            {
                                                ilCode[j] = trimmedIl; // replace default .maxstack with user's
                                                break;
                                            }
                                        }
                                    }
                                    else if (trimmedIl.StartsWith(".exportnative "))
                                    {
                                        // remove the ".exportnative " bit
                                        // this should now be the function name
                                        trimmedIl = trimmedIl.Remove(0, 14);

                                        bool replaced = false;
                                        for (int j = i - 1; j >= 0; j--) // search for method signature
                                        {
                                            string trimmedIl2 = ilCode[j].TrimStart();
                                            if (trimmedIl2.StartsWith(".method ")) // found the method we're exporting...
                                            {
                                                // update the method declaration to be stdcall (this is what Delphi.NET does)
                                                int nIndex = trimmedIl2.IndexOf(exports[nExports - 1].Item1 + "(");
                                                if (nIndex < 0)
                                                {
                                                    Console.WriteLine("Export name \"{0}\" does not match function name", exports[nExports - 1].Item1);
                                                    return false;
                                                }
                                                trimmedIl2 = trimmedIl2.Insert(nIndex, "modopt([mscorlib]System.Runtime.CompilerServices.CallConvStdcall) ");
                                                ilCode[j] = trimmedIl2;

                                                // add the nessecary information about the vtentry and the export name
                                                for (; j < ilCode.Count; j++)
                                                {
                                                    if (ilCode[j].Trim().StartsWith("{"))
                                                    {
                                                        ilCode.Insert(j + 1, string.Format(".vtentry 1 : {0}", nExports));
                                                        ilCode.Insert(j + 2, string.Format(".export [{0}] as {1}", nExports, trimmedIl));
                                                        i += 2; // align i to match our changes
                                                        replaced = true;

                                                        break;
                                                    }
                                                }

                                                break;
                                            }
                                        }

                                        if (!replaced) // was the function not updated? then we must've failed to update it... (strange)
                                        {
                                            Console.WriteLine("ERROR: Failed to sign modopt for exported function \"" + trimmedIl + "\"");
                                            return false;
                                        }

                                        nExports += 1;
                                    }
                                    else
                                    {
                                        // patch in our IL code and add a .line directive to allow it to be debugged ;)
                                        if (!trimmedIl.StartsWith(".maxstack"))
                                        {
                                            int start = codeSegment.ILCode[k].TakeWhile((c) => c == ' ').Count();
                                            int trim = codeSegment.ILCode[k].Reverse().TakeWhile((c) => c == ' ').Count();
                                            ilCode.Insert(i++, string.Format(".line {0},{0} : {1},{2} ''", codeSegment.Start + k + 1, 1 + start, codeSegment.ILCode[k].Length + 1 - trim));
                                        }
                                        ilCode.Insert(i++, codeSegment.ILCode[k]);
                                    }
                                }
                                break;
                            }
                        }
                    }

                    pnLine = nLine;
                }
            }
            return true;
        }

        static bool VerifySegments(Dictionary<string, List<CodeSegment>> segments)
        {
            foreach (var kvp in segments)
            {
                foreach (var seg in kvp.Value)
                {
                    if (!seg.Inserted)
                    {
                        Console.WriteLine("ERROR: Failed to insert IL {0} from line {1} to line {2}", seg.SrcName, seg.Start + 1, seg.End);
                        return false;
                    }
                }
            }
            return true;
        }

        static int Main(string[] args)
        {
            /*string Target = @"D:\vsproj\CSCore\source\CSCore\CSCore\bin\Release\CSCore.dll";//Environment.GetEnvironmentVariable("target");
            string SDKDirectory = @"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.1A\bin\NETFX 4.5.1 Tools";//Environment.GetEnvironmentVariable("sdk");
            string ProjectPath = @"D:\vsproj\CSCore\source\CSCore\CSCore\CSCore.csproj"; //Environment.GetEnvironmentVariable("project");
            string FrameworkDirectory = @"C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727";// Environment.GetEnvironmentVariable("framework");
            string UserILAsmArguments = "/DLL";//Environment.GetEnvironmentVariable("ilasm_args");*/

            string Target = /*@"C:\vsproj\CSCore\source\CSCore\CSCore\bin\Debug\CSCore.dll";*/Environment.GetEnvironmentVariable("target");
            string SDKDirectory = /*@"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.1A\bin\NETFX 4.5.1 Tools";*/Environment.GetEnvironmentVariable("sdk");
            string ProjectPath = /*@"C:\vsproj\CSCore\source\CSCore\CSCore\CSCore.csproj";*/ Environment.GetEnvironmentVariable("project");
            string FrameworkDirectory = /*@"C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727";*/ Environment.GetEnvironmentVariable("framework");
            string UserILAsmArguments = /*"/DLL";*/Environment.GetEnvironmentVariable("ilasm_args");

            Console.WriteLine("Target: {0}\nSDKDir: {1}\nProjectPath: {2}\nFrameworkDir: {3}\nILArgs: {4}",
                Target, SDKDirectory, ProjectPath, FrameworkDirectory, UserILAsmArguments);

            string working = Environment.CurrentDirectory;
            Dictionary<string, List<CodeSegment>> segments; // <filename, segments>            
            List<Tuple<string, string, int>> exports; // <export_name, file, line_index>

            //
            // re-direct the console
            //
            //var writer = new StreamWriter(new FileStream(working + @"\InlineILCompiler.log", FileMode.Create));
            //Console.SetOut(writer);

            try
            {
                //
                // search for inline IL and store them in segments, also find native exports
                //
                Console.WriteLine("Searching for inline IL ...");
                var sources = GetSourceFilesFromProject(ProjectPath);
                if (!PreprocessCode(sources, out exports, out segments))
                    return 0;
                Console.WriteLine("... Done searching for inline IL.");
                Console.WriteLine();

                Console.WriteLine("Found {0} native export(s).", exports.Count);
                Console.WriteLine();

                if (segments.Count > 0)
                {
                    //
                    // disassemble code
                    //
                    Console.WriteLine("Disassembling compiled executable... ");
                    string disasmFile = Path.Combine(working, "disasm.il");
                    string ildasmArguments = string.Format("\"{0}\" /linenum /out=\"{1}\"", Target, disasmFile);
                    Console.WriteLine("ildasm " + ildasmArguments);
                    if (!Run(Path.Combine(SDKDirectory, "", "ildasm.exe"), ildasmArguments))
                        return 4;
                    Console.WriteLine("... Done disassembling.");
                    Console.WriteLine();

                    //
                    // begin pre-processing the IL
                    //
                    Console.WriteLine("Attempting to insert inline IL code...");
                    List<string> ilCode = new List<string>();
                    ilCode.AddRange(File.ReadAllLines(disasmFile));

                    if (!PreprocessIL(ilCode, exports, segments))
                        return 3;
                    if (!VerifySegments(segments)) // ensure all segments were used...
                        return 2;
                    File.WriteAllLines(disasmFile, ilCode.ToArray());
                    Console.WriteLine("... Done inserting IL code.");
                    Console.WriteLine();

                    //
                    // reassemble code
                    //
                    Console.WriteLine("Attempting to assemble IL code...");
                    string ilasm_args = string.Format("\"{0}\" {2} /output=\"{1}\"", disasmFile, Target, UserILAsmArguments);
                    Console.WriteLine("ilasm " + ilasm_args);
                    if (!Run(Path.Combine(FrameworkDirectory, "ilasm.exe"), ilasm_args))
                        return 1;
                    Console.WriteLine();
                    Console.WriteLine("... Done assembling.");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("No inline IL found.");
                    Console.WriteLine();
                }
            }
            finally
            {
                var files = new[] {Path.Combine(working, "disasm.il"), Path.Combine(working, "disasm.res")};
                foreach (var file in files)
                {
                    try
                    {
                        if (File.Exists(file))
                            File.Delete(file);
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine("Failed to delete temp file: " + ex.Message);
                    }
                }
                //writer.Close();
            }

            return 0;
        }
    }

    public class CodeSegment
    {
        public List<string> ILCode;
        public int Start;
        public int End;
        public bool Inserted { get; set; }
        public string SrcName;

        public CodeSegment()
        {
            ILCode = new List<string>();
        }
    }
}
