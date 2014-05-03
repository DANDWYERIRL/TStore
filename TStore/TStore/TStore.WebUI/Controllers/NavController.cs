using TStore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        private ISpidersRepository repository;

        public NavController(ISpidersRepository repo)
        {
            repository = repo;
        }

        public PartialViewResult Menu(string sex = null) {

            ViewBag.SelectedSex = sex;

            IEnumerable<string> sexs = repository.Spiders
                                            .Select(x => x.Sex)
                                            .Distinct()
                                            .OrderBy(x => x);
            return PartialView(sexs);
        }


    }
}
