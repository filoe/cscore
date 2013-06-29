using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSCore.CoreAudioAPI;
using System.Diagnostics;

namespace CSCore.Test
{
    [TestClass]
    public class CoreAudioAPI
    {
        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void EnumerateDevice()
        {
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            var collection = enumerator.EnumAudioEndpoints(DataFlow.All, DeviceState.All);
            foreach (var item in collection)
            {
                Debug.WriteLine(item.ToString());

                item.Dispose();
            }

            enumerator.Dispose();
            collection.Dispose();
        }

        [TestMethod]
        [TestCategory("CoreAudioAPI")]
        public void DevicePropertyDump()
        {
            var enumerator = new MMDeviceEnumerator();
            var coll = enumerator.EnumAudioEndpoints(DataFlow.All, DeviceState.All);

            foreach (var dev in coll)
            {
                Console.WriteLine("Name: {0}", dev.PropertyStore[PropertyStore.FriendlyName]);
                Console.WriteLine("Desc: {0}", dev.PropertyStore[PropertyStore.DeviceDesc]);
                Console.WriteLine("-----------------------------------------------");
                foreach (var item in dev.PropertyStore)
                {
                    try
                    {
                        Console.WriteLine("Key: {0}\nValue: {1}\n\n", item.Key.PropertyID, item.Value.GetValue());
                    }
                    catch (Exception)
                    {
                    }
                }

                dev.Dispose();
            }

            enumerator.Dispose();
            coll.Dispose();
        }
    }
}
