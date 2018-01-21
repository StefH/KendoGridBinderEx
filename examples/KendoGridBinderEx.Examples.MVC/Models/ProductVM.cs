using System.Web.Mvc;
using KendoGridBinderEx.Examples.Business.Entities;

namespace KendoGridBinderEx.Examples.MVC.Models
{
    public class ProductVM : IEntity
    {
        public long Id { get; set; }

        [Remote("IsCodeUnique", "Product", AdditionalFields = "Id")]
        public string Code { get; set; }

        public string Name { get; set; }
    }
}