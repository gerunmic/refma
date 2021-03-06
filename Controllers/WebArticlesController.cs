using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Refma.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Text.RegularExpressions;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Web.Configuration;
using PagedList;
using System.ComponentModel.DataAnnotations;

namespace Refma.Controllers
{
    public class WebArticlesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /WebArticles/
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {

            string userId = User.Identity.GetUserId();
            if (userId != null)
            {
                int defaultPageSize = Int32.Parse(WebConfigurationManager.AppSettings["defaultPageSize"]);

                ApplicationUser currentUser = db.Users.First(u => u.Id == userId);
                ViewBag.LangCode = currentUser.TargetLang.Code;


                var userWebArticles = from e in db.WebArticles
                                      where e.UserId == userId && e.LangId == currentUser.TargetLangId
                                      // orderby e.ID descending
                                      select e;

                if (!String.IsNullOrEmpty(searchString))
                {
                    userWebArticles = userWebArticles.Where(s => s.Title.Contains(searchString));
                }

                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;

                int pageNumber = (page ?? 1);

                return View(userWebArticles.OrderByDescending(u => u.ID).ToPagedList(pageNumber, defaultPageSize));


            }
            return View();// show nothing
        }

        public class FrequencyModel {
            public string LangName;
            public int LangElementId;
            public string LangElementValue;
            public int Occurences;
        }

        public ActionResult Dashboard()
        {

            string userId = User.Identity.GetUserId();
            if (userId != null)
            {

                ApplicationUser currentUser = db.Users.First(u => u.Id == userId);

                int userTargetCode = currentUser.TargetLang.ID;

                var freqList = from w in db.WebArticleElements
                               join e in db.LangElements on w.LangElementId equals e.ID into temp
                            
                               from t in temp.DefaultIfEmpty()
                               where t.LangId.Equals(userTargetCode)
                               group t by new { t.Lang.Name, t.Value, w.LangElementId } into g
                               select new FrequencyModel
                               {
                                   LangName = g.Key.Name,
                                   LangElementId = g.Key.LangElementId,
                                   LangElementValue = g.Key.Value,
                                   Occurences = g.Count()
                               };


                return View(freqList.OrderByDescending(f => f.Occurences).Take(500).ToList());
            }

            return View();
        }

        public ActionResult Read(int? id) 
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            db.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
            WebArticle webarticle = db.WebArticles.Find(id);
            if (webarticle == null)
            {
                return HttpNotFound();
            }

            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.Where(u => u.Id == currentUserId).FirstOrDefault();

            ArticleDecorator decorator = new ArticleDecorator(webarticle);
            List<ViewArticleElement> viewElements = decorator.GetAllViewElements();



            ViewBag.ViewElements = viewElements;

            return View(webarticle);
        }

        // GET: /WebArticles/Create
        public ActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            string userId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.First(u => u.Id == userId);
            ViewBag.CurrentTargetLangId = currentUser.TargetLangId;

            List<Lang> langList;
            langList = db.Langs.ToList<Lang>();
            ViewBag.LangList = langList;

            return View();
        }

        // POST: /WebArticles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,LangId,Title,URL,PlainText")] WebArticle webarticle)
        {
            if (ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId();
                webarticle.UserId = userId;

                if (webarticle.URL != null)
                {
                    // first, save article to get Id
                    string title;
                    webarticle.PlainText = WebtextExtractor.ExtractTextOnly(webarticle.URL, out title);
                    webarticle.Title = System.Web.HttpUtility.HtmlDecode(webarticle.Title + title);

                    if (webarticle.Title == null)
                    {
                        webarticle.Title = webarticle.URL;
                    }

                    db.WebArticles.Add(webarticle);
                    db.SaveChanges();
                    // end saving article, id known nown

                    UpdateOrInsertBaseLangElements(webarticle);
                }
                else if (webarticle.PlainText != null)
                {
                    // manuall text
                    db.WebArticles.Add(webarticle);
                    db.SaveChanges();
                    UpdateOrInsertBaseLangElements(webarticle);

                }
                return RedirectToAction("Read", new { id = webarticle.ID });
            }
            return View(webarticle);
        }

        private void UpdateOrInsertBaseLangElements(WebArticle webarticle)
        {
            // ...update insert or update base elements

            // split text into "word"-strings to get distinct "words" of article without special characters likes spaces, line feeds etc.
            string[] splitStrings = Regex.Split(webarticle.PlainText.Replace("\n", String.Empty), SpecialCharactersClass.getNonLetterPattern()).Distinct().Where(s => s.Length >= 1).ToArray();

            // create new list and transfer plain "word" strings to LangElement-Objects
            List<LangElement> articleElements = new List<LangElement>(splitStrings.Length);
            for (int i = 0; i < splitStrings.Length; i++)
            {

                string currentString = splitStrings[i];
                // check if word exists already in database
                LangElement foundElement = db.LangElements.Where(l => l.Value.Equals(currentString, StringComparison.OrdinalIgnoreCase) && l.LangId == webarticle.LangId).FirstOrDefault();
                if (foundElement == null)
                {
                    // not found, add word to database
                    foundElement = new LangElement() { LangId = webarticle.LangId, Value = currentString };
                    db.LangElements.Add(foundElement);
                    db.SaveChanges();
                }

                // link this word also to article to reduce loading times when reading
                db.WebArticleElements.AddOrUpdate(new WebArticleElement() { WebArticleId = webarticle.ID, LangElementId = foundElement.ID });
                db.SaveChanges();

            }
        }


        // GET: /WebArticles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WebArticle webarticle = db.WebArticles.Find(id);
            if (webarticle == null)
            {
                return HttpNotFound();
            }
            return View(webarticle);
        }

        // POST: /WebArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WebArticle webarticle = db.WebArticles.Find(id);
            db.WebArticles.Remove(webarticle);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult UpdatePercentage(int id, double percentage)
        {
            string currentUserId = User.Identity.GetUserId();
            WebArticle a = db.WebArticles.Where(u => u.ID == id && u.UserId == currentUserId).FirstOrDefault();

            if (a != null)
            {
                a.PercentageKnown = percentage;
            };
            db.WebArticles.AddOrUpdate(a);
            db.SaveChanges();
            return Json(a.PercentageKnown, JsonRequestBehavior.AllowGet);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
