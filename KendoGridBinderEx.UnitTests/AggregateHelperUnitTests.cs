using System.Collections.Generic;
using KendoGridBinderEx.Containers.Json;
using KendoGridBinderEx.ModelBinder;
using NUnit.Framework;

namespace KendoGridBinderEx.UnitTests
{
    [TestFixture]
    class AggregateHelperUnitTests
    {
        [Test]
        public void AggregateHelper_TestMapNull()
        {
            IEnumerable<AggregateObject> objects = AggregateHelper.Map(null);
            Assert.IsNull(objects);
        }
    }
}