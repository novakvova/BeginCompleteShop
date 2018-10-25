using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebShop.DAL.Abstract;
using WebShop.Healpers;
using WebShop.Models;
using WebShop.Models.Entities;
using WebShop.ViewModels;
using System.Data.Entity;

namespace WebShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;// = new ApplicationDbContext();

        public HomeController(IUserService userService, ApplicationDbContext context)
        {
            _userService = userService;
            _context = context;
        }
        //public ApplicationDbContext MyDbContext
        //{
        //    get
        //    {
        //        return HttpContext.GetOwinContext().Get<ApplicationDbContext>();
        //    }
        //}
        public ActionResult Index(string[] fvalues)
        {
            int[] listFilterId = new int[0];
            //int mycount = _userService.GetCountUsers();
            HomeViewModel model = new HomeViewModel();
            model.Filter = new HomeFilterViewModel();
            //Будую дерево фільтрів
            model.Filter.Filters = GetListFilters();
            if (fvalues != null)
                listFilterId = fvalues.Select(v => int.Parse(v)).ToArray();
            model.Products = GetProductsByFilter(listFilterId, model.Filter.Filters);
            model.Filter.Check = listFilterId;
            return View(model);
        }
        private List<FNameViewModel> GetListFilters()
        {
            var query = from f in _context.VFilterNameGroups.AsQueryable()
                        where f.FilterValueId != null
                        select new
                        {
                            FNameId = f.FilterNameId,
                            FName = f.FilterName,
                            FValueId = f.FilterValueId,
                            FValue = f.FilterValue
                        };
            var groupNames = from f in query
                             group f by new
                             {
                                 Id = f.FNameId,
                                 Name = f.FName
                             } into g
                             orderby g.Key.Name
                             select g;
            List<FNameViewModel> listGroupFilters =
                groupNames.Select(fn => new FNameViewModel
                {
                    Id = fn.Key.Id,
                    Name = fn.Key.Name,
                    Children = (from v in fn
                                group v by new FValueViewModel
                                {
                                    Id = v.FValueId,
                                    Name = v.FValue
                                } into g
                                select g.Key).ToList()
                }).ToList();

            
            return listGroupFilters;
        }

        private List<ProductViewModel> GetProductsByFilter(int[] values, List<FNameViewModel> filtersList)
        {
            int[] filterValueSearchList = values;
            var query = _context
                .Products
                .Include(f => f.Filters)
                .AsQueryable();

            foreach (var fName in filtersList)
            {
                int count = 0; //Кількість співпадінь у даній групі фільрів
                var predicate = PredicateBuilder.False<Product>();
                foreach (var fValue in fName.Children)
                {
                    for (int i = 0; i < filterValueSearchList.Length; i++)
                    {
                        var idV = fValue.Id;
                        if (filterValueSearchList[i] == idV)
                        {
                            predicate = predicate
                                .Or(p => p.Filters
                                    .Any(f => f.FilterValueId == idV));
                            count++;
                        }
                    }
                }
                if (count != 0)
                    query = query.Where(predicate);
            }
            var listProductSearch = query.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            }).ToList();
            return listProductSearch;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}