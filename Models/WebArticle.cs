using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Refma.Models
{
    public class WebArticle
    {
        public int ID { get; set; }
        public int LangId { get; set; }
        [ForeignKey("LangId")]
        public virtual Lang Lang { get; set; }
        public String Title { get; set; }
        public String URL { get; set; }
        public String PlainText { get; set; }
        public string UserId { get; set; }
        public double? PercentageKnown { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }


}