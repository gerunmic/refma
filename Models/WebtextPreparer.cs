using System.Text.RegularExpressions;


namespace Refma.Models
{
    public class WebtextPreparer
    {

        public static string[] ExtractStringElements(string source)
        {
            string[] stringElements = Regex.Split(source, SpecialCharactersClass.getSplitPattern());
            return stringElements;
        }

    }
}
