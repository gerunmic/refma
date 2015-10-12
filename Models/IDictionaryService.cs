using System;
namespace Refma.Models
{
    interface IDictionaryService<T>
    {
        System.Collections.Generic.List<T> getTranslation(ApplicationUser user, LangElement element);
        System.Collections.Generic.List<T> getTranslation(string srcLang, string destLang, string word);
    }
}
