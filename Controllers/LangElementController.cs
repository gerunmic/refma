using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Refma.Models;
using System.Web.Configuration;
using PagedList;

namespace Refma.Controllers
{
    public class LangElementController : Controller
    {


        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /LangElement/
        public ActionResult Index(string langId, string currentFilter, string searchString, int? page)
        {
            int defaultPageSize = Int32.Parse(WebConfigurationManager.AppSettings["defaultPageSize"]);

            var filteredLangElements = from r in db.LangElements select r;

            ViewBag.LangList = (from ul in filteredLangElements
                                select ul.Lang).Distinct();

            if (!String.IsNullOrEmpty(langId))
            {
                int filterLangId = Int32.Parse(langId);
                filteredLangElements = db.LangElements.Where(s => s.LangId == filterLangId);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                filteredLangElements = filteredLangElements.Where(s => s.Value.Contains(searchString));
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
            ViewBag.CurrentLangId = langId;

            int pageNumber = (page ?? 1);

            return View(filteredLangElements.OrderBy(e => e.LangId).ThenBy(e => e.Value).ToPagedList(pageNumber, defaultPageSize));
        }

        // GET: /LangElement/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LangElement langelement = db.LangElements.Find(id);
            if (langelement == null)
            {
                return HttpNotFound();
            }
            return View(langelement);
        }


        // GET: /LangElement/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /LangElement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,LangId,Value,Occurrency")] LangElement langelement)
        {
            if (ModelState.IsValid)
            {

                db.LangElements.Add(langelement);

                UserLangElement userElement = new UserLangElement();
                userElement.UserId = User.Identity.GetUserId();
                userElement.LangElementId = langelement.ID;

                db.UserLangElements.Add(userElement);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(langelement);
        }

        // GET: /LangElement/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LangElement langelement = db.LangElements.Find(id);
            if (langelement == null)
            {
                return HttpNotFound();
            }
            return View(langelement);
        }

        // POST: /LangElement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,LangId,Value,Occurrency")] LangElement langelement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(langelement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(langelement);
        }

        // GET: /LangElement/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LangElement langelement = db.LangElements.Find(id);
            if (langelement == null)
            {
                return HttpNotFound();
            }
            return View(langelement);
        }

        // POST: /LangElement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LangElement langelement = db.LangElements.Find(id);
            db.LangElements.Remove(langelement);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public JsonResult UpdateElementJSON(int elementid, int knowledge)
        {
            string currentUserId = User.Identity.GetUserId();
            UserLangElement ue = db.UserLangElements.Where(u => u.LangElementId == elementid && u.UserId == currentUserId).FirstOrDefault();
            if (ue != null)
            {
                ue.Knowledge = (Knowledge)knowledge;
            }
            else
            {
                ue = new UserLangElement();
                ue.UserId = currentUserId;
                ue.LangElementId = elementid;
                ue.Knowledge = (Knowledge)knowledge;

            }
            db.UserLangElements.AddOrUpdate(ue);
            db.SaveChanges();
            return Json(ue, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExportJSON(int? langId)
        {

            var langElements = db.LangElements.Include(u => u.Lang);

            if (langId.HasValue)
            {
                langElements = langElements.Where(u => u.LangId == langId);
            }

            var jsonObjects = langElements.Select(n => new { Language = n.Lang.Code, Text = n.Value });

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
