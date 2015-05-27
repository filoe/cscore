using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CSCore.Streams;
using CSCore.Streams.Effects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSCore.Test.DMO
{
    [TestClass]
    public class DmoEffectsTests
    {
        [TestMethod]
        [TestCategory("DMO")]
        public void CanSetDmoEffectParameters()
        {
            bool flag = true;

            Type[] types = GetEffectTypes();
            foreach (Type type in types)
            {
                Console.WriteLine(type.FullName);

                using (IWaveSource effect = CreateEffect(type, GetSource()))
                {
                    IEnumerable<PropertyInfo> properties =
                        type.GetProperties(BindingFlags.SetProperty | BindingFlags.GetProperty | BindingFlags.Public |
                                           BindingFlags.Instance).Where(x => x.DeclaringType == type);
                    foreach (PropertyInfo property in properties)
                    {
                        try
                        {
                            object value = property.GetValue(effect, null);
                            property.SetValue(effect, value, null);
                            Console.WriteLine(" - SUCCESSFUL: {0}.", property.Name);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(" - FAILED: {0}. -> {1}", property.Name, ex);
                            flag = false;
                        }
                    }
                }
            }

            Console.WriteLine("=================================");
            Console.WriteLine(flag ? "Successful!" : "Failed!");

            if (!flag)
                Assert.Fail();
        }

        private Type[] GetEffectTypes()
        {
            return new[]
            {
                typeof (DmoChorusEffect),
                typeof (DmoCompressorEffect),
                typeof (DmoDistortionEffect),
                typeof (DmoEchoEffect),
                typeof (DmoFlangerEffect),
                typeof (DmoGargleEffect),
                typeof (DmoWavesReverbEffect)
            };
        }

        private IWaveSource GetSource()
        {
            return new SineGenerator().ToWaveSource();
        }

        private IWaveSource CreateEffect(Type effectType, IWaveSource source)
        {
            return (IWaveSource) Activator.CreateInstance(effectType, source);
        }
    }
}