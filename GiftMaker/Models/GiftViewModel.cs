using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web.Hosting;
using ImageMagick;

namespace GiftMaker.Models
{
    public class GiftViewModel
    {
        [Required]
        [Display(Name = "Message")]
        public string Message { get; set; }
    }
}
