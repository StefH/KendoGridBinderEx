using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using FluentValidation.Results;
using KendoGridBinderEx.Examples.Business.Entities;
using KendoGridBinderEx.Examples.Business.Service;
using KendoGridBinderEx.Examples.Business.Validation;

namespace KendoGridBinderEx.Examples.MVC.Controllers
{
    public abstract class BaseController<TEntity, TViewModel> : Controller
        where TEntity : class, IEntity, new()
        where TViewModel : class, IEntity, new()
    {
        protected readonly IBaseService<TEntity> Service;
        protected readonly Dictionary<string, List<string>> Mappings = new Dictionary<string, List<string>>();

        protected BaseController(IBaseService<TEntity> service)
        {
            Service = service;

            var kendoGridMappings = KendoGridEx<TEntity, TViewModel>.GetModelMappings();

            if (kendoGridMappings != null)
            {
                foreach (var mapping in kendoGridMappings)
                {
                    Mappings.Add(mapping.Key, new List<string> { mapping.Value });
                }

                foreach (var mapping in kendoGridMappings.Where(m => m.Value.Contains('.')))
                {
                    Mappings[mapping.Key].Add(mapping.Value.Split('.').First());
                }
            }
        }

        protected virtual IQueryable<TEntity> GetQueryable()
        {
            return Service.AsQueryable();
        }

        protected virtual TEntity GetById(long id)
        {
            return Service.GetById(id);
        }

        protected virtual TViewModel Map(TEntity entity)
        {
            return Mapper.Map<TViewModel>(entity);
        }

        protected virtual TEntity Map(TViewModel viewModel)
        {
            return Mapper.Map<TEntity>(viewModel);
        }

        protected virtual TViewModel GetDefaultViewModel()
        {
            return new TViewModel();
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
    }
}