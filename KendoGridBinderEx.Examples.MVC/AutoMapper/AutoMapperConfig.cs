using System;
using AutoMapper;
using KendoGridBinderEx.AutoMapperExtensions;

namespace KendoGridBinderEx.Examples.MVC.AutoMapper
{
    public static class AutoMapperConfig
    {
        private static MapperConfiguration _mapperConfiguration;
        private static IMapper _mapper;
        private static AutoMapperUtils _autoMapperUtils;

        public static MapperConfiguration MapperConfiguration => _mapperConfiguration;
        public static IMapper Mapper => _mapper;
        public static AutoMapperUtils AutoMapperUtils => _autoMapperUtils;

        public static void InitAutoMapper(Func<Type, object> resolver)
        {
            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.ConstructServicesUsing(resolver);

                cfg.AddProfile<EmployeeProfile>();
                cfg.AddProfile<FunctionProfile>();
                cfg.AddProfile<OUProfile>();
                cfg.AddProfile<UserProfile>();
                cfg.AddProfile<SubFunctionProfile>();
                cfg.AddProfile<ProductProfile>();
            });

            _mapperConfiguration.AssertConfigurationIsValid();

            _mapper = _mapperConfiguration.CreateMapper();

            _autoMapperUtils = new AutoMapperUtils(_mapperConfiguration);
        }
    }
}