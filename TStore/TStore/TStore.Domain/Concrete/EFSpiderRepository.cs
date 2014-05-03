using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TStore.Domain.Entities;
using TStore.Domain.Abstract;

namespace TStore.Domain.Concrete{
   public class EFSpiderRepository : ISpidersRepository{
       private EFDbContext context = new EFDbContext();

       public IQueryable<Spider> Spiders {
           get { return context.Spiders; }
       }

       public void SaveSpider(Spider spider)
       {
           if (spider.SpiderId == 0)
           {
               context.Spiders.Add(spider);
           }
           else {
               Spider dbEntry = context.Spiders.Find(spider.SpiderId);
               if (dbEntry != null) {
                   dbEntry.CommonName = spider.CommonName;
                   dbEntry.LatinName = spider.LatinName;
                   dbEntry.Description = spider.Description;
                   dbEntry.Sex = spider.Sex;
                   dbEntry.Size = spider.Size;
                   dbEntry.Price = spider.Price;
                   dbEntry.ImageData = spider.ImageData;
                   dbEntry.ImageMimeType = spider.ImageMimeType;
               
               }    
           }
           context.SaveChanges();
       }
       public Spider DeleteSpider(int spiderID) {
           Spider dbEntry = context.Spiders.Find(spiderID);
           if (dbEntry != null) {
               context.Spiders.Remove(dbEntry);
               context.SaveChanges();
           }
           return dbEntry;
       }
    }
}
