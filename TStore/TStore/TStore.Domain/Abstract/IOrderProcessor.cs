using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TStore.Domain.Entities;

namespace TStore.Domain.Abstract
{
   public interface IOrderProcessor
    {
       void ProcessOrder(Cart cart, ShippingDetails shippingDetails);
    }
}
