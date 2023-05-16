using mozzhhevelnik.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace mozzhhevelnik.Controllers
{
    public class CartController : Controller
    {
        private salondtbEntities db = new salondtbEntities();

        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = GetCart(),
                ReturnUrl = returnUrl
            });
        }

        public RedirectToRouteResult AddToCart(int iid, string returnUrl)
        {
            disks disk = db.disks
                .FirstOrDefault(g => g.id == iid);

            if (disk != null)
            {
                GetCart().AddItem(disk, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(int iid, string returnUrl)
        {
            disks disk = db.disks
                .FirstOrDefault(g => g.id == iid);

            if (disk != null)
            {
                GetCart().RemoveLine(disk);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public ViewResult Checkout()
        {
            return View(new order());
        }

        [HttpPost]
        public ViewResult Checkout(CartIndexViewModel model)
        {
            if (GetCart().Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Your cart is empty!");
            }

            if (ModelState.IsValid)
            {
                var order = new order();
                TryUpdateModel(order);
                try
                {
                    var claimsIdentity = User.Identity as ClaimsIdentity;
                    if (claimsIdentity != null)
                    {
                        var userIdClaim = claimsIdentity.Claims
                            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                        if (userIdClaim != null)
                        {
                            var userIdValue = userIdClaim.Value;
                        }

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
                        order.userid = id;

                        DateTime dt = DateTime.Now;
                        string dtt = dt.ToShortDateString();
                        order.data = dtt;

                        db.order.Add(order);
                        db.SaveChanges();
                        int lid = order.id;

                        double summ = 0;
                        foreach (var line in GetCart().Lines)
                        {
                            double total = line.disk.price * line.Quantity;
                            summ += total;
                            var delt = from del in db.supplyment
                                    select del;
                            foreach (var de in delt)
                            {
                                if (line.disk.id == de.cdid)
                                {
                                    if (de.amount > 0)
                                        de.amount -= line.Quantity;
                                    if (Convert.ToDouble(de.summ) > 0)
                                    {
                                        double sm = Convert.ToDouble(de.summ);
                                        sm -= total;
                                        de.summ = sm.ToString();
                                    }
                                }
                            }
                            var info = db.PlacePrder(lid, line.disk.id, line.Quantity, total);
                            db.SaveChanges();
                        }
                        var ins = db.InSumm(lid, summ);
                        db.SaveChanges();
                    }
                        GetCart().Clear();
                    return View("Completed");
                }
                catch
                {
                    //Invalid - redisplay with errors
                    return View(order);
                }               
            }
            else
            {
                return View("Something went wrong. Try again.");
            }
        }

        public Cart GetCart()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }
    }
}