using System;
using System.Globalization;

namespace KendoGridBinderEx.Containers
{
    public class FilterObject
    {
        public string Field1 { get; set; }
        public string Operator1 { get; set; }
        public string Value1 { get; set; }
        public string IgnoreCase1 { get; set; }

        public string Field2 { get; set; }
        public string Operator2 { get; set; }
        public string Value2 { get; set; }
        public string IgnoreCase2 { get; set; }

        public string Logic { get; set; }

        public bool IsConjugate
        {
            get { return (Field2 != null); }
        }

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

        private string GetPropertyType(Type type, string field)
        {
            foreach (var part in field.Split('.'))
            {
                if (type == null)
                {
                    return null;
                }

                var info = type.GetProperty(part);
                if (info == null)
                {
                    return null;
                }

                type = info.PropertyType;
            }

            bool bIsGenericOrNullable = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

            return bIsGenericOrNullable ? type.GetGenericArguments()[0].Name.ToLower() : type.Name.ToLower();
        }

        public string GetExpression1<TEntity>()
        {
            return GetExpression<TEntity>(Field1, Operator1, Value1, IgnoreCase1);
        }

        public string GetExpression2<TEntity>()
        {
            return GetExpression<TEntity>(Field2, Operator2, Value2, IgnoreCase2);
        }

        private string GetExpression<TEntity>(string field, string op, string param, string ignoreCase)
        {
            var dataType = GetPropertyType(typeof(TEntity), field);
            string caseMod = string.Empty;
            string nullCheck = string.Empty;

            if (dataType == "string")
            {
                param = @"""" + param.ToLower() + @"""";
                caseMod = ".ToLower()"; // always ignore case
                nullCheck = string.Format("{0} != null && ", field);
            }

            if (dataType == "datetime")
            {
                var i = param.IndexOf("GMT", StringComparison.Ordinal);
                if (i > 0)
                {
                    param = param.Remove(i);
                }
                var date = DateTime.Parse(param, new CultureInfo("en-US"));

                var str = string.Format("DateTime({0}, {1}, {2})", date.Year, date.Month, date.Day);
                param = str;
            }

            string exStr;

            switch (op)
            {
                case "eq":
                    exStr = string.Format("({3}{0}{2} == {1})", field, param, caseMod, nullCheck);
                    break;

                case "neq":
                    exStr = string.Format("({3}{0}{2} != {1})", field, param, caseMod, nullCheck);
                    break;

                case "contains":
                    exStr = string.Format("({3}{0}{2}.Contains({1}))", field, param, caseMod, nullCheck);
                    break;

                case "doesnotcontain":
                    exStr = string.Format("({3}!{0}{2}.Contains({1}))", field, param, caseMod, nullCheck);
                    break;

                case "startswith":
                    exStr = string.Format("({3}{0}{2}.StartsWith({1}))", field, param, caseMod, nullCheck);
                    break;

                case "endswith":
                    exStr = string.Format("({3}{0}{2}.EndsWith({1}))", field, param, caseMod, nullCheck);
                    break;

                case "gte":
                    exStr = string.Format("({3}{0}{2} >= {1})", field, param, caseMod, nullCheck);
                    break;

                case "gt":
                    exStr = string.Format("({3}{0}{2} > {1})", field, param, caseMod, nullCheck);
                    break;

                case "lte":
                    exStr = string.Format("({3}{0}{2} <= {1})", field, param, caseMod, nullCheck);
                    break;

                case "lt":
                    exStr = string.Format("({3}{0}{2} < {1})", field, param, caseMod, nullCheck);
                    break;

                default:
                    exStr = string.Empty;
                    break;
            }

            return exStr;
        }
    }
}
