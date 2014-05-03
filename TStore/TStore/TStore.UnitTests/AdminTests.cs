using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Spiders()
        {
            // Arrange - create the mock repository
            Mock<ISpidersRepository> mock = new Mock<ISpidersRepository>();
            mock.Setup(m => m.Spiders).Returns(new Spider[] {
                new Spider {SpiderId = 1, CommonName="S1"},
                new Spider {SpiderId = 2, CommonName="S2"},
                new Spider {SpiderId = 3, CommonName="S3"},
            }.AsQueryable());

            // Arrange - create a controller 
            AdminController target = new AdminController(mock.Object);


            // Action
            Spider[] result = ((IEnumerable<Spider>)target.Index().ViewData.Model).ToArray();

            // Assert
            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual("S1", result[0].CommonName);
            Assert.AreEqual("S2", result[1].CommonName);
            Assert.AreEqual("S3", result[2].CommonName);
        }


        [TestMethod]
        public void Can_Edit_Spider()
        {

            // Arrange - create the mock repository
            Mock<ISpidersRepository> mock = new Mock<ISpidersRepository>();
            mock.Setup(m => m.Spiders).Returns(new Spider[] {
                new Spider {SpiderId = 1, CommonName="S1"},
                new Spider {SpiderId = 2, CommonName="S2"},
                new Spider {SpiderId = 3, CommonName="S3"},
            }.AsQueryable());

            // Arrange - create the controller
            AdminController target = new AdminController(mock.Object);

            // Act
            Spider s1 = target.Edit(1).ViewData.Model as Spider;
            Spider s2 = target.Edit(2).ViewData.Model as Spider;
            Spider s3 = target.Edit(3).ViewData.Model as Spider;

            // Assert
            Assert.AreEqual(1, s1.SpiderId);
            Assert.AreEqual(2, s2.SpiderId);
            Assert.AreEqual(3, s3.SpiderId);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Spider()
        {

            // Arrange - create the mock repository
            Mock<ISpidersRepository> mock = new Mock<ISpidersRepository>();
            mock.Setup(m => m.Spiders).Returns(new Spider[] {
                new Spider {SpiderId = 1, CommonName="S1"},
                new Spider {SpiderId = 2, CommonName="S2"},
                new Spider {SpiderId = 3, CommonName="S3"},
            }.AsQueryable());

            // Arrange - create the controller
            AdminController target = new AdminController(mock.Object);

            // Act
            Spider result = (Spider)target.Edit(4).ViewData.Model;

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {

            // Arrange - create mock repository
            Mock<ISpidersRepository> mock = new Mock<ISpidersRepository>();
            // Arrange - create the controller
            AdminController target = new AdminController(mock.Object);
            // Arrange - create a product
            Spider spider = new Spider { CommonName = "Test" };

            // Act - try to save the product
            ActionResult result = target.Edit(spider, null);

            // Assert - check that the repository was called
            mock.Verify(m => m.SaveSpider(spider));
            // Assert - check the method result type
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {

            // Arrange - create mock repository
            Mock<ISpidersRepository> mock = new Mock<ISpidersRepository>();
            // Arrange - create the controller
            AdminController target = new AdminController(mock.Object);
            // Arrange - create a product
            Spider spider = new Spider { CommonName = "Test" };
            // Arrange - add an error to the model state
            target.ModelState.AddModelError("error", "error");

            // Act - try to save the product
            ActionResult result = target.Edit(spider, null);

            // Assert - check that the repository was not called
            mock.Verify(m => m.SaveSpider(It.IsAny<Spider>()), Times.Never());
            // Assert - check the method result type
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Delete_Valid_Spiders()
        {

            // Arrange - create a Product
            Spider spid = new Spider { SpiderId = 2, CommonName = "Test" };

            // Arrange - create the mock repository
            Mock<ISpidersRepository> mock = new Mock<ISpidersRepository>();
            mock.Setup(m => m.Spiders).Returns(new Spider[] {
                new Spider {SpiderId = 1, CommonName = "S1"},
                spid,
                new Spider {SpiderId = 3, CommonName = "S3"},
            }.AsQueryable());

            // Arrange - create the controller
            AdminController target = new AdminController(mock.Object);

            // Act - delete the product
            target.Delete(spid.SpiderId);

            // Assert - ensure that the repository delete method was 
            // called with the correct Product 
            mock.Verify(m => m.DeleteSpider(spid.SpiderId));
        }
    }
}