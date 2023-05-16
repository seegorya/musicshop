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
    public class ordersController : Controller
    {
        private salondtbEntities db = new salondtbEntities();

        // GET: orders
        public ActionResult Index()
        {
            var order = db.order.Include(o => o.customers1);
            if (User.Identity.Name!= "administrator@mozzhhevelnik.ru")
            {
                var customers = from s in db.customers
                                select s;
                var a = User.Identity.Name;
                var id = -1;
                foreach (var ss in customers)
                {
                    if (a == ss.login)
                    {
                        id = ss.id;
                        break;
                    }
                }

                var orders = from or in db.order
                             select or;
                orders = order.Where(or => or.userid.Equals(id));
                return View(orders.ToList());
            }
            else
                 return View(order.ToList());
        }

        // GET: orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            
            return View(order);
        }

        // GET: orders/Create
        public ActionResult Create()
        {
            ViewBag.userid = new SelectList(db.customers, "id", "login");
            return View();
        }

        // POST: orders/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,userid,data,summ")] order order)
        {
            if (ModelState.IsValid)
            {
                DateTime dt = DateTime.Now;
                string dat = dt.ToShortDateString();
                order.data = dat;
                db.order.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.userid = new SelectList(db.customers, "id", "login", order.userid);
            return View(order);
        }

        // GET: orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.userid = new SelectList(db.customers, "id", "login", order.userid);
            return View(order);
        }

        // POST: orders/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,userid,data,summ")] order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.userid = new SelectList(db.customers, "id", "login", order.userid);
            return View(order);
        }

        // GET: orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            order order = db.order.Find(id);
            db.order.Remove(order);
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
