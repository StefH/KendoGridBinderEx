using System.Collections.Specialized;
using System.Web;

namespace KendoGridBinderEx.UnitTests.Helpers
{
    public class FakeContext : HttpContextBase
    {
        private readonly HttpRequestBase _request;

        public FakeContext(HttpRequestBase request)
        {
            _request = request;
        }

        public override HttpRequestBase Request
        {
            get
            {
                return _request;
            }
        }
    }

    public class FakeRequest : HttpRequestBase
    {
        private readonly string _httpMethod;
        private readonly NameValueCollection _form;
        private readonly NameValueCollection _query;

        public FakeRequest(string httpMethod, NameValueCollection form, NameValueCollection query)
        {
            _httpMethod = httpMethod;
            _form = form;
            _query = query;
        }

        public override string this[string key]
        {
            get
            {
                return _form[key];
            }
        }

        public override string HttpMethod
        {
            get
            {
                return _httpMethod;
            }
        }

        public override NameValueCollection Form
        {
            get
            {
                return _form;
            }
        }

        public override NameValueCollection QueryString
        {
            get
            {
                return _query;
            }
        }
    }
}