using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Refma.Models
{
    public class ViewArticleElement
    {
        public int LangElementId { get; set; }
        public string Value { get; set; }
        public Knowledge Knowledge { get; set; }
        public bool IsNotAWord { get; set; }
        public List<string> Translations { get; set; }

        public ViewArticleElement()
        {
            this.Translations = new List<string>();
        }
    }
}