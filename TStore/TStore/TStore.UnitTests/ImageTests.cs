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
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {

            // Arrange - create a Product with image data
            Spider spid = new Spider
            {
                SpiderId = 2,
                CommonName = "Test",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };

            // Arrange - create the mock repository
            Mock<ISpidersRepository> mock = new Mock<ISpidersRepository>();
            mock.Setup(m => m.Spiders).Returns(new Spider[] {
                new Spider {SpiderId = 1, CommonName = "S1"},
                spid,                            
                new Spider {SpiderId = 3, CommonName = "S3"}
            }.AsQueryable());

            // Arrange - create the controller
            SpiderController target = new SpiderController(mock.Object);

            // Act - call the GetImage action method
            ActionResult result = target.getImage(2);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(spid.ImageMimeType, ((FileResult)result).ContentType);
        }

        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
        {

            // Arrange - create the mock repository
            Mock<ISpidersRepository> mock = new Mock<ISpidersRepository>();
            mock.Setup(m => m.Spiders).Returns(new Spider[] {
                new Spider {SpiderId = 1, CommonName = "S1"},
                new Spider {SpiderId = 2, CommonName = "S2"}
            }.AsQueryable());

            // Arrange - create the controller
            SpiderController target = new SpiderController(mock.Object);

            // Act - call the GetImage action method
            ActionResult result = target.getImage(100);

            // Assert
            Assert.IsNull(result);
        }
    }
}
