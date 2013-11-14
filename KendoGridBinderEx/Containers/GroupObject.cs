using System.Collections.Generic;

namespace KendoGridBinderEx.Containers
{
    public class GroupObject
    {
        public string Field { get; set; }
        public string Direction { get; set; }

        public IEnumerable<AggregateObject> AggregateObjects { get; set; }
    }

    public class AggregateObject
    {
        public string Field { get; set; }
        public string Aggregate { get; set; }

        public string FieldToken
        {
            get
            {
                return string.Format("{0}__{1}", Aggregate, Field);
            }
        }

        public string LinqAggregate
        {
            get
            {
                switch (Aggregate)
                {
                    case "count":
                        return string.Format("count() as {0}", FieldToken);

                    case "sum":
                        return string.Format("sum({0}) as {1}", Field, FieldToken);

                    case "max":
                        return string.Format("max({0}) as {1}", Field, FieldToken);

                    case "min":
                        return string.Format("min({0}) as {1}", Field, FieldToken);

                    default:
                        return string.Empty;
                }
            }
        }
    }
}