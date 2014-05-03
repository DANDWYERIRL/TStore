using TStore.Domain.Abstract;
using TStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TStore.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private ISpidersRepository repository;

        public AdminController(ISpidersRepository repo) 
        {
            repository = repo;
        }

        public ViewResult Index() {
            return View(repository.Spiders);
        
        }

        public ViewResult Create() {
            return View("Edit", new Spider());
        }

        public ViewResult Edit(int spiderId) {
            Spider spider = repository.Spiders
                .FirstOrDefault(p => p.SpiderId == spiderId);
            return View(spider);
        }
        [HttpPost]
        public ActionResult Edit(Spider spider, HttpPostedFileBase image) {
            if (ModelState.IsValid)
            {
                if(image != null){
                    spider.ImageMimeType = image.ContentType;
                    spider.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(spider.ImageData, 0, image.ContentLength);               
                }


                repository.SaveSpider(spider);
                TempData["message"] = string.Format("{0} has been saved", spider.CommonName);
                return RedirectToAction("Index");
            }
            else {
                return View(spider);
            }
        
        }
        [HttpPost]
        public ActionResult Delete(int spiderId)
        {
            Spider deletedSpider = repository.DeleteSpider(spiderId);
            if (deletedSpider != null)
            {
                TempData["message"] = string.Format("{0} has been deleted", deletedSpider.CommonName);

            }
            return RedirectToAction("Index");
        }
    }
}
