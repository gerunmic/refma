using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Refma.Models
{
    public class Sentence
    {
        public int ID { get; set; }
        public int LangId { get; set; }
        [ForeignKey("LangId")]
        public virtual Lang Lang { get; set; }
        [Required, MinLength(1), MaxLength(450)]
        public string Pattern { get; set; }

    }
}