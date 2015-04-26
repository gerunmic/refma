using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System;
using System.Collections.Generic;



namespace Refma.Models
{
    public class LangElement
    {
        
        public int ID { get; set; }
        [Index("IX_UQ_ELEMENT", Order = 10, IsUnique = true)]
        public int LangId { get; set; }
        [ForeignKey("LangId")]
        public virtual Lang Lang { get; set; }
        [Required, MinLength(1), MaxLength(450)]
        [Index("IX_UQ_ELEMENT", Order=20, IsUnique=true)]
        public string Value { get; set; }
        
        public int Occurrency { get; set; }

        [NotMapped]
        public bool Ignore { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is LangElement)
            {
                LangElement compareObj = (LangElement)obj;
                return this.Value.Equals(compareObj.Value, StringComparison.OrdinalIgnoreCase) &&
                       this.LangId == compareObj.LangId;
                
            }
            return false;
        }

    }

}