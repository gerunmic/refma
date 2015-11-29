using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Data.Entity;
using System.Web.Configuration;


namespace Refma.Models
{
	public class PonsDictionaryService : Refma.Models.IDictionaryService<PonsResponse>
	{
		
		public List<PonsResponse> getTranslation (String srcLang, String destLang, String word)
		{
			String jsonString = "";

			using (WebClient c = new WebClient ()) {

                c.Encoding = System.Text.Encoding.UTF8;
				Uri ponsUri = new Uri (String.Format ("https://api.pons.com/v1/dictionary?q={0}&l={1}{2}", word, srcLang, destLang));

                String secret = WebConfigurationManager.AppSettings["ponsSecret"];
				c.Headers.Add ("X-Secret", secret);
				jsonString = c.DownloadString (ponsUri);

			}

			return  JsonConvert.DeserializeObject<List<PonsResponse>> (jsonString);
		}

        public List<PonsResponse> getTranslation(ApplicationUser user, LangElement element)
        {
            // check if already requested
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
              
                RawTranslationResponse response = context.RawTranslationReponses.Include(x => x.LangElement).Where(o => o.LangId == user.LangId && o.LangElement.ID == user.TargetLangId && o.LangElementId == element.ID).FirstOrDefault();

                if (response != null)
                {
                    return JsonConvert.DeserializeObject<List<PonsResponse>>(response.Response);
                }
            }

           return this.getTranslation(user.Lang.Code, user.TargetLang.Code, element.Value);
        }

	}
}

