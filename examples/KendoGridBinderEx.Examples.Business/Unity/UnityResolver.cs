using Microsoft.Practices.Unity;

namespace KendoGridBinderEx.Examples.Business.Unity
{
    public static class UnityResolver
    {
        public static T Resolve<T>()
        {
            return UnityBootstrapper.Container.Resolve<T>();
        }
    }
}