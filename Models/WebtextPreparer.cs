using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Diagnostics;


namespace Refma.Models
{
    public class WebtextPrepareResult
    {
        public WebArticle article { get; set; }
        public List<LangElement> elements { get; set; }


        public WebtextPrepareResult()
        {
            this.elements = new List<LangElement>();
        }

        public void AddLangElement(LangElement element)
        {
            if (!this.elements.Any(c => c.Equals(element)))
            {
                this.elements.Add(element);
            }
        }
    }

    public class WebtextPreparer
    {

        private List<LangElement> cache;
        private WebArticle Article { get; set; }

        public WebtextPreparer(WebArticle article)
        {
            this.Article = article;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
               cache =  db.LangElements.Where(e => e.Lang.ID == article.LangId).ToList();
            }
            
        }

        public LangElement AnalyzeTextElement(String textElement)
        {
            LangElement element = new LangElement();
            element.LangId = this.Article.LangId;
            element.Value = textElement;
            return findLangElement(element);

        }


        public static string[] ExtractStringElements(string source)
        {
            string[] stringElements = Regex.Split(source, SpecialCharactersClass.getSplitPattern());
            return stringElements;
        }

        public static string[] ExtractWordElementsOnly(string source)
        {
            string[] stringElements = Regex.Split(source, SpecialCharactersClass.getNonLetterPattern());
            return stringElements;
        }

        public String[] ExtractStringElements()
        {
            return ExtractStringElements(Article.PlainText);
        }

        public WebtextPrepareResult PrepareArticle()
        {
            WebtextPrepareResult prepResult = new WebtextPrepareResult();

            string[] stringElements = ExtractWordElementsOnly(Article.PlainText);
            stringElements = stringElements.Distinct(StringComparer.OrdinalIgnoreCase).Select(s => s.Trim()).Where(x => !string.IsNullOrEmpty(x) && Regex.IsMatch(x, @"^[\p{L}]+$")).ToArray();

            foreach (string el in stringElements)
            {

                if (cache.Any(e => e.Value.Equals(el, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }

            
             //   if (!isIgnoredTextElement(el))
             //   {
                    LangElement l = AnalyzeTextElement(el);
                    prepResult.AddLangElement(l); // Todo: do not add duplicates
              //  }

            }
            Article.SourceText = Article.PlainText.Replace("\n", "<br/>");
            prepResult.article = Article;
            return prepResult;
        }


        public bool isIgnoredTextElement(LangElement element)
        {
            return isIgnoredTextElement(element.Value);
        }

        public  static bool isIgnoredTextElement(String element) {
             return element.Trim().Length == 0 || Regex.IsMatch(element, SpecialCharactersClass.getSplitPattern());
        }

        public LangElement findLangElement(LangElement element)
        {
       
                LangElement foundElement = null;
                foundElement = cache.Where(f => f.Equals(element)).FirstOrDefault();

                if (foundElement == null)
                {
                    cache.Add(element);
                    return element;
                }
                
                return foundElement;

        }
    }
}