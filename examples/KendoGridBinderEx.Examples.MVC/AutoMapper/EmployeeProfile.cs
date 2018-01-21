﻿using AutoMapper;
using KendoGridBinderEx.Examples.Business.AutoMapper;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.MVC.Models;

namespace KendoGridBinderEx.Examples.MVC.AutoMapper
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeVM>()
                .ForMember(vm => vm.First, opt => opt.MapFrom(m => m.FirstName))
                .ForMember(vm => vm.Full, opt => opt.MapFrom(m => m.FullName))
                .ForMember(vm => vm.LastName, opt => opt.MapFrom(m => m.LastName))
                .ForMember(vm => vm.Number, opt => opt.MapFrom(m => m.EmployeeNumber))
                .ForMember(vm => vm.CompanyId, opt => opt.MapFrom(e => e.Company.Id))
                .ForMember(vm => vm.CompanyName, opt => opt.MapFrom(m => m.Company.Name))
                .ForMember(vm => vm.MainCompanyName, opt => opt.MapFrom(m => m.Company.MainCompany.Name))
                .ForMember(vm => vm.CountryId, opt => opt.MapFrom(m => m.Country.Id))
                .ForMember(vm => vm.CountryCode, opt => opt.MapFrom(m => m.Country.Code))
                .ForMember(vm => vm.CountryName, opt => opt.MapFrom(m => m.Country.Name))
                .ForMember(vm => vm.FunctionId, opt => opt.MapFrom(m => m.Function.Id))
                .ForMember(vm => vm.FunctionCode, opt => opt.MapFrom(m => m.Function.Code))
                .ForMember(vm => vm.FunctionName, opt => opt.MapFrom(m => m.Function.Name))
                .ForMember(vm => vm.SubFunctionId, opt => opt.MapFrom(m => m.SubFunction.Id))
                .ForMember(vm => vm.SubFunctionCode, opt => opt.MapFrom(m => m.SubFunction.Code))
                .ForMember(vm => vm.SubFunctionName, opt => opt.MapFrom(m => m.SubFunction.Name))
                .ForMember(vm => vm.ResourceType, opt => opt.UseValue("Employee"))
                ;

            CreateMap<EmployeeVM, Employee>()
                .ForMember(e => e.EmployeeNumber, opt => opt.MapFrom(vm => vm.Number))
                .ForMember(e => e.FirstName, opt => opt.MapFrom(vm => vm.First))
                .ForMember(e => e.LastName, opt => opt.MapFrom(vm => vm.LastName))
                .ForMember(e => e.Company, opt => opt.ResolveUsing<EntityResolver<Company>>()) //.FromMember(vm => vm.CompanyId))
                .ForMember(e => e.Country, opt => opt.ResolveUsing<EntityResolver<Country>>()) //.FromMember(vm => vm.CountryId))
                .ForMember(e => e.Function, opt => opt.ResolveUsing<EntityResolver<Function>>()) //.FromMember(vm => vm.FunctionId))
                .ForMember(e => e.SubFunction, opt => opt.ResolveUsing<EntityResolver<SubFunction>>()) //.FromMember(vm => vm.SubFunctionId))
                ;

            CreateMap<Employee, EmployeeDetailVM>()
                .ForMember(vm => vm.First, opt => opt.MapFrom(m => m.FirstName))
                .ForMember(vm => vm.Full, opt => opt.MapFrom(m => m.FullName))
                .ForMember(vm => vm.LastName, opt => opt.MapFrom(m => m.LastName))
                .ForMember(vm => vm.FunctionCode, opt => opt.MapFrom(m => m.Function.Code))
                .ForMember(vm => vm.SubFunctionCode, opt => opt.MapFrom(m => m.SubFunction.Code))
                .ForMember(vm => vm.ResourceType, opt => opt.UseValue("Employee"))
                ;

            CreateMap<EmployeeDetailVM, Employee>()
                .ForAllMembers(opt => opt.Ignore());


            // View
            CreateMap<EmployeeView, EmployeeViewVM>()
                .ForMember(vm => vm.First, opt => opt.MapFrom(m => m.FirstName))
                .ForMember(vm => vm.Full, opt => opt.MapFrom(m => m.FullName))
                .ForMember(vm => vm.LastName, opt => opt.MapFrom(m => m.LastName))
                .ForMember(vm => vm.Number, opt => opt.MapFrom(m => m.EmployeeNumber))
                //.ForMember(vm => vm.CompanyId, opt => opt.MapFrom(e => e.Company.Id))
                //.ForMember(vm => vm.CompanyName, opt => opt.MapFrom(m => m.Company.Name))
                //.ForMember(vm => vm.MainCompanyName, opt => opt.MapFrom(m => m.Company.MainCompany.Name))
                //.ForMember(vm => vm.CountryId, opt => opt.MapFrom(m => m.Country.Id))
                .ForMember(vm => vm.CountryCode, opt => opt.MapFrom(m => m.CountryCode))
                .ForMember(vm => vm.CountryName, opt => opt.MapFrom(m => m.CountryName))
                //.ForMember(vm => vm.FunctionId, opt => opt.MapFrom(m => m.Function.Id))
                //.ForMember(vm => vm.FunctionCode, opt => opt.MapFrom(m => m.Function.Code))
                //.ForMember(vm => vm.FunctionName, opt => opt.MapFrom(m => m.Function.Name))
                //.ForMember(vm => vm.SubFunctionId, opt => opt.MapFrom(m => m.SubFunction.Id))
                //.ForMember(vm => vm.SubFunctionCode, opt => opt.MapFrom(m => m.SubFunction.Code))
                //.ForMember(vm => vm.SubFunctionName, opt => opt.MapFrom(m => m.SubFunction.Name))
                .ForMember(vm => vm.ResourceType, opt => opt.UseValue("Employee"))
                ;

            CreateMap<EmployeeVM, Employee>()
                .ForMember(e => e.EmployeeNumber, opt => opt.MapFrom(vm => vm.Number))
                .ForMember(e => e.FirstName, opt => opt.MapFrom(vm => vm.First))
                .ForMember(e => e.LastName, opt => opt.MapFrom(vm => vm.LastName))
                .ForMember(e => e.Company, opt => opt.ResolveUsing<EntityResolver<Company>>()) //.FromMember(vm => vm.CompanyId))
                .ForMember(e => e.Country, opt => opt.ResolveUsing<EntityResolver<Country>>()) //.FromMember(vm => vm.CountryId))
                .ForMember(e => e.Function, opt => opt.ResolveUsing<EntityResolver<Function>>()) //.FromMember(vm => vm.FunctionId))
                .ForMember(e => e.SubFunction, opt => opt.ResolveUsing<EntityResolver<SubFunction>>()) //.FromMember(vm => vm.SubFunctionId))
                ;

            CreateMap<Employee, EmployeeDetailVM>()
                .ForMember(vm => vm.First, opt => opt.MapFrom(m => m.FirstName))
                .ForMember(vm => vm.Full, opt => opt.MapFrom(m => m.FullName))
                .ForMember(vm => vm.LastName, opt => opt.MapFrom(m => m.LastName))
                .ForMember(vm => vm.FunctionCode, opt => opt.MapFrom(m => m.Function.Code))
                .ForMember(vm => vm.SubFunctionCode, opt => opt.MapFrom(m => m.SubFunction.Code))
                .ForMember(vm => vm.ResourceType, opt => opt.UseValue("Employee"))
                ;

            CreateMap<EmployeeDetailVM, Employee>()
                .ForAllMembers(opt => opt.Ignore());
        }
    }
}