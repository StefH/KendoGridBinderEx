using System.Collections.Generic;

namespace KendoGridBinder.Containers
{
    public class FilterObjectWrapper
    {
        public string Logic { get; set; }

        public IEnumerable<FilterObject> FilterObjects { get; set; }

        public string LogicToken
        {
            get
            {
                switch (Logic)
                {
                    case "and":
                        return "&&";
                    case "or":
                        return "||";
                    default:
                        return null;
                }
            }
        }
    }
}
