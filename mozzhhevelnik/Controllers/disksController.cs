using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mozzhhevelnik.Models;
using System.Data.Entity.Validation;

namespace mozzhhevelnik.Controllers
{
    public class disksController : Controller
    {
        private salondtbEntities db = new salondtbEntities();

        // GET: disks
        public ActionResult Index(string sortOrder, string searchString, string searchString1)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "genre" ? "genre_desc" : "genre";
            ViewBag.TeSortParm = sortOrder == "singer" ? "singer_desc" : "singer";
            ViewBag.AteSortParm = sortOrder == "price" ? "price_desc" : "price";
            var disks = from s in db.disks
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                disks = disks.Where(s => s.name.Contains(searchString)
                                       || s.singer.Contains(searchString));
            }
            if (!String.IsNullOrEmpty(searchString1))
            {
                disks = disks.Where(s => s.genre.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    disks = disks.OrderByDescending(s => s.name);
                    break;
                case "genre":
                    disks = disks.OrderBy(s => s.genre);
                    break;
                case "genre_desc":
                    disks = disks.OrderByDescending(s => s.genre);
                    break;
                case "singer":
                    disks = disks.OrderBy(s => s.singer);
                    break;
                case "singer_desc":
                    disks = disks.OrderByDescending(s => s.singer);
                    break;
                case "price":
                    disks = disks.OrderBy(s => s.price);
                    break;
                case "price_desc":
                    disks = disks.OrderByDescending(s => s.price);
                    break;
                default:
                    disks = disks.OrderBy(s => s.name);
                    break;
            }
                    return View(disks.ToList());
        }

        // GET: disks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            disks disks = db.disks.Find(id);
            if (disks == null)
            {
                return HttpNotFound();
            }
            return View(disks);
        }

        // GET: disks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: disks/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,genre,singer,year,price,cover")] disks disks, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (upload != null && upload.ContentLength > 0)
                    {
                        using (var reader = new System.IO.BinaryReader(upload.InputStream))
                        {
                            disks.cover = reader.ReadBytes(upload.ContentLength);
                        }
                    }

                    db.disks.Add(disks);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                catch (DbEntityValidationException ex)
                {
                    string message;
                    foreach (DbEntityValidationResult validationError in ex.EntityValidationErrors)
                    {
                        message = "Object: " + validationError.Entry.Entity.ToString();

                        foreach (DbValidationError err in validationError.ValidationErrors)
                        {
                            message = message + err.ErrorMessage + "";
                        }
                    }
                }
            }

            return View(disks);
        }

        // GET: disks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            disks disks = db.disks.Find(id);
            if (disks == null)
            {
                return HttpNotFound();
            }
            return View(disks);
        }

        // POST: disks/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,genre,singer,year,price,cover")] disks disks, HttpPostedFileBase upload)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(disks).State = EntityState.Modified;
                    if (upload != null && upload.ContentLength > 0)
                    {
                        using (var reader = new System.IO.BinaryReader(upload.InputStream))
                        {
                            disks.cover = reader.ReadBytes(upload.ContentLength);
                        }
                        db.SaveChanges();
                    }

                    else
                    {
                        db.Entry(disks).Property(m => m.cover).IsModified = false;
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index");
                }

                return View(disks);
            }
            catch (Exception e) { return View(disks); }
       
        }

        // GET: disks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            disks disks = db.disks.Find(id);
            if (disks == null)
            {
                return HttpNotFound();
            }
            return View(disks);
        }

        // POST: disks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            disks disks = db.disks.Find(id);
            db.disks.Remove(disks);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
