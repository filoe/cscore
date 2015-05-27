using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CSCore.MediaFoundation;
using CSCore.Win32;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSCore.Test.MediaFoundation
{
    [TestClass]
    public class MFAttributesTest
    {
        [TestMethod]
        public void CanCreateEmptyMFAttributes()
        {
            using (var instance = new MFAttributes(0))
            {
                Assert.IsNotNull(instance);
                Assert.AreNotSame(IntPtr.Zero, instance.BasePtr);
            }
        }

        [TestMethod]
        public void CanSetAndGetValues()
        {
            var random = new Random();

            using (var instance = new MFAttributes(0))
            {
                Guid guid = Guid.NewGuid();
                instance.SetDouble(guid, 10d);
                Assert.AreEqual(10d, instance.GetDouble(guid));

                guid = Guid.NewGuid();
                instance.SetGuid(guid, guid);
                Assert.AreEqual(guid, instance.GetGuid(guid));

                guid = Guid.NewGuid();
                instance.SetString(guid, "Hello World");
                Assert.AreEqual("Hello World", instance.GetString(guid));

                guid = Guid.NewGuid();
                instance.SetUINT32(guid, 10);
                Assert.AreEqual(10, instance.GetUINT32(guid));

                guid = Guid.NewGuid();
                instance.SetUINT64(guid, 10L);
                Assert.AreEqual(10L, instance.GetUINT64(guid));

                guid = Guid.NewGuid();
                var value = new PropertyVariant()
                {
                    DataType = VarEnum.VT_UI4,
                    UIntValue = 10
                };
                instance.SetItem(guid, value);
                Assert.AreEqual(value, instance.GetItem(guid));

                guid = Guid.NewGuid();
                byte[] inputBlob = new byte[100];
                random.NextBytes(inputBlob);
                instance.Set(guid, inputBlob);
                CollectionAssert.AreEqual(inputBlob, instance.Get<byte[]>(guid));

                guid = Guid.NewGuid();
                TestStruct testStructValue = new TestStruct()
                {
                    Field0 = Guid.NewGuid(),
                    Field1 = random.Next(),
                    Field2 = random.NextDouble() * random.Next(),
                };
                instance.Set(guid, testStructValue);
                Assert.AreEqual(testStructValue, instance.Get<TestStruct>(guid));
            }
        }

        [TestMethod]
        public void CanDeleteItem()
        {
            double val;

            using (var instance = new MFAttributes(0))
            {
                Guid guid = Guid.NewGuid();
                instance.SetDouble(guid, 100);
                Assert.AreEqual(100, instance.GetDouble(guid));

                instance.DeleteItem(guid);
                Assert.IsFalse(instance.TryGet(guid, out val), "Item got not deleted.");
            }
        }

        [TestMethod]
        public void CanDeleteAllItems()
        {
            const int count = 20;
            double val;
            Random random = new Random();

            using (var instance = new MFAttributes(0))
            {
                for (int i = 0; i < count; i++)
                {
                    Guid guid = Guid.NewGuid();
                    val = random.NextDouble() * (i + 1);
                    instance.SetDouble(guid, val);
                    Assert.AreEqual(val, instance.GetDouble(guid));
                }

                Assert.AreEqual(count, instance.Count);
                instance.DeleteAllItems();
                Assert.AreEqual(0, instance.Count);
            }
        }

        [TestMethod]
        public void CanLockAndUnlockStore()
        {
            Guid guid = Guid.NewGuid();
            double val;

            using (var instance = new MFAttributes(0))
            using(var waitHandle = new AutoResetEvent(false))
            {
                instance.LockStore();
                try
                {
                    instance.SetDouble(Guid.NewGuid(), 10);

                    ThreadPool.QueueUserWorkItem(o =>
                    {
                        waitHandle.Set();
                        instance.SetDouble(guid, 500);
                        waitHandle.Set();
                    });

                    waitHandle.WaitOne(); //wait for the threadpool the queue the user item
                    Thread.Sleep(2000); //give the threadpool some additional time to call instance.SetDouble
                    Assert.IsFalse(instance.TryGet(guid, out val), "Second thread could set the attribute while instance was locked.");
                }
                finally
                {
                    instance.UnlockStore();
                }

                waitHandle.WaitOne();
            }
        }

        [TestMethod]
        public void CanGetItemByIndex()
        {
            using (var instance = new MFAttributes(0))
            {
                var guid = Guid.NewGuid();
                instance.SetDouble(guid, 10);

                Assert.AreEqual(10d, instance[0].GetValue());
                Assert.AreEqual(10d, instance[guid]);
                using (var instance0 = new MFAttributes(0))
                {
                    instance.CopyAllItems(instance0);

                    Assert.AreEqual(10d, instance[0].GetValue());
                    Assert.AreEqual(10d, instance[guid]);
                }
            }
        }

        struct TestStruct
        {
            public Guid Field0;
            public long Field1;
            public double Field2;
        }
    }
}
