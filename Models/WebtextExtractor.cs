using System.Net;
using System.Text;
using System.Collections.Generic;

namespace Refma.Models
{
    public class WebtextExtractor
    {


        private static string ExtractSource(string URL)
        {
            string htmlContent;

            using (var wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                htmlContent = wc.DownloadString(URL);
            }
            return htmlContent;

        }

        public static string ExtractTextOnly(string URL)
        {
            string htmlContent = ExtractSource(URL);

            string plainText = NBoilerpipe.Extractors.ArticleExtractor.INSTANCE.GetText(htmlContent);

            return plainText;
        }

    }
}