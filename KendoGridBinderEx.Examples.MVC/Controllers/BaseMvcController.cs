using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation.Results;
using KendoGridBinder;
using KendoGridBinder.AutoMapperExtensions;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Service.Interface;
using KendoGridBinderEx.Examples.Business.Validation;
using KendoGridBinderEx.Examples.MVC.AutoMapper;
using KendoGridBinder.ModelBinder.Mvc;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    /*
    public class NullSafeResolver<TSource, TResult> : ValueResolver<TSource, TResult>, IKendoGridExValueResolver
    {
        private readonly Expression<Func<TSource, TResult>> _expression;

        public NullSafeResolver(Expression<Func<TSource, TResult>> expression)
        {
            _expression = expression;
        }

        protected override TResult ResolveCore(TSource source)
        {
            return source.NullSafeGetValue(_expression, default(TResult));
        }

        public string GetDestinationProperty()
        {
            return _expression.Body.ToString().Replace(_expression.Parameters[0] + ".", string.Empty);
        }
    }
    */

    public abstract class BaseMvcController<TEntity, TViewModel> : Controller
        where TEntity : class, IEntity, new()
        where TViewModel : class, IEntity, new()
    {
        protected readonly IBaseService<TEntity> Service;
        protected readonly Dictionary<string, MapExpression<TEntity>> KendoGridExMappings;
        protected readonly Dictionary<string, List<MapExpression<TEntity>>> Mappings = new Dictionary<string, List<MapExpression<TEntity>>>();

        protected BaseMvcController(IBaseService<TEntity> service)
        {
            Service = service;

            KendoGridExMappings = AutoMapperConfig.AutoMapperUtils.GetModelMappings<TEntity, TViewModel>();

            if (KendoGridExMappings != null)
            {
                foreach (var mapping in KendoGridExMappings)
                {
                    Mappings.Add(mapping.Key, new List<MapExpression<TEntity>> { mapping.Value });
                }

                foreach (var mapping in KendoGridExMappings.Where(m => m.Value.Path.Contains('.')))
                {
                    mapping.Value.Path = mapping.Value.Path.Split('.').First();
                    Mappings[mapping.Key].Add(mapping.Value);
                }
            }
        }

        protected virtual IQueryable<TEntity> GetQueryable()
        {
            return Service.AsQueryable();
        }

        protected virtual IQueryable<TEntity> GetQueryableNoTracking()
        {
            return Service.AsQueryableNoTracking();
        }

        #region AutoMapper
        protected string MapFieldfromViewModeltoEntity(string field)
        {
            return (KendoGridExMappings != null && field != null && KendoGridExMappings.ContainsKey(field)) ? KendoGridExMappings[field].Path : field;
        }

        protected string MapFieldfromEntitytoViewModel(string field)
        {
            return (KendoGridExMappings != null && field != null && KendoGridExMappings.Any(m => m.Value.Path == field)) ? KendoGridExMappings.First(kvp => kvp.Value.Path == field).Key : field;
        }

        protected virtual TViewModel Map(TEntity entity)
        {
            return AutoMapperConfig.Mapper.Map<TViewModel>(entity);
        }

        protected virtual ICollection<TViewModel> ProjectToList(IQueryable<TEntity> queryableEntities)
        {
            return queryableEntities.ProjectTo<TViewModel>(AutoMapperConfig.MapperConfiguration).ToList();
        }

        protected virtual IQueryable<TViewModel> ProjectToQueryable(IQueryable<TEntity> queryableEntities)
        {
            return queryableEntities.ProjectTo<TViewModel>(AutoMapperConfig.MapperConfiguration);
        }

        /// <summary>
        /// Map a ViewModel to Entity
        /// </summary>
        /// <param name="viewModel">The ViewModel</param>
        /// <returns>Entity</returns>
        protected virtual TEntity Map(TViewModel viewModel)
        {
            return Map(viewModel, null);
        }

        /// <summary>
        /// Map a ViewModel to Entity
        /// </summary>
        /// <param name="viewModel">The ViewModel</param>
        /// <param name="entity">The Entity. If present then update, else create new.</param>
        /// <returns>Entity</returns>
        protected virtual TEntity Map(TViewModel viewModel, TEntity entity)
        {
            return entity == null ?
                AutoMapperConfig.Mapper.Map<TEntity>(viewModel) :
                AutoMapperConfig.Mapper.Map(viewModel, entity);
        }
        #endregion

        protected virtual TViewModel GetDefaultViewModel()
        {
            return new TViewModel();
        }

        protected virtual TEntity GetById(long id)
        {
            return Service.GetById(id);
        }

        #region MVC Actions
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(long id)
        {
            var entity = GetById(id);
            var viewModel = Map(entity);

            return View(viewModel);
        }

        public ActionResult Edit(long id)
        {
            var entity = GetById(id);
            var viewModel = Map(entity);

            return View(viewModel);
        }

        [HttpPost]
        public virtual ActionResult Edit(TViewModel viewModel)
        {
            try
            {
                ModelState.Clear();

                var entity = GetById(viewModel.Id);
                entity = Map(viewModel, entity);
                var result = Validate(entity, ValidationRuleSets.Edit);

                if (result.IsValid)
                {
                    Service.Update(entity);

                    return RedirectToAction("Index");
                }

                AddToModelState(result, ModelState);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.ToString());
            }

            return View(viewModel);
        }

        public virtual ViewResult Create()
        {
            var viewModel = GetDefaultViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(TViewModel viewModel)
        {
            try
            {
                ModelState.Clear();

                var entity = Map(viewModel);
                var result = Validate(entity, ValidationRuleSets.Create);

                if (result.IsValid)
                {
                    Service.Insert(entity);

                    return RedirectToAction("Index");
                }

                AddToModelState(result, ModelState);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.ToString());
            }

            return View(viewModel);
        }

        public ActionResult Delete(long id)
        {
            var entity = GetById(id);
            var viewModel = Map(entity);

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Delete(TViewModel viewModel)
        {
            try
            {
                var entity = GetById(viewModel.Id);
                Service.Delete(entity);

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.ToString());
            }

            return View(viewModel);
        }
        #endregion

        #region Validation
        protected void AddToModelState(ValidationResult validationResult, ModelStateDictionary modelState)
        {
            foreach (var error in validationResult.Errors)
            {
                var found = Mappings.FirstOrDefault(mappings => mappings.Value.Any(s => s.Path == error.PropertyName));
                var key = found.Key ?? error.PropertyName;
                modelState.AddModelError(key, error.ErrorMessage);
            }
        }

        protected virtual ValidationResult Validate(TEntity entity, string ruleSet)
        {
            return null;
        }

        protected JsonResult JsonValidationResult(ValidationResult result)
        {
            if (!result.IsValid)
            {
                var message = result.Errors.Select(e => e.ErrorMessage).FirstOrDefault();
                return Json(message, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region AutoComplete
        public virtual IQueryable GetAutoComplete(KendoGridMvcRequest request)
        {
            // Get filter from KendoGridRequest (in case of kendoAutoComplete there is only 1 filter)
            var filter = request.FilterObjectWrapper.FilterObjects.First();

            // Change the field-name in the filter from ViewModel to Entity
            string fieldOriginal = filter.Field1;
            filter.Field1 = MapFieldfromViewModeltoEntity(filter.Field1);

            // Query the database with the filter
            var query = Service.AsQueryable().Where(filter.GetExpression1<TEntity>());

            // Apply paging if needed
            if (request.PageSize != null)
            {
                query = query.Take(request.PageSize.Value);
            }

            // Do a linq dynamic query GroupBy to get only unique results
            var groupingQuery = query.GroupBy(string.Format("it.{0}", filter.Field1), string.Format("new (it.{0} as Key)", filter.Field1));

            // Make sure to return new objects which are defined as { "FieldName" : "Value" }, { "FieldName" : "Value" } else the kendoAutoComplete will not display search results.
            return groupingQuery.Select(string.Format("new (Key as {0})", fieldOriginal));
        }

        public virtual JsonResult GetAutoCompleteAsJson(KendoGridMvcRequest request)
        {
            var results = GetAutoComplete(request);
            return Json(results, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region JsonResult
        protected new JsonResult Json(object data, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                //ContentType = "application/json",
                Data = data,
                JsonRequestBehavior = behavior
            };
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding)
        {
            return new JsonNetResult
            {
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                Data = data,
            };
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                Data = data,
                JsonRequestBehavior = behavior
            };
        }
        #endregion
    }
}