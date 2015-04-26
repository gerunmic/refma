using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.AspNet.Identity;

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

            this.dic = new Dictionary<string, LangElement>(StringComparer.InvariantCultureIgnoreCase);

            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                var userELements = db.UserLangElements.Where(e => e.UserId == this.article.UserId);
                var allElements = db.LangElements.Where(e => e.LangId == this.article.LangId);

                foreach (var e in userELements.ToList<UserLangElement>())
                {
                    dicUser.Add(e.LangElementId, e);
                }

                foreach (var e in allElements.ToList<LangElement>())
                {
                    dic.Add(e.Value, e);
                }
            }

        }

        public List<ViewArticleElement> ExtractElements()
        {
            List<ViewArticleElement> viewElements = new List<ViewArticleElement>();
            string[] allStrings = WebtextPreparer.ExtractStringElements(article.PlainText);
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
                    else
                    {
                        ue = new UserLangElement();
                        using (ApplicationDbContext db = new ApplicationDbContext())
                        {
                            ue.LangElementId = element.ID;
                            ue.UserId = article.UserId;
                            ue.Occurency = 1;
                            ue.Knowledge = Knowledge.Unknown;
                            db.UserLangElements.Add(ue);
                            db.SaveChanges(); // saved
                            dicUser.Add(ue.LangElementId, ue);
                        }
                    }
                    viewElements.Add(new ViewArticleElement() { LangElementId = ue.LangElementId, Value = str, Knowledge = ue.Knowledge });
                }
                else
                {
                    if (WebtextPreparer.isIgnoredTextElement(str)) // special character or space
                    {
                        viewElements.Add(new ViewArticleElement() { IsNotAWord = true, Value = str });
                    }
                    else
                    {
                        LangElement e = new LangElement();
                        UserLangElement u = new UserLangElement();

                        using (ApplicationDbContext db = new ApplicationDbContext())
                        {
                            e.LangId = article.LangId;
                            e.Value = str;

                            u.LangElement = e;
                            u.UserId = article.UserId;
                            u.Knowledge = Knowledge.Unknown;

                            db.LangElements.Add(e);
                            db.UserLangElements.Add(u);
                            db.SaveChanges();

                            dic.Add(e.Value, e);
                            dicUser.Add(e.ID, u);
                        }

                        viewElements.Add(new ViewArticleElement() { LangElementId = u.LangElementId, Value = str, Knowledge = u.Knowledge });
                    }
                }
            }
            return viewElements;
        }




    }
}