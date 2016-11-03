using System.Collections.Specialized;
using System.Linq;
using KendoGridBinderEx.UnitTests.Helpers;
using KendoGridBinderEx;
using NUnit.Framework;

namespace KendoGridBinderEx.UnitTests
{
    [TestFixture]
    public class KendoGridModelBinderParseTests : TestHelper
    {
        [Test]
        public void TestParse_KendoGridModelBinder_Page()
        {
            var form = new NameValueCollection
            {
                {"take", "5"},
                {"skip", "0"},
                {"page", "1"},
                {"pagesize", "5"}
            };

            var gridRequest = SetupBinder(form, null);
            CheckTake(gridRequest, 5);
            CheckSkip(gridRequest, 0);
            CheckPage(gridRequest, 1);
            CheckPageSize(gridRequest, 5);
        }

        [Test]
        public void TestParse_KendoGridModelBinder_Page_Filter()
        {
            var form = new NameValueCollection
            {
                {"take", "5"},
                {"skip", "0"},
                {"page", "1"},
                {"pagesize", "5"},

                {"filter[filters][0][field]", "CompanyName"},
                {"filter[filters][0][operator]", "eq"},
                {"filter[filters][0][value]", "A"},
                {"filter[logic]", "and"},
            };

            var gridRequest = SetupBinder(form, null);
            CheckTake(gridRequest, 5);
            CheckSkip(gridRequest, 0);
            CheckPage(gridRequest, 1);
            CheckPageSize(gridRequest, 5);

            Assert.IsNotNull(gridRequest.FilterObjectWrapper);
            Assert.AreEqual("and", gridRequest.FilterObjectWrapper.Logic);
            Assert.AreEqual("&&", gridRequest.FilterObjectWrapper.LogicToken);

            Assert.IsNotNull(gridRequest.FilterObjectWrapper.FilterObjects);
            Assert.AreEqual(1, gridRequest.FilterObjectWrapper.FilterObjects.Count());

            var filterObjects = gridRequest.FilterObjectWrapper.FilterObjects.ToList();
            var filter1 = filterObjects[0];
            Assert.AreEqual(false, filter1.IsConjugate);
            Assert.AreEqual("CompanyName", filter1.Field1);
            Assert.AreEqual("eq", filter1.Operator1);
            Assert.AreEqual("A", filter1.Value1);
            Assert.AreEqual(null, filter1.Logic);
            Assert.AreEqual(null, filter1.LogicToken);
        }

        [Test]
        public void TestParse_KendoGridModelBinder_Page_Filter_DifferentOrder()
        {
            var form = new NameValueCollection
            {
                {"take", "5"},
                {"skip", "0"},
                {"page", "1"},
                {"pagesize", "5"},

                {"filter[filters][0][operator]", "eq"}, // Different order
                {"filter[filters][0][field]", "CompanyName"}, // Different order
                {"filter[filters][0][value]", "A"}, // Different order
                {"filter[logic]", "and"},
            };

            var gridRequest = SetupBinder(form, null);
            CheckTake(gridRequest, 5);
            CheckSkip(gridRequest, 0);
            CheckPage(gridRequest, 1);
            CheckPageSize(gridRequest, 5);

            Assert.IsNotNull(gridRequest.FilterObjectWrapper);
            Assert.AreEqual("and", gridRequest.FilterObjectWrapper.Logic);
            Assert.AreEqual("&&", gridRequest.FilterObjectWrapper.LogicToken);

            Assert.IsNotNull(gridRequest.FilterObjectWrapper.FilterObjects);
            Assert.AreEqual(1, gridRequest.FilterObjectWrapper.FilterObjects.Count());

            var filterObjects = gridRequest.FilterObjectWrapper.FilterObjects.ToList();
            var filter1 = filterObjects[0];
            Assert.AreEqual(false, filter1.IsConjugate);
            Assert.AreEqual("CompanyName", filter1.Field1);
            Assert.AreEqual("eq", filter1.Operator1);
            Assert.AreEqual("A", filter1.Value1);
            Assert.AreEqual(null, filter1.Logic);
            Assert.AreEqual(null, filter1.LogicToken);
        }

