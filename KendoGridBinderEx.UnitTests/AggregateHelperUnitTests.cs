using System.Collections.Generic;
using KendoGridBinder.Containers.Json;
using KendoGridBinder.Containers.Json;
using KendoGridBinder.ModelBinder;
using NUnit.Framework;

namespace KendoGridBinder.UnitTests
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