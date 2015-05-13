using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Refma.Models
{
    public class SpecialCharactersClass
    {
        public static string getSplitPattern()
        {
            return @"([\p{P}\p{N}\p{Z}\p{S}\p{C}]+)";
            //return "([\\d\\ \\-\\.\\:\\,\\–,\\»,\\«,\\;\\(\\)\\{\\}\\[\\]\\n\\/\\\"\\|\\°\\~\\*\\&\\%\\<\\↑\\£\\>\\?])";
        }

        public static string getNonLetterPattern()
        {
            return @"[\p{P}\p{N}\p{Z}\p{S}\p{C}]+";
        }

        public static string getLetterOnlyPattern()
        {
            return @"[\p{L}]+";
        }
    }
}