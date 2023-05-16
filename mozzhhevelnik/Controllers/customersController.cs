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
    public class customersController : Controller
    {
        private salondtbEntities db = new salondtbEntities();

        // GET: customers
        public ActionResult Index(string sortOrder, string searchString)
        {
            if (User.Identity.Name == "administrator@mozzhhevelnik.ru")
            {
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.ContSortParm = sortOrder == "Cont" ? "cont_desc" : "Cont";
                ViewBag.DelvSortParm = sortOrder == "Delv" ? "delv_desc" : "Delv";
                var customers = from s in db.customers
                                select s;
                if (!String.IsNullOrEmpty(searchString))
                {
                    customers = customers.Where(s => s.login.Contains(searchString)
                                           || s.contacts.Contains(searchString));
                }

                
                switch (sortOrder)
                {
                    case "name_desc":
                        customers = customers.OrderByDescending(s => s.login);
                        break;
                    case "Cont":
                        customers = customers.OrderBy(s => s.contacts);
                        break;
                    case "con_desc":
                        customers = customers.OrderByDescending(s => s.contacts);
                        break;
                    case "Delv":
                        customers = customers.OrderBy(s => s.delivery);
                        break;
                    case "del_desc":
                        customers = customers.OrderByDescending(s => s.delivery);
                        break;
                    default:
                        customers = customers.OrderBy(s => s.login);
                        break;
                }
                return View(customers.ToList());
            }
            else
            {
                var cust = from s in db.customers
                           select s;
                int id = -1;
                foreach (var ss in cust)
                {
                    if (User.Identity.Name == ss.login)
                    {
                        id = ss.id;
                        break;
                    }
                }
                cust = cust.Where(or => or.id.Equals(id));
                return View(cust.ToList());
            }            
        }


        // GET: customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            customers customers = db.customers.Find(id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            return View(customers);
        }

        // GET: customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: customers/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,login,contacts,delivery,password")] customers customers)
        {
            if (ModelState.IsValid)
            {
                db.customers.Add(customers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customers);
        }

        // GET: customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            customers customers = db.customers.Find(id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            return View(customers);
        }

        // POST: customers/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,login,contacts,delivery,password")] customers customers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customers);
        }

        // GET: customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            customers customers = db.customers.Find(id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            return View(customers);
        }

        // POST: customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            customers customers = db.customers.Find(id);
            db.customers.Remove(customers);
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
