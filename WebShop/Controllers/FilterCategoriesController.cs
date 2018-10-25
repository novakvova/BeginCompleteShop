using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebShop.Models;
using WebShop.Models.Entities;

namespace WebShop.Controllers
{
    public class FilterCategoriesController : Controller
    {
        private ApplicationDbContext _context;// = new ApplicationDbContext();
        public FilterCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FilterCategories
        public ActionResult Index()
        {
            var filterCategories = _context.FilterCategories.Include(f => f.CategoryOf).Include(f => f.FilterNameOf).Include(f => f.FilterValueOf);
            return View(filterCategories.ToList());
        }

        // GET: FilterCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilterCategory filterCategory = _context.FilterCategories.Find(id);
            if (filterCategory == null)
            {
                return HttpNotFound();
            }
            return View(filterCategory);
        }

        [HttpGet]
        public ActionResult CreateFilterName()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFilterName(FilterNameViewModel filterName)
        {
            if (ModelState.IsValid)
            {
                _context.FiltersName.Add(new FilterName { Name = filterName.Name });
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(filterName);
        }
        [HttpGet]
        public ActionResult CreateFilterValue()
        {

            ViewBag.FilterNameId = new SelectList(_context.FiltersName, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFilterValue(FilterGroupViewModel filterGroup)
        {
            if (ModelState.IsValid)
            {
                FilterValue filterValue = new FilterValue() { Name = filterGroup.FilterValue };
                _context.FiltersValue.Add(filterValue);
                _context.SaveChanges();
                _context.FilterNameGroups.Add(new FilterNameGroup { FilterNameId = filterGroup.FilterNameId, FilterValueId = filterValue.Id });
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FilterNameId = new SelectList(_context.FiltersName, "Id", "Name");
            return View(filterGroup);
        }

        // GET: FilterCategories/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name");
            ViewBag.FilterNameId = new SelectList(_context.FiltersName, "Id", "Name");
            ViewBag.FilterValueId = new SelectList(_context.FiltersValue, "Id", "Name");
            return View();
        }

        // POST: FilterCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FilterNameId,FilterValueId,CategoryId")] FilterCategory filterCategory)
        {
            if (ModelState.IsValid)
            {
                _context.FilterCategories.Add(filterCategory);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", filterCategory.CategoryId);
            ViewBag.FilterNameId = new SelectList(_context.FiltersName, "Id", "Name", filterCategory.FilterNameId);
            ViewBag.FilterValueId = new SelectList(_context.FiltersValue, "Id", "Name", filterCategory.FilterValueId);
            return View(filterCategory);
        }

        // GET: FilterCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilterCategory filterCategory = _context.FilterCategories.Find(id);
            if (filterCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", filterCategory.CategoryId);
            ViewBag.FilterNameId = new SelectList(_context.FiltersName, "Id", "Name", filterCategory.FilterNameId);
            ViewBag.FilterValueId = new SelectList(_context.FiltersValue, "Id", "Name", filterCategory.FilterValueId);
            return View(filterCategory);
        }

        // POST: FilterCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FilterNameId,FilterValueId,CategoryId")] FilterCategory filterCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(filterCategory).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", filterCategory.CategoryId);
            ViewBag.FilterNameId = new SelectList(_context.FiltersName, "Id", "Name", filterCategory.FilterNameId);
            ViewBag.FilterValueId = new SelectList(_context.FiltersValue, "Id", "Name", filterCategory.FilterValueId);
            return View(filterCategory);
        }

        // GET: FilterCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilterCategory filterCategory = _context.FilterCategories.Find(id);
            if (filterCategory == null)
            {
                return HttpNotFound();
            }
            return View(filterCategory);
        }

        // POST: FilterCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FilterCategory filterCategory = _context.FilterCategories.Find(id);
            _context.FilterCategories.Remove(filterCategory);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
