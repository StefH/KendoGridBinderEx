using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation.Results;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Service.Interface;
using KendoGridBinderEx.Examples.Business.Validation;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    public abstract class BaseController<TEntity, TViewModel> : Controller
        where TEntity : class, IEntity, new()
        where TViewModel : class, IEntity, new()
    {
        protected readonly IBaseService<TEntity> Service;
        protected readonly Dictionary<string, string> KendoGridExMappings;
        protected readonly Dictionary<string, List<string>> Mappings = new Dictionary<string, List<string>>();

        protected BaseController(IBaseService<TEntity> service)
        {
            Service = service;

            KendoGridExMappings = KendoGridEx<TEntity, TViewModel>.GetModelMappings();

            if (KendoGridExMappings != null)
            {
                foreach (var mapping in KendoGridExMappings)
                {
                    Mappings.Add(mapping.Key, new List<string> { mapping.Value });
                }

                foreach (var mapping in KendoGridExMappings.Where(m => m.Value.Contains('.')))
                {
                    Mappings[mapping.Key].Add(mapping.Value.Split('.').First());
                }
            }
        }

        protected virtual IQueryable<TEntity> GetQueryable()
        {
            return Service.AsQueryable();
        }

        #region AutoMapper
        protected string MapFieldfromViewModeltoEntity(string field)
        {
            return (KendoGridExMappings != null && field != null && KendoGridExMappings.ContainsKey(field)) ? KendoGridExMappings[field] : field;
        }

        protected string MapFieldfromEntitytoViewModel(string field)
        {
            return (KendoGridExMappings != null && field != null && KendoGridExMappings.ContainsValue(field)) ? KendoGridExMappings.First(kvp => kvp.Value == field).Key : field;
        }

        protected virtual TViewModel Map(TEntity entity)
        {
            return Mapper.Map<TViewModel>(entity);
        }

        protected virtual ICollection<TViewModel> ProjectToList(IQueryable<TEntity> queryableEntities)
        {
            return queryableEntities.Project().To<TViewModel>().ToList();
        }

        protected virtual IQueryable<TViewModel> ProjectToQueryable(IQueryable<TEntity> queryableEntities)
        {
            return queryableEntities.Project().To<TViewModel>();
        }

        protected virtual TEntity Map(TViewModel viewModel)
        {
            return Mapper.Map<TEntity>(viewModel);
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

        protected async virtual Task<TEntity> GetByIdAsync(long id)
        {
            return await Service.GetByIdAsync(id);
        }

        #region MVC Actions
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Details(long id)
        {
            var entity = await GetByIdAsync(id);
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

                var entity = Map(viewModel);
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
                var found = Mappings.FirstOrDefault(mappings => mappings.Value.Any(s => s == error.PropertyName));
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
        public virtual IQueryable GetAutoComplete(KendoGridRequest request)
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

        public virtual JsonResult GetAutoCompleteAsJson(KendoGridRequest request)
        {
            var results = GetAutoComplete(request);
            return Json(results, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}