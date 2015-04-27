using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Refma.Models
{
    public class WebArticleElement
    {
        [Key, Column(Order = 0)]
        public int WebArticleId { get; set; }
        [ForeignKey("WebArticleId")]
        public virtual WebArticle WebArticle { get; set; }

        [Key, Column(Order = 1)]
        public int LangElementId { get; set; }
        [ForeignKey("LangElementId")]
        public virtual LangElement LangElement { get; set; }
    }
}
