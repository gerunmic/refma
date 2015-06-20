using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Refma.Models;
using Microsoft.AspNet.Identity;

namespace Refma.Controllers
{
    public class LangElementTranslationController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /LangElementTranslation/
        public ActionResult Index(int? langElementId)
        {
            var langelementtranslations = db.LangElementTranslations.Include(l => l.Lang).Include(l => l.LangElement);

            if (langElementId.HasValue)
            {
                langelementtranslations = langelementtranslations.Where(t => t.LangElementId == langElementId);
                ViewBag.Itemid = langElementId;
            }
            
            return View(langelementtranslations.ToList());
        }

        public ActionResult RequestTranslation(int langElementId)
        {
            String currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.Where(u => u.Id == currentUserId).FirstOrDefault();

            LangElement element = db.LangElements.Where(l => l.LangId== currentUser.TargetLangId && l.ID == langElementId).FirstOrDefault();

           // PonsDemo.PonsDictionaryService service = new PonsDemo.PonsDictionaryService();
          //  String response = service.getWordRaw(currentUser.Lang.Code, currentUser.TargetLang.Code, element.Value);


          //  LangElementTranslation trans = new LangElementTranslation() { LangElementId = element.ID, LangId = currentUser.LangId, RawResponse = response };

            //db.LangElementTranslations.Add(trans);
            //db.SaveChanges();

            return RedirectToAction("Index");

        }


        public JsonResult GetTranslations(int langElementId) {
            var langelementtranslations = db.LangElementTranslations.Include(l => l.Lang).Include(l => l.LangElement);

         
                langelementtranslations = langelementtranslations.Where(t => t.LangElementId == langElementId);
                ViewBag.Itemid = langElementId;
         

            return Json(langelementtranslations.ToList(), JsonRequestBehavior.AllowGet);
        }

        // GET: /LangElementTranslation/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LangElementTranslation langelementtranslation = db.LangElementTranslations.Find(id);
            if (langelementtranslation == null)
            {
                return HttpNotFound();
            }
            return View(langelementtranslation);
        }

        // GET: /LangElementTranslation/Create
        public ActionResult Create(int langElementId)
        {
            String currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.Where(u => u.Id == currentUserId).FirstOrDefault();
            ViewBag.LangId = new SelectList(db.Langs, "ID", "Code", currentUser.LangId); // set user-lang-id!
            ViewBag.LangElementId = new SelectList(db.LangElements.Where(e => e.ID == langElementId), "ID", "Value", langElementId);
            return View();
        }


        // POST: /LangElementTranslation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,LangElementId,LangId,Translation")] LangElementTranslation langelementtranslation)
        {
            if (ModelState.IsValid)
            {
                db.LangElementTranslations.Add(langelementtranslation);
                db.SaveChanges();

                if (Request.UrlReferrer.PathAndQuery != null)
                {
                    return Redirect(Request.UrlReferrer.PathAndQuery);
                }
                return RedirectToAction("Index");
            }

            ViewBag.LangId = new SelectList(db.Langs, "ID", "Code", langelementtranslation.LangId);
            ViewBag.LangElementId = new SelectList(db.LangElements, "ID", "Value", langelementtranslation.LangElementId);
            return View(langelementtranslation);
        }

        // GET: /LangElementTranslation/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LangElementTranslation langelementtranslation = db.LangElementTranslations.Find(id);
            if (langelementtranslation == null)
            {
                return HttpNotFound();
            }
            ViewBag.LangId = new SelectList(db.Langs, "ID", "Code", langelementtranslation.LangId);
            ViewBag.LangElementId = new SelectList(db.LangElements, "ID", "Value", langelementtranslation.LangElementId);
            return View(langelementtranslation);
        }

        // POST: /LangElementTranslation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,LangElementId,LangId,Translation")] LangElementTranslation langelementtranslation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(langelementtranslation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LangId = new SelectList(db.Langs, "ID", "Code", langelementtranslation.LangId);
            ViewBag.LangElementId = new SelectList(db.LangElements, "ID", "Value", langelementtranslation.LangElementId);
            return View(langelementtranslation);
        }

        // GET: /LangElementTranslation/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LangElementTranslation langelementtranslation = db.LangElementTranslations.Find(id);
            if (langelementtranslation == null)
            {
                return HttpNotFound();
            }
            return View(langelementtranslation);
        }

        // POST: /LangElementTranslation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LangElementTranslation langelementtranslation = db.LangElementTranslations.Find(id);
            db.LangElementTranslations.Remove(langelementtranslation);
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
