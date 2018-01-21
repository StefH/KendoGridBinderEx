
namespace KendoGridBinderEx.Examples.MVC.Extensions
{
    public static class StringExtensions
    {
        public static string Left(this string param, int length)
        {
            return param.Substring(0, length);
        }

        public static string Right(this string param, int length)
        {
            return param.Substring(param.Length - length, length);
        }
    }
}