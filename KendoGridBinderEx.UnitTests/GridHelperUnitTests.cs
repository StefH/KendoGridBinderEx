using System.Collections.Generic;
using KendoGridBinderEx.Containers.Json;
using KendoGridBinderEx.ModelBinder;
using KendoGridBinderEx.ModelBinder.Api;
using NUnit.Framework;

namespace KendoGridBinderEx.UnitTests
{
    [TestFixture]
    public class GridHelperUnitTests
    {
        [Test]
        public void GridHelper_ParseTest()
        {
            const string jsonString = "{\"take\":10,\"skip\":3,\"page\":1,\"pageSize\":11,\"group\":[],\"aggregate\":[]}";
            KendoGridApiRequest result = GridHelper.Parse(jsonString);

            Assert.IsNotNull(result);
            Assert.AreEqual(null, result.AggregateObjects);
            Assert.AreEqual(null, result.FilterObjectWrapper);
            Assert.AreEqual(null, result.GroupObjects);
            Assert.AreEqual(null, result.Logic);
            Assert.AreEqual(1, result.Page);
            Assert.AreEqual(11, result.PageSize);
            Assert.AreEqual(3, result.Skip);
            Assert.AreEqual(null, result.SortObjects);
            Assert.AreEqual(10, result.Take);
        }

        [Test]
        public void GridHelper_ParseGroup()
        {
            const string jsonString = "{\"group\":[]}";
            KendoGridApiRequest result = GridHelper.Parse(jsonString);

            Assert.IsNotNull(result);
            Assert.AreEqual(null, result.AggregateObjects);
            Assert.AreEqual(null, result.FilterObjectWrapper);
            Assert.AreEqual(null, result.GroupObjects);
            Assert.AreEqual(null, result.Logic);
            Assert.AreEqual(null, result.Page);
            Assert.AreEqual(null, result.PageSize);
            Assert.AreEqual(null, result.Skip);
            Assert.AreEqual(null, result.SortObjects);
            Assert.AreEqual(null, result.Take);
        }

        [Test]
        public void GridHelper_ParseAggregates()
        {
            const string jsonString = "{\"aggregate\":[]}";
            var result = GridHelper.Parse(jsonString);

            Assert.IsNotNull(result);
            Assert.AreEqual(null, result.AggregateObjects);
            Assert.AreEqual(null, result.FilterObjectWrapper);
            Assert.AreEqual(null, result.GroupObjects);
            Assert.AreEqual(null, result.Logic);
            Assert.AreEqual(null, result.Page);
            Assert.AreEqual(null, result.PageSize);
            Assert.AreEqual(null, result.Skip);
            Assert.AreEqual(null, result.SortObjects);
            Assert.AreEqual(null, result.Take);
        }
    }
}
