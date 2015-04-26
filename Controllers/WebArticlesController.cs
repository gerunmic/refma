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

namespace Refma.Controllers
{
    public class WebArticlesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /WebArticles/
        public ActionResult Index()
        {

            string userId = User.Identity.GetUserId();
            if (userId != null)
            {
                
                var userWebArticles = from e in db.WebArticles
                                      from u in db.Users 
                                      where e.UserId == userId && e.LangId == u.TargetLangId
                                      select e;
                return View(userWebArticles);
            }
            return View();// show nothing
            // return View(db.WebArticles.ToList());
        }


        public ActionResult Read(int? id)
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
            
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.Where(u => u.Id == currentUserId).FirstOrDefault();

            ArticleDecorator d = new ArticleDecorator(webarticle);
            List<ViewArticleElement> viewElements = d.ExtractElements();

            // todo: check if not too slow!
            /*
            foreach (var ve in viewElements)
            {
                if (ve.IsNotAWord == false)
                {
                   var trans =  db.LangElementTranslations.Where(t => t.LangElementId == ve.UserLangElement.LangElementId && t.LangId == currentUser.LangId).Distinct().Take(2);
                   foreach(var t in trans)
                   {
                       ve.Translations.Add(t.Translation);

                   }
                }
            }
             * */

            ViewBag.ViewElements = viewElements;
          
            return View(webarticle);
        }

        // GET: /WebArticles/Create
        public ActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Register", "Account");
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
        public ActionResult Create([Bind(Include = "ID,LangId,Title,URL")] WebArticle webarticle)
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
                    webarticle.Title = webarticle.Title + title;
                    db.WebArticles.Add(webarticle);
                    db.SaveChanges();

                    
                    WebtextPreparer wp = new WebtextPreparer(webarticle);
                    WebtextPrepareResult articleResult = wp.PrepareArticle();
                    webarticle = articleResult.article;

                    db.LangElements.AddOrUpdate<LangElement>(e => new { e.LangId, e.Value }, articleResult.elements.Distinct().ToArray());
                    db.SaveChanges();
                    foreach (LangElement e in articleResult.elements)
                    {
                        UserLangElement ue = new UserLangElement();
                        ue.UserId = userId;
                        ue.LangElementId = e.ID;
                        db.UserLangElements.AddOrUpdate<UserLangElement>(ue);
                    }
                   
                }

          
                db.SaveChanges();
                return RedirectToAction("Read", new { id = webarticle.ID });
            }
            return View(webarticle);
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
