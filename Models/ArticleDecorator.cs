using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Refma.Models
{
    public class ArticleDecorator
    {
        private Dictionary<String, LangElement> dic = new Dictionary<string, LangElement>(StringComparer.OrdinalIgnoreCase);

        public Dictionary<String, LangElement> Dic
        {
            get { return dic; }
            set { dic = value; }
        }
        private Dictionary<int, UserLangElement> dicUser = new Dictionary<int, UserLangElement>();

        public Dictionary<int, UserLangElement> DicUser
        {
            get { return dicUser; }
            set { dicUser = value; }
        }
        private WebArticle article = null;


        public ArticleDecorator(WebArticle article, Boolean readFromDatabase = true)
        {
            this.article = article;

            // this.dic = new Dictionary<string, LangElement>(StringComparer.OrdinalIgnoreCase);

            if (readFromDatabase)
            {
                ReadArticleElementsFromDatabase(article);
            }

        }

        private void ReadArticleElementsFromDatabase(WebArticle article)
        {
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
                    try
                    {
                        dicUser.Add(e.LangElementId, e);
                    }
                    catch (ArgumentException x)
                    {

                    }
                }

                foreach (var e in allElements.ToList<LangElement>())
                {
                    try
                    {
                        dic.Add(e.Value, e);
                    }
                    catch (ArgumentException x)
                    {

                    }
                }

            }
        }

        public static string[] ExtractStringElements(string source)
        {
            string[] stringElements = Regex.Split(source, SpecialCharactersClass.getSplitPattern());
            return stringElements.Where(s => s != String.Empty).ToArray();
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
