﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Refma.Models
{
    public class RawTranslationResponse
    {
        public int Id { get; set; }


        public int LangElementId { get; set; }
        [ForeignKey("LangElementId")]
        public LangElement LangElement { get; set; }

        public int LangId { get; set; }
        [ForeignKey("LangId")]
        public Lang Lang { get; set; }

        public String Provider { get; set; }
        public String Response { get; set; }
    }
}