        [Test]
        public void TestParse_KendoGridModelBinder_Page_Filter_Sort()
        {
            var form = new NameValueCollection
            {
                {"take", "5"},
                {"skip", "0"},
                {"page", "1"},
                {"pagesize", "5"},
                
                {"sort[0][field]", "First"},
                {"sort[0][dir]", "asc"},
                {"sort[1][field]", "Email"},
                {"sort[1][dir]", "desc"},

                {"filter[filters][0][logic]", "or"},
                {"filter[filters][0][filters][0][field]", "CompanyName"},
                {"filter[filters][0][filters][0][operator]", "eq"},
                {"filter[filters][0][filters][0][value]", "A"},
                {"filter[filters][0][filters][1][field]", "CompanyName"},
                {"filter[filters][0][filters][1][operator]", "contains"},
                {"filter[filters][0][filters][1][value]", "B"},
                
                {"filter[filters][1][field]", "Last"},
                {"filter[filters][1][operator]", "contains"},
                {"filter[filters][1][value]", "s"},
                {"filter[logic]", "and"}
            };

            var gridRequest = SetupBinder(form, null);
            CheckTake(gridRequest, 5);
            CheckSkip(gridRequest, 0);
            CheckPage(gridRequest, 1);
            CheckPageSize(gridRequest, 5);

            Assert.IsNotNull(gridRequest.FilterObjectWrapper);
            Assert.AreEqual("and", gridRequest.FilterObjectWrapper.Logic);
            Assert.AreEqual("&&", gridRequest.FilterObjectWrapper.LogicToken);

            Assert.IsNotNull(gridRequest.FilterObjectWrapper.FilterObjects);
            Assert.AreEqual(2, gridRequest.FilterObjectWrapper.FilterObjects.Count());

            var filterObjects = gridRequest.FilterObjectWrapper.FilterObjects.ToList();
            var filter1 = filterObjects[0];
            Assert.AreEqual(true, filter1.IsConjugate);
            Assert.AreEqual("CompanyName", filter1.Field1);
            Assert.AreEqual("eq", filter1.Operator1);
            Assert.AreEqual("A", filter1.Value1);
            Assert.AreEqual("CompanyName", filter1.Field2);
            Assert.AreEqual("contains", filter1.Operator2);
            Assert.AreEqual("B", filter1.Value2);
            Assert.AreEqual("or", filter1.Logic);
            Assert.AreEqual("||", filter1.LogicToken);

            var filter2 = filterObjects[1];
            Assert.AreEqual(false, filter2.IsConjugate);
            Assert.AreEqual("Last", filter2.Field1);
            Assert.AreEqual("contains", filter2.Operator1);
            Assert.AreEqual("s", filter2.Value1);

            Assert.IsNotNull(gridRequest.SortObjects);
            Assert.AreEqual(2, gridRequest.SortObjects.Count());
            var sortObjects = gridRequest.SortObjects.ToList();

            var sort1 = sortObjects[0];
            Assert.AreEqual("First", sort1.Field);
            Assert.AreEqual("asc", sort1.Direction);

            var sort2 = sortObjects[1];
            Assert.AreEqual("Email", sort2.Field);
            Assert.AreEqual("desc", sort2.Direction);
        }

        //{"take":5,"skip":0,"page":1,"pageSize":5,"filter":{"logic":"and","filters":[{"field":"CompanyName","operator":"eq","value":"A"}]},"group":[]}
        [Test]
        public void TestParseJson_KendoGridModelBinder_Page_Filter()
        {
            var form = new NameValueCollection
            {
                {"take", "5"},
                {"skip", "0"},
                {"page", "1"},
                {"pagesize", "5"},

                {"group", "[]"},
                {"filter", "{\"logic\":\"and\",\"filters\":[{\"field\":\"CompanyName\",\"operator\":\"eq\",\"value\":\"A\"}]}"}
            };

            var gridRequest = SetupBinder(form, null);
            CheckTake(gridRequest, 5);
            CheckSkip(gridRequest, 0);
            CheckPage(gridRequest, 1);
            CheckPageSize(gridRequest, 5);

            Assert.IsNotNull(gridRequest.FilterObjectWrapper);
            Assert.AreEqual("and", gridRequest.FilterObjectWrapper.Logic);
            Assert.AreEqual("&&", gridRequest.FilterObjectWrapper.LogicToken);

            Assert.IsNotNull(gridRequest.FilterObjectWrapper.FilterObjects);
            Assert.AreEqual(1, gridRequest.FilterObjectWrapper.FilterObjects.Count());

            var filterObjects = gridRequest.FilterObjectWrapper.FilterObjects.ToList();
            var filter1 = filterObjects[0];
            Assert.AreEqual(false, filter1.IsConjugate);
            Assert.AreEqual("CompanyName", filter1.Field1);
            Assert.AreEqual("eq", filter1.Operator1);
            Assert.AreEqual("A", filter1.Value1);
            Assert.AreEqual(null, filter1.Logic);
            Assert.AreEqual(null, filter1.LogicToken);
        }

