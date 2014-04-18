using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSCore.Streams.Effects;
using System.Reflection;

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

            var types = GetEffectTypes();
            foreach (var type in types)
            {
                Console.WriteLine(type.FullName);

                using (var effect = CreateEffect(type, GetSource()))
                {
                    var properties = type.GetProperties(BindingFlags.SetProperty | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance).Where(x => x.DeclaringType == type);
                    foreach (var property in properties)
                    {
                        try
                        {
                            var value = property.GetValue(effect, null);
                            property.SetValue(effect, value, null);
                            Console.WriteLine(" - SUCCESSFUL: {0}.", property.Name);
                        }
                        catch(Exception)
                        {
                            Console.WriteLine(" - FAILED: {0}.", property.Name);
                            flag = false;
                        }
                    }
                }
            }

            Console.WriteLine("=================================");
            Console.WriteLine(flag ? "Successful!" : "Failed!");

            if(!flag)
                Assert.Fail();
        }

        private Type[] GetEffectTypes()
        {
            return new Type[]
            {
                typeof(DmoChorusEffect),
                typeof(DmoCompressorEffect),
                typeof(DmoDistortionEffect),
                typeof(DmoEchoEffect),
                typeof(DmoFlangerEffect),
                typeof(DmoGargleEffect),
                typeof(DmoWavesReverbEffect)
            };
        }

        private IWaveSource GetSource()
        {
            return new CSCore.Streams.SineGenerator().ToWaveSource();
        }

        private IWaveSource CreateEffect(Type effectType, IWaveSource source)
        {
            return (IWaveSource)Activator.CreateInstance(effectType, source);
        }
    }
}
