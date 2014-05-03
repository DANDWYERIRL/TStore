using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TStore.Domain.Entities;

namespace TStore.WebUI.Binders
{
    public class CartModelBinder : IModelBinder
    {
        private const string sessionKey = "Cart";

        public object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext){

        //get the cart from the session 
                Cart cart = (Cart)controllerContext.HttpContext.Session[sessionKey];
        // create the cart if there wasnt one in the session data
        if (cart == null){
            cart = new Cart();
            controllerContext.HttpContext.Session[sessionKey] = cart;
        }
        return cart;
        }
    }
}