using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using WebShop.Healpers;
using WebShop.Models;
using WebShop.Models.Entities;
using WebShop.ViewModels;

namespace WebShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;// = new ApplicationDbContext();
        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public ActionResult Index(string category, string search, string sortBy, int? page)
        {
            //instantiate a new view model 
            ProductIndexViewModel viewModel = new ProductIndexViewModel();


            var products = _context.Products.Include(p => p.Category);


            if (!String.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name.Contains(search) ||
                p.Description.Contains(search) ||
                p.Category.Name.Contains(search));
                viewModel.Search = search;
            }
            //group search results into categories and count how many items in each category 
            viewModel.CatsWithCount = from matchingProducts in products
                               where matchingProducts.CategoryId != null
                               group matchingProducts by
                               matchingProducts.Category.Name into
                               catGroup
                               select new CategoryWithCount()
                               {
                                  CategoryName = catGroup.Key,
                                  ProductCount = catGroup.Count()
                               };


            if (!String.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category.Name == category);
                viewModel.Category = category;
            }

            // sort the results
            switch (sortBy)
            {
                case "price_lowest":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "price_highest":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                default:
                    products = products.OrderBy(p => p.Name);
                    break;
            }

            int currentPage = (page ?? 1);
            viewModel.Products = products.ToPagedList(currentPage, Constants.PageItems);
            viewModel.SortBy = sortBy;
            viewModel.Sorts = new Dictionary<string, string>
            {
                { "Price low to high", "price_lowest" },
                { "Price high to low", "price_highest" }
            };

            return View(viewModel);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _context.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Price,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ProductAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    Product product = new Product()
                    {
                        Name = model.Name,
                        Price = model.Price,
                        Description = model.Description,
                        CategoryId=model.CategoryId
                    };
                    _context.Products.Add(product);
                    for (int i = 0; i < model.DescriptionImages.Count(); i++)
                    {
                        var temp = model.DescriptionImages[i];
                        if (temp != null)
                        {
                            _context.ProductDescriptionImages
                                .FirstOrDefault(t => t.Name == temp)
                                .ProductId = product.Id;
                        }

                    }
                    _context.SaveChanges();
                    scope.Complete();
                }
                    return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", model.CategoryId);
            return View(model);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _context.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Price,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(product).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }
        //Буде чистити фотки на сайті
        public static void ClearImages()
        {
            var context= new ApplicationDbContext();
            var listImages = context.ProductDescriptionImages
                .Where(p => p.ProductId == null).ToList();
            foreach (var item in listImages)
            {
                try
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        string path = System.Web.Hosting.HostingEnvironment
                            .MapPath(Constants.ProductDescriptionPath);
                        string image = path + item.Name;
                        context.ProductDescriptionImages.Remove(item);
                        context.SaveChanges();

                        if (System.IO.File.Exists(image))
                        {
                            System.IO.File.Delete(image);
                        }
                        scope.Complete();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }
        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _context.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = _context.Products.Find(id);
            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ContentResult CreateAjax([Bind(Include = "Id,Name,Description,Price,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();
            }
            string json = JsonConvert.SerializeObject(new
            {
                product
            });

            return Content(json, "application/json");
        }

        [HttpGet]
        public ContentResult SearchByNameJson(string name)
        {

            var products = _context.Products
                .Include(p => p.Category)
                .Where(p => p.Name.Contains(name) ||
                p.Description.Contains(name) ||
                p.Category.Name.Contains(name))
                .Select(p=>new ProductSearchViewModel
                {
                    Id=p.Id,
                    Name=p.Name,
                    CategoryName=p.Category.Name
                }).ToList();


            string json = JsonConvert.SerializeObject(new
            {
                products
            });

            return Content(json, "application/json");
        }
        [HttpPost]
        public JsonResult UploadImageDecription(HttpPostedFileBase file)
        {
            string link = string.Empty;
            string filename = Guid.NewGuid().ToString() + ".jpg";
            string image = Server.MapPath(Constants.ProductDescriptionPath) + filename;
            try
            {
                // The Complete method commits the transaction. If an exception has been thrown,
                // Complete is not  called and the transaction is rolled back.
                Bitmap imgCropped = new Bitmap(file.InputStream);
                var saveImage = ImageWorker.CreateImage(imgCropped, 450, 450);
                if (saveImage == null)
                    throw new Exception("Error save image");
                saveImage.Save(image, ImageFormat.Jpeg);
                link = Url.Content(Constants.ProductDescriptionPath) + filename;
                ProductDescriptionImage pImage = new ProductDescriptionImage()
                {
                    Name = filename
                };
                _context.ProductDescriptionImages.Add(pImage);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                if (System.IO.File.Exists(image))
                {
                    System.IO.File.Delete(image);
                }
            }

            return Json(new { link, filename });
        }

        [HttpPost]
        public JsonResult DeleteImageDecription(string src)
        {
            string link = string.Empty;
            string filename = Path.GetFileName(src);
            string image = Server.MapPath(Constants.ProductDescriptionPath) +
                filename;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var pdImage = _context
                        .ProductDescriptionImages
                        .SingleOrDefault(p => p.Name == filename);
                    if (pdImage != null)
                    {
                        _context.ProductDescriptionImages.Remove(pdImage);
                        _context.SaveChanges();
                    }
                    //throw new Exception("Галяк");
                    if (System.IO.File.Exists(image))
                    {
                        System.IO.File.Delete(image);
                    }
                    scope.Complete();
                }
            }
            catch
            {
                filename = string.Empty;
            }

            return Json(new { filename });
        }
    }

}
