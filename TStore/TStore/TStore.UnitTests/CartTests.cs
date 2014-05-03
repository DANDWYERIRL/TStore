﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TStore.Domain.Abstract;
using TStore.Domain.Entities;
using TStore.WebUI.Controllers;
using TStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TStore.WebUI.HtmlHelpers;
namespace TStore.UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {

            // Arrange - create some test products
            Spider s1 = new Spider { SpiderId = 1, CommonName = "S1" };
            Spider s2 = new Spider { SpiderId = 2, CommonName = "S2" };

            // Arrange - create a new cart
            Cart target = new Cart();

            // Act
            target.AddItem(s1, 1);
            target.AddItem(s2, 1);
            CartLine[] results = target.Lines.ToArray();

            // Assert
            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Spider, s1);
            Assert.AreEqual(results[1].Spider, s2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {

            // Arrange - create some test products
            Spider s1 = new Spider { SpiderId = 1, CommonName = "S1" };
            Spider s2 = new Spider { SpiderId = 2, CommonName = "S2" };

            // Arrange - create a new cart
            Cart target = new Cart();

            // Act
            target.AddItem(s1, 1);
            target.AddItem(s2, 1);
            target.AddItem(s1, 10);
            CartLine[] results = target.Lines.OrderBy(c => c.Spider.SpiderId).ToArray();

            // Assert
            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Quantity, 11);
            Assert.AreEqual(results[1].Quantity, 1);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {

            // Arrange - create some test products
            Spider s1 = new Spider { SpiderId = 1, CommonName = "S1" };
            Spider s2 = new Spider { SpiderId = 2, CommonName = "S2" };
            Spider s3 = new Spider { SpiderId = 3, CommonName = "S3" };

            // Arrange - create a new cart
            Cart target = new Cart();
            // Arrange - add some products to the cart
            target.AddItem(s1, 1);
            target.AddItem(s2, 3);
            target.AddItem(s3, 5);
            target.AddItem(s2, 1);

            // Act
            target.RemoveLine(s2);

            // Assert
            Assert.AreEqual(target.Lines.Where(c => c.Spider == s2).Count(), 0);
            Assert.AreEqual(target.Lines.Count(), 2);
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {

            // Arrange - create some test products
            Spider s1 = new Spider { SpiderId = 1, CommonName = "S1", Price=100 };
            Spider s2 = new Spider { SpiderId = 2, CommonName = "S2", Price= 50};

            // Arrange - create a new cart
            Cart target = new Cart();

            // Act
            target.AddItem(s1, 1);
            target.AddItem(s2, 1);
            target.AddItem(s1, 3);
            decimal result = target.ComputeTotalValue();

            // Assert
            Assert.AreEqual(result, 450);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {

            // Arrange - create some test products
            Spider s1 = new Spider { SpiderId = 1, CommonName = "S1", Price = 100m };
            Spider s2 = new Spider { SpiderId = 1, CommonName = "S2", Price = 50m };

            // Arrange - create a new cart
            Cart target = new Cart();

            // Arrange - add some items
            target.AddItem(s1, 1);
            target.AddItem(s2, 1);

            // Act - reset the cart
            target.Clear();

            // Assert
            Assert.AreEqual(target.Lines.Count(), 0);
        }

        [TestMethod]
        public void Can_Add_To_Cart()
        {

            // Arrange - create the mock repository
            Mock<ISpidersRepository> mock = new Mock<ISpidersRepository>();
            mock.Setup(m => m.Spiders).Returns(new Spider[] {
                new Spider {SpiderId = 1, CommonName = "S1", Sex = "Female"},
            }.AsQueryable());

            // Arrange - create a Cart
            Cart cart = new Cart();

            // Arrange - create the controller
            CartController target = new CartController(mock.Object, null);

            // Act - add a product to the cart
            target.AddToCart(cart, 1, null);

            // Assert
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Spider.SpiderId, 1);
        }

        [TestMethod]
        public void Adding_Spider_To_Cart_Goes_To_Cart_Screen()
        {
            // Arrange - create the mock repository
            Mock<ISpidersRepository> mock = new Mock<ISpidersRepository>();
            mock.Setup(m => m.Spiders).Returns(new Spider[] {
                new Spider {SpiderId = 1, CommonName = "S1", Sex = "Female"},
            }.AsQueryable());

            // Arrange - create a Cart
            Cart cart = new Cart();

            // Arrange - create the controller
            CartController target = new CartController(mock.Object, null);

            // Act - add a product to the cart
            RedirectToRouteResult result = target.AddToCart(cart, 2, "myUrl");

            // Assert
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            // Arrange - create a Cart
            Cart cart = new Cart();

            // Arrange - create the controller
            CartController target = new CartController(null, null);

            // Act - call the Index action method
            CartIndexViewModel result
                = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

            // Assert
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {

            // Arrange - create a mock order processor
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            // Arrange - create an empty cart
            Cart cart = new Cart();
            // Arrange - create shipping details
            ShippingDetails shippingDetails = new ShippingDetails();
            // Arrange - create an instance of the controller
            CartController target = new CartController(null, mock.Object);

            // Act
            ViewResult result = target.Checkout(cart, shippingDetails);

            // Assert - check that the order hasn't been passed on to the processor
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());
            // Assert - check that the method is returning the default view
            Assert.AreEqual("", result.ViewName);
            // Assert - check that we are passing an invalid model to the view
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {

            // Arrange - create a mock order processor
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            // Arrange - create a cart with an item
            Cart cart = new Cart();
            cart.AddItem(new Spider(), 1);

            // Arrange - create an instance of the controller
            CartController target = new CartController(null, mock.Object);
            // Arrange - add an error to the model
            target.ModelState.AddModelError("error", "error");

            // Act - try to checkout
            ViewResult result = target.Checkout(cart, new ShippingDetails());

            // Assert - check that the order hasn't been passed on to the processor
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());
            // Assert - check that the method is returning the default view
            Assert.AreEqual("", result.ViewName);
            // Assert - check that we are passing an invalid model to the view
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {
            // Arrange - create a mock order processor
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            // Arrange - create a cart with an item
            Cart cart = new Cart();
            cart.AddItem(new Spider(), 1);
            // Arrange - create an instance of the controller
            CartController target = new CartController(null, mock.Object);

            // Act - try to checkout
            ViewResult result = target.Checkout(cart, new ShippingDetails());

            // Assert - check that the order has been passed on to the processor
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Once());
            // Assert - check that the method is returning the Completed view
            Assert.AreEqual("Completed", result.ViewName);
            // Assert - check that we are passing a valid model to the view
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }

    }
}
