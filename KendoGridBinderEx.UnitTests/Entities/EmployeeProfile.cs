using AutoMapper;
using KendoGridBinderEx.UnitTests.Helpers;

namespace KendoGridBinderEx.UnitTests.Entities
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeVM>()
                .ForMember(vm => vm.First, opt => opt.MapFrom(m => m.FirstName))
                .ForMember(vm => vm.Full, opt => opt.MapFrom(m => m.FullName))
                .ForMember(vm => vm.Last, opt => opt.MapFrom(m => m.LastName))
                .ForMember(vm => vm.Number, opt => opt.MapFrom(m => m.EmployeeNumber))

                //.ForMember(vm => vm.CompanyId, opt => opt.MapFrom(m => m.Company.Id))
                //.ForMember(vm => vm.CompanyName, opt => opt.MapFrom(m => m.Company.Name))
                //.ForMember(vm => vm.MainCompanyName, opt => opt.MapFrom(m => m.Company.MainCompany.Name))
                //.ForMember(vm => vm.CountryId, opt => opt.MapFrom(m => m.Country.Id))
                //.ForMember(vm => vm.CountryCode, opt => opt.MapFrom(m => m.Country.Code))
                //.ForMember(vm => vm.CountryName, opt => opt.MapFrom(m => m.Country.Name))

                .ForMember(vm => vm.CompanyId, opt => opt.ResolveUsing(new NullSafeResolver<Employee, EmployeeVM, long>(e => e.Company.Id)))
                /*.ForMember(vm => vm.CompanyName, opt => opt.Ignore())
                .ForMember(vm => vm.MainCompanyName, opt => opt.Ignore())
                .ForMember(vm => vm.CountryId, opt => opt.Ignore())
                .ForMember(vm => vm.CountryCode, opt => opt.Ignore())
                .ForMember(vm => vm.CountryName, opt => opt.Ignore())*/

                //.ForMember(vm => vm.CompanyName, opt => opt.ResolveUsing<CompanyNameResolver>().FromMember(x => x.Company))
                .ForMember(vm => vm.CompanyName, opt => opt.ResolveUsing(new NullSafeResolver<Employee, EmployeeVM, string>(e => e.Company.Name)))
                .ForMember(vm => vm.MainCompanyName, opt => opt.ResolveUsing(new NullSafeResolver<Employee, EmployeeVM, string>(e => e.Company.MainCompany.Name)))
                .ForMember(vm => vm.CountryId, opt => opt.ResolveUsing(new NullSafeResolver<Employee, EmployeeVM, long>(e => e.Country.Id)))
                .ForMember(vm => vm.CountryCode, opt => opt.ResolveUsing(new NullSafeResolver<Employee, EmployeeVM, string>(e => e.Country.Code)))
                .ForMember(vm => vm.CountryName, opt => opt.ResolveUsing(new NullSafeResolver<Employee, EmployeeVM, string>(e => e.Country.Name)))
                ;

            CreateMap<EmployeeVM, Employee>()
                .ForMember(e => e.Email, opt => opt.MapFrom(vm => vm.Email))
                .ForMember(e => e.EmployeeNumber, opt => opt.MapFrom(vm => vm.Number))
                .ForMember(e => e.FirstName, opt => opt.MapFrom(vm => vm.First))
                .ForMember(e => e.HireDate, opt => opt.MapFrom(vm => vm.HireDate))
                .ForMember(e => e.LastName, opt => opt.MapFrom(vm => vm.Last))
                .ForMember(e => e.Company, opt => opt.Ignore())
                .ForMember(e => e.Country, opt => opt.Ignore())
                ;
        }
    }
}