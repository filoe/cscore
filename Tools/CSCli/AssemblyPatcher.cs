#define handleException

using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Pdb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CSCli
{
    public class AssemblyPatcher
    {
        private AssemblyDefinition _asm;
        private List<TypeDefinition> _objectsToRemove;

        public AssemblyPatcher(AssemblyDefinition asm)
        {
            _asm = asm;
            _objectsToRemove = new List<TypeDefinition>();
        }

        public bool Patch()
        {
#if handleException
            try
            {
#endif
                StdOut.Info("CSCli is searching for calls to replace.");
                var assembly = _asm;

                foreach (var type in assembly.MainModule.Types)
                {
                    ProcessType(type);
                }

                StdOut.Info("Processed {0} types.", assembly.MainModule.Types.Count);
                StdOut.Info("Replaced {0} method-calls", ReplacedCalls);
                StdOut.Info("Cleaning up assembly. {0} types to remove.", _objectsToRemove.Count);

                _objectsToRemove.ForEach((t) => assembly.MainModule.Types.Remove(t));
#if handleException
            }
            catch (Exception e)
            {
                StdOut.Error(e.ToString());
                return false;
            }
#endif
            return true;
        }

        private const string CSCalli = "CSCalliAttribute";
        private const string RemoveObj = "RemoveObjAttribute";

        private int replacedCalls = 0;

        public int ReplacedCalls
        {
            get { return replacedCalls; }
        }

        private void ProcessType(TypeDefinition type)
        {
            if (GetAttributes(type).Contains(RemoveObj))
            {
                _objectsToRemove.Add(type);
            }

            foreach (var method in type.Methods)
            {
                ProcessMethod(method);
            }
        }

        private void ProcessMethod(MethodDefinition method)
        {
            if (method.HasBody)
            {
                ILProcessor ilProcessor = method.Body.GetILProcessor();

                for (int i = 0; i < method.Body.Instructions.Count; i++)
                {
                    var instruction = method.Body.Instructions[i];
                    if (instruction.OpCode.Code == Code.Call && instruction.Operand is MethodReference)
                    {
                        MethodReference methodDescription = (MethodReference)instruction.Operand;
                        var attributes = GetAttributes(methodDescription.Resolve());

                        if (attributes.Contains(CSCalli))
                        {
                            StdOut.Info("Patching [{0}].", methodDescription.FullName);
                            if (!methodDescription.FullName.EndsWith("Native"))
                                StdOut.Instance.WriteInfo("CallI-Comimport Methodnames should end with a \"Native\"");

                            var callSite = new CallSite(methodDescription.ReturnType) { CallingConvention = MethodCallingConvention.StdCall };
                            for (int n = 0; n < methodDescription.Parameters.Count - 1; n++)
                            {
                                var p = methodDescription.Parameters[n];
                                if (p.ParameterType.FullName == "System.Boolean")
                                    StdOut.Error(methodDescription.FullName + ": Don't use System.Boolean in a CalllI-signature. Use any type which has a size of 32bit instead(see System.Runtime.InteropServices.NativeBool contained by CSCore.dll).");
                                callSite.Parameters.Add(p);
                            }

                            var callInstruction = ilProcessor.Create(OpCodes.Calli, callSite);
                            ilProcessor.Replace(instruction, callInstruction);

                            replacedCalls++;
                        }
                    }
                }
            }
        }

        private List<string> GetAttributes(ICustomAttributeProvider method)
        {
            List<string> attributes = new List<string>();
            if (method == null) return attributes;
            foreach (var attribute in method.CustomAttributes)
            {
                attributes.Add(attribute.AttributeType.Name);
            }
            return attributes;
        }
    }
}