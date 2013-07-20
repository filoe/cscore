using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Pdb;

namespace CSCli
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length >= 1 && File.Exists(args[0]))
            {
                LoadAssembly(args[0]);
            }
            else
            {
                StdOut.Error("Invalid filename.");
                Environment.Exit(-1);
            }
            Environment.Exit(0);
        }

        static void LoadAssembly(string fileName)
        {
            WriterParameters wp = new WriterParameters();
            ReaderParameters rp = new ReaderParameters();

            wp.WriteSymbols = rp.ReadSymbols = File.Exists(Path.ChangeExtension(fileName, "pdb"));
            if (rp.ReadSymbols)
            {
                rp.SymbolReaderProvider = new PdbReaderProvider();
            }

            var assembly = AssemblyDefinition.ReadAssembly(fileName, rp);
            ((BaseAssemblyResolver)assembly.MainModule.AssemblyResolver).AddSearchDirectory(Path.GetDirectoryName(fileName));

            AssemblyPatcher patcher = new AssemblyPatcher(assembly);
            if (patcher.Patch())
            {
                var output = fileName;
                if (File.Exists(output)) File.Delete(output);

                assembly.Write(output, wp);

                StdOut.Info("CSCli patched {" + Path.GetFileName(fileName) + "} successfully");
            }
            else
            {
                StdOut.Error(String.Format("{0} could not be patched.", fileName));
                Environment.Exit(-2);
            }
        }
    }
}
