using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TStore.Domain.Entities
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(Spider spider, int quantity) {
           CartLine line = lineCollection
               .Where(p => p.Spider.SpiderId == spider.SpiderId)
               .FirstOrDefault();

           if( line == null){
               lineCollection.Add(new CartLine{ Spider = spider,
                   Quantity = quantity});
           }else{
               line.Quantity += quantity;
        }
      }
             public void RemoveLine(Spider spider)
             {
                 
                  lineCollection.RemoveAll(l => l.Spider.SpiderId == spider.SpiderId);

             }


        public decimal ComputeTotalValue() { 
            return lineCollection.Sum(e => e.Spider.Price * e.Quantity);
        
        }

        public void Clear() {
            lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }

    public class CartLine {
        public Spider Spider {get; set;}
        public int Quantity {get; set;}
                     
        }
    }
