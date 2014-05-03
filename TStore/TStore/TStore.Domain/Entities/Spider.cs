using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TStore.Domain.Entities
{
    public class Spider
    {

        [HiddenInput(DisplayValue= false)]
        public int SpiderId { get; set; }
        
        [Required(ErrorMessage = "Please Enter a spider name")]
        public string CommonName { get; set; }

        [Required(ErrorMessage = "Please Enter a Latin name")]
        public string LatinName { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Please Enter a Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please Enter a sex")]
        public string Sex { get; set; }

        [Required(ErrorMessage = "Please Enter a size")]
        public string Size { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage="Please enter a price that is more than 0")]
        public decimal Price {get; set;}

        public byte[] ImageData { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ImageMimeType { get; set; }


    }
}
