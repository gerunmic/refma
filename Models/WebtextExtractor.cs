using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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

        public static string ExtractTextOnly(string URL, out string title)
        {
            string htmlContent = ExtractSource(URL);

            title = GetTitle(htmlContent);

            string plainText = NBoilerpipe.Extractors.DefaultExtractor.INSTANCE.GetText(htmlContent);

            return plainText;
        }

        static string GetTitle(string htmlText)
        {
            Match m = Regex.Match(htmlText, @"<title>\s*(.+?)\s*</title>");
            if (m.Success)
            {
                return m.Groups[1].Value;
            }
            else
            {
                return "";
            }
        }

    }
}