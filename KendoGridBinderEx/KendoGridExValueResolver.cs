using System;
using System.Linq.Expressions;
using AutoMapper;

namespace KendoGridBinderEx
{
    public interface IKendoGridExValueResolver
    {
        string GetDestinationProperty();
    }
}