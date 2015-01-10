using System;
using System.Runtime.Serialization;

namespace KendoGridBinderEx.Containers.Json
{
    /// <summary>
    /// This class maps 1 : 1 to JSON Aggregate
    /// </summary>
    [DataContract(Name = "aggregate")]
    public class AggregateObject
    {
        [DataMember(Name = "field")]
        public string Field { get; set; }

        [DataMember(Name = "aggregate")]
        public string Aggregate { get; set; }

        // TODO : is this used/supported ?
        [DataMember(Name = "dir")]
        public string Direction { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateObject"/> class.
        /// </summary>
        public AggregateObject()
        {
            Direction = "asc";
        }

        /// <summary>
        /// Get the Linq Aggregate
        /// </summary>
        /// <param name="fieldConverter">Convert which can be used to convert ViewModel to Model if needed.</param>
        /// <returns>string</returns>
        public string GetLinqAggregate(Func<string, string> fieldConverter = null)
        {
            string convertedField = fieldConverter != null ? fieldConverter.Invoke(Field) : Field;
            switch (Aggregate)
            {
                case "count":
                    return string.Format("Count() as count__{0}", Field);

                case "sum":
                    return string.Format("Sum(TEntity__.{0}) as sum__{1}", convertedField, Field);

                case "max":
                    return string.Format("Max(TEntity__.{0}) as max__{1}", convertedField, Field);

                case "min":
                    return string.Format("Min(TEntity__.{0}) as min__{1}", convertedField, Field);

                case "average":
                    return string.Format("Average(TEntity__.{0}) as average__{1}", convertedField, Field);

                default:
                    return string.Empty;
            }
        }
    }
}