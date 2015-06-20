using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Refma.Models
{
    public class LangElementTranslation
    {
        public int Id { get; set; }
       
     
        public int LangElementId { get; set; }
        [ForeignKey("LangElementId")]
        public LangElement LangElement { get; set; }

        public int LangId { get; set; }
        [ForeignKey("LangId")]
        public Lang Lang { get; set; }

        public String RawResponse { get; set; }

        public String Translation { get; set; }
    }
}