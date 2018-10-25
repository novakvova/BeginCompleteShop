using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WebShop.Healpers;
using WebShop.Models;
using WebShop.Models.Entities;

namespace WebShop.Controllers
{
    public class ProductImagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductImagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProductImages
        public ActionResult Index()
        {
            return View(_context.ProductImages.OrderBy(p=>p.Id).ToList());
        }

        // GET: ProductImages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductImage productImage = _context.ProductImages.Find(id);
            if (productImage == null)
            {
                return HttpNotFound();
            }
            return View(productImage);
        }

        // GET: ProductImages/Create
        public ActionResult Upload()
        {
            return View();
        }

        // POST: ProductImages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase[] files)
        {
            bool allValid = true;
            string inValidFiles = "";
            //check the user has entered a file
            if (files[0] != null)
            {
                //if the user has entered less than ten files
                if (files.Length <= 10)
                {
                    //check they are all valid
                    foreach (var file in files)
                    {
                        if (!ValidateFile(file))
                        {
                            allValid = false;
                            inValidFiles += ", " + file.FileName;
                        }
                    }
                    //if they are all valid then try to save them to disk
                    if (allValid)
                    {
                        foreach (var file in files)
                        {
                            try
                            {
                                SaveFileToDisk(file);
                            }
                            catch (Exception)
                            {
                                ModelState.AddModelError("FileName", "Sorry an error occurred saving the files to disk, please try again");
                            }
                        }
                    }
                    //else add an error listing out the invalid files            
                    else
                    {
                        ModelState.AddModelError("FileName", "All files must be gif, png, jpeg or jpg  and less than 2MB in size.The following files" + inValidFiles +
                            " are not valid");
                    }
                }
                //the user has entered more than 10 files
                else
                {
                    ModelState.AddModelError("FileName", "Please only upload up to ten files at a time");
                }
            }
            else
            {
                //if the user has not entered a file return an error message
                ModelState.AddModelError("FileName", "Please choose a file");
            }

            if (ModelState.IsValid)
            {
                bool duplicates = false;
                bool otherDbError = false;
                string duplicateFiles = "";
                foreach (var file in files)
                {
                    //try and save each file
                    var productToAdd = new ProductImage { FileName = file.FileName };
                    try
                    {
                        _context.ProductImages.Add(productToAdd);
                        _context.SaveChanges();
                    }
                    //if there is an exception check if it is caused by a duplicate file
                    catch (DbUpdateException ex)
                    {
                        SqlException innerException = ex.InnerException.InnerException as SqlException;
                        if (innerException != null && innerException.Number == 2601)
                        {
                            duplicateFiles += ", " + file.FileName;
                            duplicates = true;
                            _context.Entry(productToAdd).State = EntityState.Detached;
                        }
                        else
                        {
                            otherDbError = true;
                        }
                    }
                }
                //add a list of duplicate files to the error message
                if (duplicates)
                {
                    ModelState.AddModelError("FileName", "All files uploaded except the files" + duplicateFiles + ", which already exist in the system." +
                        " Please delete them and try again if you wish to re-add them");
                    return View();
                }
                else if (otherDbError)
                {
                    ModelState.AddModelError("FileName", "Sorry an error has occurred saving to the database, please try again");
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: ProductImages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductImage productImage = _context.ProductImages.Find(id);
            if (productImage == null)
            {
                return HttpNotFound();
            }
            return View(productImage);
        }

        // POST: ProductImages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FileName")] ProductImage productImage)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(productImage).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productImage);
        }

        // GET: ProductImages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductImage productImage = _context.ProductImages.Find(id);
            if (productImage == null)
            {
                return HttpNotFound();
            }
            return View(productImage);
        }

        // POST: ProductImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductImage productImage = _context.ProductImages.Find(id);
            if (productImage != null)
            {
                string filename = productImage.FileName;
                _context.ProductImages.Remove(productImage);
                _context.SaveChanges();
                string imageBig = Server.MapPath(Constants.ProductImagePath) + filename;
                string imageSmall = Server.MapPath(Constants.ProductThumbnailPath) + filename;
                if (System.IO.File.Exists(imageSmall))
                {
                    System.IO.File.Delete(imageSmall);
                }
                if (System.IO.File.Exists(imageBig))
                {
                    System.IO.File.Delete(imageBig);
                }
            }
            return RedirectToAction("Index");
        }

        private bool ValidateFile(HttpPostedFileBase file)
        {
            string fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();
            string[] allowedFileTypes = { ".gif", ".png", ".jpeg", ".jpg" };
            if ((file.ContentLength > 0 && file.ContentLength < 2097152) &&
                allowedFileTypes.Contains(fileExtension))
            {
                return true;
            }
            return false;
        }
        private void SaveFileToDisk(HttpPostedFileBase file)
        {
            WebImage img = new WebImage(file.InputStream);
            if (img.Width > 190)
            {
                img.Resize(190, img.Height);
            }
            img.Save(Constants.ProductImagePath + file.FileName);
            if (img.Width > 100)
            {
                img.Resize(100, img.Height);
            }
            img.Save(Constants.ProductThumbnailPath + file.FileName);
        }
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ContentResult UploadBase64(string base64image)
        {
            string filename = Guid.NewGuid().ToString() + ".jpg";
            string imageBig = Server.MapPath(Constants.ProductImagePath) + filename;
            string imageSmall = Server.MapPath(Constants.ProductThumbnailPath) + filename;
            string json = null;
            try
            {
                // The Complete method commits the transaction. If an exception has been thrown,
                // Complete is not  called and the transaction is rolled back.
                Bitmap imgCropped = base64image.FromBase64StringToBitmap();
                var saveImage = ImageWorker.CreateImage(imgCropped, 300, 300);
                if (saveImage == null)
                    throw new Exception("Error save image");

                saveImage.Save(imageBig, ImageFormat.Jpeg);

                var saveImageIcon = ImageWorker.CreateImage(imgCropped, 32, 32);
                if (saveImageIcon == null)
                    throw new Exception("Error save image");
                saveImageIcon.Save(imageSmall, ImageFormat.Jpeg);

                var productImage = new ProductImage { FileName = filename };
                _context.ProductImages.Add(productImage);
                _context.SaveChanges();

                json = JsonConvert.SerializeObject(new
                {
                    imagePath = Url.Content(Constants.ProductImagePath) + filename,
                    id = productImage.Id
                });

            }
            catch (Exception)
            {
                json = JsonConvert.SerializeObject(new
                {
                    imagePath = ""
                });
                if(System.IO.File.Exists(imageSmall))
                {
                    System.IO.File.Delete(imageSmall);
                }
                if (System.IO.File.Exists(imageBig))
                {
                    System.IO.File.Delete(imageBig);
                }
            }
            return Content(json, "application/json");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ContentResult DeleteImageAjax(int id)
        {
            string json = JsonConvert.SerializeObject(new
            {
                success = false
            });
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    ProductImage productImage = _context.ProductImages.Find(id);
                    if (productImage != null)
                    {
                        string filename = productImage.FileName;
                        _context.ProductImages.Remove(productImage);
                        _context.SaveChanges();
                        string imageBig = Server.MapPath(Constants.ProductImagePath) + filename;
                        string imageSmall = Server.MapPath(Constants.ProductThumbnailPath) + filename;
                        if (System.IO.File.Exists(imageSmall))
                        {
                            System.IO.File.Delete(imageSmall);
                        }
                        if (System.IO.File.Exists(imageBig))
                        {
                            System.IO.File.Delete(imageBig);
                        }
                        json = JsonConvert.SerializeObject(new
                        {
                            success = true
                        });
                    }
                    // The Complete method commits the transaction. If an exception has been thrown,
                    // Complete is not  called and the transaction is rolled back.
                    scope.Complete();
                }
            }
            catch
            {
            }
            return Content(json, "application/json");
        }
    }
}
