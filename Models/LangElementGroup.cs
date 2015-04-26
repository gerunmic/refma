using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Refma.Models
{
    public class LangElementGroup
    {
        public LangElementGroup()
        {
            Tags = new List<Tag>();
        }

        [Key, Column(Order = 0)]
        public int LexemeId { get; set; }
        [ForeignKey("LexemeId")]
        public virtual LangElement Lexeme {get;set;}

        [Key, Column(Order = 1)]
        public int LangElementId { get; set; }
        [ForeignKey("LangElementId")]
        public virtual LangElement LangElement { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }

       
    }
}