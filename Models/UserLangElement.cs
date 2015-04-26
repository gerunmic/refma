using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public Knowledge Knowledge { get; set; }
        public int Occurency { get; set; }
    }
}