using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Refma.Models
{
    public class ArticleDecorator
    {
        private Dictionary<String, LangElement> dic;
        private Dictionary<int, UserLangElement> dicUser = new Dictionary<int, UserLangElement>();
        private WebArticle article = null;


        public ArticleDecorator(WebArticle article)
        {
            this.article = article;

            this.dic = new Dictionary<string, LangElement>(StringComparer.OrdinalIgnoreCase);

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var allElements = from e in db.LangElements
                                  from w in db.WebArticleElements
                                  where e.ID == w.LangElementId && w.WebArticleId == article.ID
                                  select e;
                var userElements = from u in db.UserLangElements
                                   from w in db.WebArticleElements
                                   where u.LangElementId == w.LangElementId && w.WebArticleId == article.ID
                                   select u;


                foreach (var e in userElements.ToList<UserLangElement>())
                {
                    dicUser.Add(e.LangElementId, e);
                }

                foreach (var e in allElements.ToList<LangElement>())
                {
                    dic.Add(e.Value, e);
                }
            }

        }

        public static string[] ExtractStringElements(string source)
        {
            string[] stringElements = Regex.Split(source, SpecialCharactersClass.getSplitPattern());
            return stringElements;
        }

        public List<ViewArticleElement> GetAllViewElements()
        {
            // returns all elements of the article with additional information for the view (such as if its a word, a special character, known etc.) the order must be exactly as it appears in the source article
            List<ViewArticleElement> viewElements = new List<ViewArticleElement>();
            string[] allStrings = ExtractStringElements(article.PlainText);
            foreach (var str in allStrings)
            {
                if (dic.ContainsKey(str))
                {
                    LangElement element = null;
                    UserLangElement ue = null;
                    dic.TryGetValue(str, out element);

                    if (dicUser.ContainsKey(element.ID))
                    {
                        dicUser.TryGetValue(element.ID, out ue);
                    }

                    ViewArticleElement ve = new ViewArticleElement() { LangElementId = element.ID, Value = str, Knowledge = Knowledge.Unknown };
                    if (ue != null)
                    {
                        ve.Knowledge = ue.Knowledge;
                    }

                    viewElements.Add(ve);
                }
                else
                {
                    viewElements.Add(new ViewArticleElement() { IsNotAWord = true, Value = str });
                }
            }
            return viewElements;
        }
    }
}
