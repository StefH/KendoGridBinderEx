using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Service.Interface;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    public class MainCompanyController : BaseMvcGridController<MainCompany, MainCompany>
    {
        private readonly IMainCompanyService _mainCompanyService;

        public MainCompanyController(IMainCompanyService mainCompanyService)
            : base(mainCompanyService)
        {
            _mainCompanyService = mainCompanyService;
        }
    }
}