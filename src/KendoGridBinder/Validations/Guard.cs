using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace KendoGridBinder.Validations
{
    [DebuggerStepThrough]
    public static class Guard
    {
        [ContractAnnotation("value:null => halt")]
        public static T NotNull<T>([NoEnumeration] T value, [InvokerParameterName] [NotNull] string argumentName)
        {
            if (ReferenceEquals(value, null))
            {
                NotNullOrEmpty(argumentName, nameof(argumentName));

                throw new ArgumentNullException(argumentName);
            }

            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static string NotNullOrEmpty(string value, [InvokerParameterName] [NotNull] string argumentName)
        {
            Exception e = null;
            if (ReferenceEquals(value, null))
            {
                e = new ArgumentNullException(argumentName);
            }
            else if (value.Trim().Length == 0)
            {
                e = new ArgumentException($"The string argument '{argumentName}' cannot be empty.");
            }

            if (e != null)
            {
                NotNullOrEmpty(argumentName, nameof(argumentName));

                throw e;
            }

            return value;
        }
    }
}