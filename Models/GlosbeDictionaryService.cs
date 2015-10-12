using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Data.Entity;
using System.Web.Configuration;

namespace Refma.Models
{

    public class Phrase
    {
        public string text { get; set; }
        public string language { get; set; }
    }

    public class Meaning
    {
        public string language { get; set; }
        public string text { get; set; }
    }

    public class Tuc
    {
        public Phrase phrase { get; set; }
        public object meaningId { get; set; }
        public List<Meaning> meanings { get; set; }
    }


    public class GlosbeResponse
    {
        public string result { get; set; }
        public List<Tuc> tuc { get; set; }
        public string phrase { get; set; }
        public string from { get; set; }
        public string dest { get; set; }

    }


    public class GlosbeDictionaryService 
    {

        public GlosbeResponse getTranslation(ApplicationUser user, LangElement element)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {

                RawTranslationResponse response = context.RawTranslationReponses.Include(x => x.LangElement).Where(o => o.LangId == user.LangId && o.LangElement.ID == user.TargetLangId && o.LangElementId == element.ID).FirstOrDefault();

                if (response != null)
                {
                    return JsonConvert.DeserializeObject<GlosbeResponse>(response.Response);
                }
            }

            return this.getTranslation(user.Lang.Code, user.TargetLang.Code, element.Value);
            
        }

        public GlosbeResponse getTranslation(string srcLang, string destLang, string word)
        {
            String jsonString = "";

            using (WebClient c = new WebClient())
            {

                c.Encoding = System.Text.Encoding.UTF8;
                Uri transUri = new Uri(String.Format("https://glosbe.com/gapi/translate?from={1}&dest={0}&format=json&phrase={2}", srcLang, destLang, word));
                jsonString = c.DownloadString(transUri);

            }
            
            return JsonConvert.DeserializeObject<GlosbeResponse>(jsonString);
        }
    }
}