using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TStore.Domain.Entities;
using System.Threading.Tasks;

namespace TStore.Domain.Abstract
{
    public interface ISpidersRepository
    {
        IQueryable<Spider> Spiders { get; }
        void SaveSpider(Spider spider);

        Spider DeleteSpider(int spiderID);
    }
}
