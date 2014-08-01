using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCore.DMO;

namespace CSCore.Test.DMO
{
    [TestClass]
    public class DmoEnumeratorTests
    {
        [TestMethod]
        [TestCategory("DMO")]
        public void CanEnumerateEffects()
        {
            foreach (var item in EnumDmo.EnumerateDMOs(DmoEnumeratorCategories.AudioEffect, DmoEnumFlags.None))
            {
                Console.WriteLine("\"{0}\": {1}", item.Name, item.CLSID);
            }
        }
    }
}
