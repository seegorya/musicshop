using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mozzhhevelnik.Models;

namespace mozzhhevelnik.Controllers
{
    public class supplymentsController : Controller
    {
        private salondtbEntities db = new salondtbEntities();

        // GET: supplyments
        public ActionResult Index()
        {
            var supplyment = db.supplyment.Include(s => s.disks).Include(s => s.supplier);
            return View(supplyment.ToList());
        }

        // GET: supplyments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplyment supplyment = db.supplyment.Find(id);
            if (supplyment == null)
            {
                return HttpNotFound();
            }
            return View(supplyment);
        }

        // GET: supplyments/Create
        public ActionResult Create()
        {
            ViewBag.cdid = new SelectList(db.disks, "id", "name");
            ViewBag.supid = new SelectList(db.supplier, "id", "name");
            return View();
        }

        // POST: supplyments/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,supid,cdid,amount,summ")] supplyment supplyment)
        {
            if (ModelState.IsValid)
            {
                db.supplyment.Add(supplyment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cdid = new SelectList(db.disks, "id", "name", supplyment.cdid);
            ViewBag.supid = new SelectList(db.supplier, "id", "name", supplyment.supid);
            return View(supplyment);
        }

        // GET: supplyments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplyment supplyment = db.supplyment.Find(id);
            if (supplyment == null)
            {
                return HttpNotFound();
            }
            ViewBag.cdid = new SelectList(db.disks, "id", "name", supplyment.cdid);
            ViewBag.supid = new SelectList(db.supplier, "id", "name", supplyment.supid);
            return View(supplyment);
        }

        // POST: supplyments/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,supid,cdid,amount,summ")] supplyment supplyment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplyment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cdid = new SelectList(db.disks, "id", "name", supplyment.cdid);
            ViewBag.supid = new SelectList(db.supplier, "id", "name", supplyment.supid);
            return View(supplyment);
        }

        // GET: supplyments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplyment supplyment = db.supplyment.Find(id);
            if (supplyment == null)
            {
                return HttpNotFound();
            }
            return View(supplyment);
        }

        // POST: supplyments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            supplyment supplyment = db.supplyment.Find(id);
            db.supplyment.Remove(supplyment);
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
