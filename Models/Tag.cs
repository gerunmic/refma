using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Refma.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public String TagName { get; set; }

        public virtual ICollection<LangElementGroup> LangElementGroups { get; set; }
    }
}