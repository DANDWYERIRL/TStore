using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TStore.Domain.Entities;
using TStore.Domain.Abstract;
using TStore.WebUI.Models;

namespace TStore.WebUI.Controllers
{
    public class SpiderController : Controller
    {
        private ISpidersRepository repository;
        public int PageSize = 4;

        public SpiderController(ISpidersRepository spiderRespository)
        {
            this.repository = spiderRespository;
        }

        public ViewResult List(string sex, int page = 1)
        {
            SpidersListViewModel model = new SpidersListViewModel
            {
                Spiders = repository.Spiders
                .Where(p => sex == null || p.Sex == sex)
                .OrderBy(p => p.SpiderId)
                .Skip((page - 1) * PageSize)
                .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = sex == null ?
                        repository.Spiders.Count() :
                        repository.Spiders.Where(e => e.Sex == sex).Count()
                },
                CurrentCategory = sex
            };

            return View(model);
        }

        public FileContentResult getImage(int spiderId)
        {
            Spider spid = repository.Spiders.FirstOrDefault(p => p.SpiderId == spiderId);
            if (spid != null)
            {
                return File(spid.ImageData, spid.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
    }
}

