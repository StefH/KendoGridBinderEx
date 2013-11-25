using System;
using System.Reflection;

namespace KendoGridBinderEx.Extensions
{
    /// <summary>
    /// http://dotnetfollower.com/wordpress/2012/12/c-how-to-set-or-get-value-of-a-private-or-internal-property-through-the-reflection/
    /// </summary>
    public static class ReflectionHelper
    {
        private static FieldInfo GetFieldInfo(Type type, string fieldName)
        {
            FieldInfo fieldInfo;
            do
            {
                fieldInfo = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                type = type.BaseType;
            }
            while (fieldInfo == null && type != null);

            return fieldInfo;
        }

        public static object GetFieldValue(this object obj, string fieldName)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            var objType = obj.GetType();
            var fieldInfo = GetFieldInfo(objType, fieldName);
            if (fieldInfo == null)
                throw new ArgumentOutOfRangeException("fieldName", string.Format("Couldn't find field {0} in type {1}", fieldName, objType.FullName));

            return fieldInfo.GetValue(obj);
        }

        public static void SetFieldValue(this object obj, string fieldName, object val)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            var objType = obj.GetType();
            var fieldInfo = GetFieldInfo(objType, fieldName);

            if (fieldInfo == null)
                throw new ArgumentOutOfRangeException("fieldName", string.Format("Couldn't find field {0} in type {1}", fieldName, objType.FullName));

            fieldInfo.SetValue(obj, val);
        }
    }
}
