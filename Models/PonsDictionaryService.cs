using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;


namespace Refma.Models
{
	public class PonsDictionaryService
	{
		
		public List<PonsResponse> getTranslation (String srcLang, String destLang, String word)
		{
			String jsonString = "";

			using (WebClient c = new WebClient ()) {


				Uri ponsUri = new Uri (String.Format ("https://api.pons.com/v1/dictionary?q={0}&l={1}{2}", word, srcLang, destLang));

				c.Headers.Add ("X-Secret", "fd0b5986eb267935a3bce0891b2c91598484f69b041936cf5f08c9bd4024ebe3");
				jsonString = c.DownloadString (ponsUri);

			}

			return  JsonConvert.DeserializeObject<List<PonsResponse>> (jsonString);
		}


	}
}

