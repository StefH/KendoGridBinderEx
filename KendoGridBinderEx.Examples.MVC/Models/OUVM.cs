using KendoGridBinderEx.Examples.Business.Entities;

namespace KendoGridBinderEx.Examples.MVC.Models
{
    public class OUVM : IEntity
    {
        public long Id { get; set; }
        
        public string Code { get; set; }

        public string Name { get; set; }
    }
}