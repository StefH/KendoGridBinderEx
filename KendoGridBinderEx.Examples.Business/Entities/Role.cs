using System.Collections.Generic;

namespace KendoGridBinderEx.Examples.Business.Entities
{
    public class Role : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual List<User> Users { get; set; }
    }
}