using KendoGridBinderEx.Examples.Business.Entities;

namespace KendoGridBinderEx.Examples.MVC.Models
{
    public class RoleVM : IEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}