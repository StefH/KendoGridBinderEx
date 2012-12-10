using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;
using KendoGridBinder.Examples.MVC.Data.Entities;
using KendoGridBinder.Examples.MVC.Data.Service;

namespace KendoGridBinder.Examples.MVC.Data.Validation
{
    public static class ValidationRuleSets
    {
        public const string Edit = "Edit";
        public const string Create = "Create";
        public const string Delete = "Delete";
    }

    public abstract class BaseValidator<T> : AbstractValidator<T> where T : class, IEntity, new()
    {
        protected readonly BaseService<T> Service;

        protected BaseValidator(BaseService<T> service)
        {
            Service = service;
        }

        public ValidationResult ValidateAll(T instance)
        {
            return this.Validate(instance, ruleSet: "*");
        }

        public ValidationResult Validate(T instance, params string[] ruleSets)
        {
            return ruleSets == null ? Validate(instance) : Validate(new ValidationContext<T>(instance, new PropertyChain(), new RulesetValidatorSelector(ruleSets)));
        }

        public ValidationResult Validate(long? id, Expression<Func<T, object>> propertyExpression, object value)
        {
            if (id > 0)
            {
                var entity = Service.GetById(id.Value);

                if (entity != null)
                {
                    return Validate(entity, propertyExpression, value);
                }
            }

            return Validate(propertyExpression, value);
        }

        public ValidationResult Validate<TValue>(Expression<Func<T, object>> propertyExpression, TValue value)
        {
            return Validate(new T(), propertyExpression, value);
        }

        public ValidationResult Validate<TValue>(T instance, Expression<Func<T, object>> propertyExpression, TValue value)
        {
            MemberExpression memberExpr;
            if (propertyExpression.Body is MemberExpression)
            {
                memberExpr = propertyExpression.Body as MemberExpression;
            }
            else if (propertyExpression.Body is UnaryExpression)
            {
                var unaryExpr = propertyExpression.Body as UnaryExpression;
                if (unaryExpr == null) throw new ArgumentException();
                memberExpr = unaryExpr.Operand as MemberExpression;
            }
            else
            {
                throw new ArgumentException();
            }

            if (memberExpr == null) throw new ArgumentException();

            var propertyInfo = memberExpr.Member as PropertyInfo;
            if (propertyInfo == null) throw new ArgumentException();

            propertyInfo.SetValue(instance, value, null);

            return this.Validate(instance, propertyExpression);
        }
    }
}