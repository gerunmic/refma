using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Refma.Models;
using Microsoft.AspNet.Identity;
using PagedList;

using System.Web.Configuration;

namespace Refma.Controllers
{
    public class UserLangElementController : Controller
    {

        
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /UserLangElement/
        public ActionResult Index(string currentFilter, string searchString, int? knowledge, int? page)
        {
            string userId = User.Identity.GetUserId();
            if (userId == null)
            {
                return View(); // return nothing
            }
            
            int defaultPageSize = Int32.Parse(WebConfigurationManager.AppSettings["defaultPageSize"]);

            ApplicationUser currentUser = db.Users.Single(u => u.Id == userId);
            var userlangelements = db.UserLangElements.Include(u => u.LangElement).Include(u => u.User).Where(u => u.UserId == userId);

            if (currentUser.TargetLangId != null)
            {
                userlangelements = userlangelements.Where(u => u.LangElement.LangId == currentUser.TargetLangId);
            }

            ViewBag.TargetLangId = currentUser.TargetLangId;
            ViewBag.CountUnknown = userlangelements.Where(u => u.Knowledge == Knowledge.Unknown).Count();
            ViewBag.CountSeen = userlangelements.Where(u => u.Knowledge == Knowledge.Seen).Count();
            ViewBag.CountDifficult = userlangelements.Where(u => u.Knowledge == Knowledge.Difficult).Count();
            ViewBag.CountKnown = userlangelements.Where(u => u.Knowledge == Knowledge.Known).Count();
            ViewBag.Knowledge = knowledge;

            if (!String.IsNullOrEmpty(searchString))
            {
                userlangelements = userlangelements.Where(s => s.LangElement.Value.Contains(searchString));
            }

            
            if (knowledge.HasValue)
            {
                userlangelements = userlangelements.Where(u => u.Knowledge == (Knowledge)knowledge);
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

            return View(userlangelements.OrderBy(s => s.LangElement.Value).ToPagedList(pageNumber, defaultPageSize));
        }

        // GET: /UserLangElement/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserLangElement userlangelement = db.UserLangElements.Find(id);
            if (userlangelement == null)
            {
                return HttpNotFound();
            }
            return View(userlangelement);
        }

        // GET: /UserLangElement/Create
        public ActionResult Create()
        {
            ViewBag.LangElementId = new SelectList(db.LangElements, "ID", "Value");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: /UserLangElement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,LangElementId,Knowledge,Occurency")] UserLangElement userlangelement)
        {
            if (ModelState.IsValid)
            {
                db.UserLangElements.Add(userlangelement);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LangElementId = new SelectList(db.LangElements, "ID", "Value", userlangelement.LangElementId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", userlangelement.UserId);
            return View(userlangelement);
        }

        // GET: /UserLangElement/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserLangElement userlangelement = db.UserLangElements.Find(id);
            if (userlangelement == null)
            {
                return HttpNotFound();
            }
            ViewBag.LangElementId = new SelectList(db.LangElements, "ID", "Value", userlangelement.LangElementId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", userlangelement.UserId);
            return View(userlangelement);
        }

        // POST: /UserLangElement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,LangElementId,Knowledge,Occurency")] UserLangElement userlangelement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userlangelement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LangElementId = new SelectList(db.LangElements, "ID", "Value", userlangelement.LangElementId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", userlangelement.UserId);
            return View(userlangelement);
        }

        // GET: /UserLangElement/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserLangElement userlangelement = db.UserLangElements.Find(id);
            if (userlangelement == null)
            {
                return HttpNotFound();
            }
            return View(userlangelement);
        }

        // POST: /UserLangElement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            UserLangElement userlangelement = db.UserLangElements.Find(id);
            db.UserLangElements.Remove(userlangelement);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult ExportJSON(int? langId, int? knowledge)
        {
            string userId = User.Identity.GetUserId();
            var userlangelements = db.UserLangElements.Include(u => u.LangElement).Where(u => u.UserId == userId);

            if (langId.HasValue)
            {
                userlangelements = userlangelements.Where(u => u.LangElement.LangId == langId);
            }

            if (knowledge.HasValue)
            {
                userlangelements = userlangelements.Where(u => u.Knowledge == (Knowledge)knowledge);
            }

            var jsonObjects = userlangelements.Select(n => new { Language = n.LangElement.Lang.Code, Text = n.LangElement.Value  });

            return Json(jsonObjects, JsonRequestBehavior.AllowGet);
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