        //{"take":5,"skip":0,"page":1,"pageSize":5,"sort":[{"field":"FirstName","dir":"asc","compare":null}],"filter":{"logic":"and","filters":[{"field":"CompanyName","operator":"eq","value":"A"}]},"group":[]}
        [Test]
        public void TestParseJson_KendoGridModelBinder_Page_Filter_Sort()
        {
            var form = new NameValueCollection
            {
                {"take", "5"},
                {"skip", "0"},
                {"page", "1"},
                {"pagesize", "5"},

                {"group", "[]"},
                {"filter", "{\"logic\":\"and\",\"filters\":[{\"logic\":\"or\",\"filters\":[{\"field\":\"LastName\",\"operator\":\"contains\",\"value\":\"s\"},{\"field\":\"LastName\",\"operator\":\"endswith\",\"value\":\"ll\"}]},{\"field\":\"FirstName\",\"operator\":\"startswith\",\"value\":\"n\"}]}"},
                {"sort", "[{\"field\":\"FirstName\",\"dir\":\"asc\",\"compare\":null},{\"field\":\"LastName\",\"dir\":\"desc\",\"compare\":null}]"}
            };

            var gridRequest = SetupBinder(form, null);
            CheckTake(gridRequest, 5);
            CheckSkip(gridRequest, 0);
            CheckPage(gridRequest, 1);
            CheckPageSize(gridRequest, 5);

            Assert.IsNotNull(gridRequest.FilterObjectWrapper);
            Assert.AreEqual("and", gridRequest.FilterObjectWrapper.Logic);
            Assert.AreEqual("&&", gridRequest.FilterObjectWrapper.LogicToken);

            Assert.IsNotNull(gridRequest.FilterObjectWrapper.FilterObjects);
            Assert.AreEqual(2, gridRequest.FilterObjectWrapper.FilterObjects.Count());

            var filterObjects = gridRequest.FilterObjectWrapper.FilterObjects.ToList();
            var filter1 = filterObjects[0];
            Assert.AreEqual(true, filter1.IsConjugate);
            Assert.AreEqual("LastName", filter1.Field1);
            Assert.AreEqual("contains", filter1.Operator1);
            Assert.AreEqual("s", filter1.Value1);
            Assert.AreEqual("LastName", filter1.Field2);
            Assert.AreEqual("endswith", filter1.Operator2);
            Assert.AreEqual("ll", filter1.Value2);

            var filter2 = filterObjects[1];
            Assert.AreEqual(false, filter2.IsConjugate);
            Assert.AreEqual("FirstName", filter2.Field1);
            Assert.AreEqual("startswith", filter2.Operator1);
            Assert.AreEqual("n", filter2.Value1);
            Assert.AreEqual(null, filter2.Logic);
            Assert.AreEqual(null, filter2.LogicToken);

            var sortObjects = gridRequest.SortObjects;
            Assert.IsNotNull(sortObjects);

            var sortList = sortObjects.ToList();
            Assert.AreEqual("FirstName", sortList.First().Field);
            Assert.AreEqual("asc", sortList.First().Direction);
            Assert.AreEqual("LastName", sortList.Last().Field);
            Assert.AreEqual("desc", sortList.Last().Direction);
        }

        [Test]
        public void TestParse_KendoGridModelBinder_Page_Agggregates()
        {
            var form = new NameValueCollection
            {
                {"take", "5"},
                {"skip", "0"},
                {"page", "1"},
                {"pagesize", "5"},

                {"aggregate[0][field]", "id"},
                {"aggregate[0][aggregate]", "count"},
                {"aggregate[1][field]", "id"},
                {"aggregate[1][aggregate]", "sum"}
            };

            var gridRequest = SetupBinder(form, null);
            CheckTake(gridRequest, 5);
            CheckSkip(gridRequest, 0);
            CheckPage(gridRequest, 1);
            CheckPageSize(gridRequest, 5);

            Assert.IsNull(gridRequest.FilterObjectWrapper);
            Assert.AreEqual(0, gridRequest.SortObjects.Count());
            Assert.IsNull(gridRequest.GroupObjects);

            var aggregateObjects = gridRequest.AggregateObjects.ToList();
            Assert.AreEqual(2, aggregateObjects.Count);

            var aggregate0 = aggregateObjects[0];
            Assert.AreEqual("id", aggregate0.Field);
            Assert.AreEqual("count", aggregate0.Aggregate);

            var aggregate1 = aggregateObjects[1];
            Assert.AreEqual("id", aggregate1.Field);
            Assert.AreEqual("sum", aggregate1.Aggregate);
        }

        #region Check helper methods
        private static void CheckTake(KendoGridBaseRequest gridRequest, int take)
        {
            Assert.IsNotNull(gridRequest.Take);
            Assert.AreEqual(take, gridRequest.Take.Value);
        }

        private static void CheckSkip(KendoGridBaseRequest gridRequest, int skip)
        {
            Assert.IsNotNull(gridRequest.Skip);
            Assert.AreEqual(skip, gridRequest.Skip.Value);
        }

        private static void CheckPage(KendoGridBaseRequest gridRequest, int page)
        {
            Assert.IsNotNull(gridRequest.Page);
            Assert.AreEqual(page, gridRequest.Page.Value);
        }

        private static void CheckPageSize(KendoGridBaseRequest gridRequest, int pagesize)
        {
            Assert.IsNotNull(gridRequest.PageSize);
            Assert.AreEqual(pagesize, gridRequest.PageSize.Value);
        }
        #endregion
    }
}