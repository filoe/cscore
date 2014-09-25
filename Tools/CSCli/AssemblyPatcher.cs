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
        private AssemblyDefinition _assembly;
        private string _calliAttributeName;
        private string _removeTypeAttributeName;
        private List<TypeDefinition> _typesToRemove;

        private int _replacedCallsCount;

        public AssemblyPatcher(AssemblyDefinition assembly, string calliAttributeName, string removeTypeAttributeName)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");
            if (String.IsNullOrEmpty(calliAttributeName))
                throw new ArgumentNullException("calliAttributeName");
            if (String.IsNullOrEmpty(removeTypeAttributeName))
                throw new ArgumentNullException("removeTypeAttributeName");

            _assembly = assembly;
            _calliAttributeName = calliAttributeName;
            _removeTypeAttributeName = removeTypeAttributeName;
            _typesToRemove = new List<TypeDefinition>();
        }

        public bool PatchAssembly()
        {
            try
            {
                MessageIntegration.Info("CSCli is searching for calls to replace. Assemblyname: " + _assembly.FullName);

                var module = _assembly.MainModule;
                foreach (var type in module.Types)
                {
                    ProcessType(type);
                }

                MessageIntegration.Info(String.Format("CSCli processed {0} types.", module.Types.Count));
                MessageIntegration.Info(String.Format("CSCli replaced {0} method-calls by Calli-instruction.", _replacedCallsCount));
                MessageIntegration.Info(String.Format("Cleaning up assembly. {0} types to remove from assembly.", _typesToRemove.Count));

                foreach (var typeToRemove in _typesToRemove)
                {
                    module.Types.Remove(typeToRemove);
                }
            }
            catch (Exception ex)
            {
                MessageIntegration.WriteError("Unknown error: " + ex.ToString());
                return false;
            }

            return true;
        }

        private void ProcessType(TypeDefinition type)
        {
            foreach (var nestedType in type.NestedTypes)
            {
                ProcessType(nestedType);
            }

            List<MethodDefinition> methods = new List<MethodDefinition>(type.Methods);
            foreach (var property in type.Properties)
            {
                if (property.GetMethod != null)
                {
                    methods.Add(property.GetMethod);
                }
                if (property.SetMethod != null)
                {
                    methods.Add(property.SetMethod);
                }
            }

            foreach (var method in type.Methods)
            {
                methods.Add(method);
            }

            foreach (var method in methods)
            {
                ProcessMethod(method);
            }

            var attributes = GetAttributes(type);
            if (attributes.Contains(_removeTypeAttributeName))
            {
                AddTypeToRemove(type);
            }
        }

        private void ProcessMethod(MethodDefinition method)
        {
            if (method.HasBody)
            {
                ILProcessor ilProcessor = method.Body.GetILProcessor();

                //process every instruction of the methods body
                for (int n = 0; n < ilProcessor.Body.Instructions.Count; n++) //foreach won't work because iterator length is bigger than count??
                {
                    var instruction = ilProcessor.Body.Instructions[n];

                    //check whether the instruction is a call to a method
                    if (instruction.OpCode.Code == Code.Call && instruction.Operand is MethodReference)
                    {
                        //get the called method
                        MethodReference methodDescription = (MethodReference)instruction.Operand;
                        //get the attributes of the called method
                        var attributes = GetAttributes(methodDescription.Resolve());

                        //check whether the called method is marked with the given calli attribute
                        if (attributes.Contains(_calliAttributeName))
                        {
                            //MessageIntegration.Info("Patching [{0}] @ [{1}].", method.ToString(), instruction.ToString());
                            if (!method.Name.EndsWith("Native"))
                                MessageIntegration.Info(String.Format("CallI-Comimport Methodnames should end with a \"Native\". Method: \"{0}\".", method.FullName));

                            //create a callsite for the calli instruction using stdcall calling convention
                            var callSite = new CallSite(methodDescription.ReturnType)
                            {
                                CallingConvention = MethodCallingConvention.StdCall
                            };

                            //iterate through every parameter of the original method-call
                            for (int j = 0; j < methodDescription.Parameters.Count - 1; j++) //foreach won't work because iterator length is bigger than count??
                            {
                                var p = methodDescription.Parameters[j];
                                if (p.ParameterType.FullName == "System.Boolean")
                                {
                                    MessageIntegration.WriteWarning("Native bool has a size of 4 bytes. Make sure to use a 16bit Integer for \"VARIANT_BOOL\" and a 32bit Integer for \"BOOL\".", "CI0001");
                                }

                                //append every parameter of the method-call to the callsite of the calli instruction
                                callSite.Parameters.Add(p);
                            }

                            //create a calli-instruction including the just built callSite
                            var calliInstruction = ilProcessor.Create(OpCodes.Calli, callSite);
                            //replace the method-call by the calli-instruction
                            ilProcessor.Replace(instruction, calliInstruction);

                            _replacedCallsCount++;
                        }
                    }
                }
            }
        }

        private IEnumerable<string> GetAttributes(ICustomAttributeProvider attributeProvider)
        {
            if (attributeProvider == null || !attributeProvider.HasCustomAttributes)
            {
                yield break;
            }

            foreach (var attribute in attributeProvider.CustomAttributes)
            {
                yield return attribute.AttributeType.Name;
            }
        }

        private void AddTypeToRemove(TypeDefinition type)
        {
            var attributes = GetAttributes(type);
            if (!attributes.Contains(_removeTypeAttributeName))
            {
                string error = String.Format("Can't remove type \"{0}\", because it is not marked with \"{1}\" attribute.",
                    type.FullName, _removeTypeAttributeName);
                MessageIntegration.WriteError(error);
                throw new Exception(error);
            }

            if (!_typesToRemove.Contains(type))
            {
                _typesToRemove.Add(type);
            }

            foreach (var nestedType in type.NestedTypes)
            {
                AddTypeToRemove(nestedType);
            }
        }
    }
}