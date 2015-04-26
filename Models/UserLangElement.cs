using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Refma.Models
{
    public class UserLangElement
    {
        [Key, Column(Order=0)]
        public string UserId {get; set;}
        [ForeignKey("UserId")]
        public virtual ApplicationUser User {get;set;}

        [Key, Column(Order=1)]
        public int LangElementId { get; set; }
        [ForeignKey("LangElementId")]
        public virtual LangElement LangElement { get; set; }

        [DefaultValue(Knowledge.Known)]
        public Knowledge Knowledge { get; set; }
        public int Occurency { get; set; }
    }
}