#define handleException
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Pdb;
using System.IO;

namespace CSCli
{
    public class AssemblyPatcher
    {
        AssemblyDefinition _asm;
        List<TypeDefinition> _objectsToRemove;
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
                var assembly = _asm;

                foreach (var type in assembly.MainModule.Types)
                {
                    ProcessType(type);
                }

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

        const string CSCli = "CSCliAttribute";
        const string RemoveObj = "RemoveObjAttribute";

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

                        if (attributes.Contains(CSCli))
                        {
                            var callSite = new CallSite(methodDescription.ReturnType) { CallingConvention = MethodCallingConvention.StdCall };
                            for (int n = 0; n < methodDescription.Parameters.Count - 1; n++)
                            {
                                callSite.Parameters.Add(methodDescription.Parameters[n]);
                            }

                            var callInstruction = ilProcessor.Create(OpCodes.Calli, callSite);
                            ilProcessor.Replace(instruction, callInstruction);
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
