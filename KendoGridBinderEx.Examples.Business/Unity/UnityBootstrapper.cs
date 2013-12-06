using Microsoft.Practices.Unity;

namespace KendoGridBinderEx.Examples.Business.Unity
{
    public static class UnityBootstrapper
    {
        private static readonly IUnityContainer UnityContainer = new UnityContainer();

        public static IUnityContainer Container { get { return UnityContainer; } }
    }
}