using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TStore.Domain.Entities;

namespace TStore.WebUI.Models
{
    public class SpidersListViewModel
    {
        public IEnumerable<Spider> Spiders { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
    }
}