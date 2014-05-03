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
    public class PagingTests
    {
        [TestMethod]
        public void Can_Paginate()
        {

            Mock<ISpidersRepository> mock = new Mock<ISpidersRepository>();
            mock.Setup(m => m.Spiders).Returns(new Spider[]{
                new Spider {SpiderId = 1, CommonName="S1"},
                new Spider {SpiderId = 2, CommonName="S2"},
                new Spider {SpiderId = 3, CommonName="S3"},
                new Spider {SpiderId = 4, CommonName="S4"},
                new Spider {SpiderId = 5, CommonName="S5"},                           
            }.AsQueryable());
            SpiderController controller = new SpiderController(mock.Object);
            controller.PageSize = 3;

            SpidersListViewModel result = (SpidersListViewModel)controller.List(null, 2).Model;

            Spider[] spidArray = result.Spiders.ToArray();
            Assert.IsTrue(spidArray.Length == 2);
            Assert.AreEqual(spidArray[0].CommonName, "S4");
            Assert.AreEqual(spidArray[1].CommonName, "S5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            HtmlHelper myHelper = null;
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            Assert.AreEqual(result.ToString(), @"<a href=""Page1"">1</a>"
            + @"<a class=""selected"" href=""Page2"">2</a>" + @"<a href=""Page3"">3</a>");
        }

        [TestMethod]
        public void Can_Send_Paginatation_View_Model()
        {

            Mock<ISpidersRepository> mock = new Mock<ISpidersRepository>();
            mock.Setup(m => m.Spiders).Returns(new Spider[]{
                new Spider {SpiderId = 1, CommonName="S1"},
                new Spider {SpiderId = 2, CommonName="S2"},
                new Spider {SpiderId = 3, CommonName="S3"},
                new Spider {SpiderId = 4, CommonName="S4"},
                new Spider {SpiderId = 5, CommonName="S5"},                           
            }.AsQueryable());

            SpiderController controller = new SpiderController(mock.Object);
            controller.PageSize = 3;

            SpidersListViewModel result = (SpidersListViewModel)controller.List(null, 2).Model;

            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Spiders()
        {
            Mock<ISpidersRepository> mock = new Mock<ISpidersRepository>();
            mock.Setup(m => m.Spiders).Returns(new Spider[]{
                new Spider {SpiderId = 1, CommonName="S1", Sex="Female"},
                new Spider {SpiderId = 2, CommonName="S2",Sex="Male"},
                new Spider {SpiderId = 3, CommonName="S3",Sex="Female"},
                new Spider {SpiderId = 4, CommonName="S4",Sex="Male"},
                new Spider {SpiderId = 5, CommonName="S5",Sex="Unsexed"},                           
            }.AsQueryable());

            SpiderController controller = new SpiderController(mock.Object);
            controller.PageSize = 3;

            Spider[] result = ((SpidersListViewModel)controller.List("Male", 1).Model).Spiders.ToArray();

            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].CommonName == "S2" && result[0].Sex == "Male");
            Assert.IsTrue(result[1].CommonName == "S4" && result[1].Sex == "Male");
        }

        [TestMethod]
        public void Can_Create_Sexes()
        {
            Mock<ISpidersRepository> mock = new Mock<ISpidersRepository>();
            mock.Setup(m => m.Spiders).Returns(new Spider[]{
                new Spider {SpiderId = 1, CommonName="S1", Sex="Female"},
                new Spider {SpiderId = 3, CommonName="S2",Sex="Female"},
                new Spider {SpiderId = 4, CommonName="S3",Sex="Male"},
                new Spider {SpiderId = 5, CommonName="S4",Sex="Unsexed"},                           
            }.AsQueryable());

            NavController target = new NavController(mock.Object);

            string[] results = ((IEnumerable<string>)target.Menu().Model).ToArray();

            Assert.AreEqual(results.Length, 3);
            Assert.AreEqual(results[0], "Female");
            Assert.AreEqual(results[1], "Male");
            Assert.AreEqual(results[2], "Unsexed");

        }

        [TestMethod]
        public void Indicates_Selected_Sex()
        {
            Mock<ISpidersRepository> mock = new Mock<ISpidersRepository>();
            mock.Setup(m => m.Spiders).Returns(new Spider[]{
                new Spider {SpiderId = 1, CommonName="S1", Sex="Female"},
                new Spider {SpiderId = 4, CommonName="S2",Sex="Male"},                           
            }.AsQueryable());

            NavController target = new NavController(mock.Object);

            string sexToSelect = "Female";

            string result = target.Menu(sexToSelect).ViewBag.SelectedSex;

            Assert.AreEqual(sexToSelect, result);

        }


        [TestMethod]
        public void Generate_Sex_Specific_Spider_Count()
        {
            Mock<ISpidersRepository> mock = new Mock<ISpidersRepository>();
            mock.Setup(m => m.Spiders).Returns(new Spider[]{
                new Spider {SpiderId = 1, CommonName="S1", Sex="Female"},
                new Spider {SpiderId = 2, CommonName="S2",Sex="Male"},
                new Spider {SpiderId = 3, CommonName="S3",Sex="Female"},
                new Spider {SpiderId = 4, CommonName="S4",Sex="Male"},
                new Spider {SpiderId = 5, CommonName="S5",Sex="Unsexed"},                              
            }.AsQueryable());

            SpiderController target = new SpiderController(mock.Object);
            target.PageSize = 3;

            int res1 = ((SpidersListViewModel)target.List("Female").Model).PagingInfo.TotalItems;
            int res2 = ((SpidersListViewModel)target.List("Male").Model).PagingInfo.TotalItems;
            int res3 = ((SpidersListViewModel)target.List("Unsexed").Model).PagingInfo.TotalItems;
            int resAll = ((SpidersListViewModel)target.List(null).Model).PagingInfo.TotalItems;

            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }

    }
}