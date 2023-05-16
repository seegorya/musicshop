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
    public class ordetailsController : Controller
    {
        private salondtbEntities db = new salondtbEntities();

        // GET: ordetails
        public ActionResult Index(int id)
        {
            var ordetails = db.ordetails.Include(o => o.disks).Include(o => o.order);

            var ordt = from od in db.ordetails
                       select od;
            ordt = ordt.Where(od => od.ordid.Equals(id));
            return View(ordt.ToList());
            //return View(ordetails.ToList());
        }

        // GET: ordetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ordetails ordetails = db.ordetails.Find(id);
            if (ordetails == null)
            {
                return HttpNotFound();
            }
            return View(ordetails);
        }

        // GET: ordetails/Create
        public ActionResult Create()
        {
            ViewBag.cdid = new SelectList(db.disks, "id", "name");
            ViewBag.ordid = new SelectList(db.order, "id", "data");
            return View();
        }

        // POST: ordetails/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ordid,cdid,amount,price")] ordetails ordetails)
        {
            if (ModelState.IsValid)
            {
                db.ordetails.Add(ordetails);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cdid = new SelectList(db.disks, "id", "name", ordetails.cdid);
            ViewBag.ordid = new SelectList(db.order, "id", "data", ordetails.ordid);
            return View(ordetails);
        }

        // GET: ordetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ordetails ordetails = db.ordetails.Find(id);
            if (ordetails == null)
            {
                return HttpNotFound();
            }
            ViewBag.cdid = new SelectList(db.disks, "id", "name", ordetails.cdid);
            ViewBag.ordid = new SelectList(db.order, "id", "data", ordetails.ordid);
            return View(ordetails);
        }

        // POST: ordetails/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ordid,cdid,amount,price")] ordetails ordetails)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ordetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cdid = new SelectList(db.disks, "id", "name", ordetails.cdid);
            ViewBag.ordid = new SelectList(db.order, "id", "data", ordetails.ordid);
            return View(ordetails);
        }

        // GET: ordetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ordetails ordetails = db.ordetails.Find(id);
            if (ordetails == null)
            {
                return HttpNotFound();
            }
            return View(ordetails);
        }

        // POST: ordetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ordetails ordetails = db.ordetails.Find(id);
            db.ordetails.Remove(ordetails);
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
