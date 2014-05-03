using TStore.Domain.Entities;
using TStore.Domain.Abstract;
using TStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private ISpidersRepository repository;
        private IOrderProcessor orderProcessor;

            public CartController(ISpidersRepository repo, IOrderProcessor proc) {

                repository = repo;
                orderProcessor = proc;
            }

            public ViewResult Index(Cart cart,string returnUrl) {
                return View(new CartIndexViewModel
                {
                    Cart = cart,
                    ReturnUrl = returnUrl
                });
            
            }

            public PartialViewResult Summary(Cart cart)
            {
                return PartialView(cart);

            }

            [HttpPost]
            public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
            {
                if (cart.Lines.Count() == 0)
                {
                    ModelState.AddModelError("", "sorry, your cart is empty");
                }

                if (ModelState.IsValid)
                {
                    orderProcessor.ProcessOrder(cart, shippingDetails);
                    cart.Clear();
                    return View("Completed");
                }
                else
                {
                    return View(shippingDetails);
                }

            }
            public ViewResult Checkout()
            {
                return View(new ShippingDetails());
            }

            public RedirectToRouteResult AddToCart(Cart cart, int spiderId, string returnUrl) {
                Spider spider = repository.Spiders
                    .FirstOrDefault(p => p.SpiderId == spiderId);

                if (spider != null)
                {
                    cart.AddItem(spider, 1);

                }
                return RedirectToAction("Index", new { returnUrl });
             }
            
            public RedirectToRouteResult RemoveFromCart(Cart cart, int spiderId, string returnUrl){
                Spider spider = repository.Spiders
                    .FirstOrDefault(p => p.SpiderId == spiderId);

                if (spider != null)
                {
                   
                    cart.RemoveLine(spider);
                }        
                return RedirectToAction("Index", new { returnUrl });
            }

                /*private Cart GetCart() {
                
                    Cart cart = (Cart)Session["Cart"];
                    if (cart == null){
                        cart= new Cart();
                        Session["Cart"] = cart;
                    }
                    return cart;            
            }*/
                
               
                
    }
}
