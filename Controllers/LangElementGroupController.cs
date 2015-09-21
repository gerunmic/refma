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
    public class LangElementGroupController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /LangElementGroup/
        public ActionResult Index()
        {
            var langelementgroups = db.LangElementGroups.Include(l => l.LangElement).Include(l => l.Lexeme);
            return View(langelementgroups.ToList());
        }

        // GET: /LangElementGroup/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LangElementGroup langelementgroup = db.LangElementGroups.Find(id);
            if (langelementgroup == null)
            {
                return HttpNotFound();
            }
          
            return View(langelementgroup);
        }

        // GET: /LangElementGroup/Create
        public ActionResult Create()
        {
            String currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.Where(u => u.Id == currentUserId).FirstOrDefault();
            var langElements = db.LangElements.Where(e => e.LangId == currentUser.TargetLangId);
            ViewBag.LangElementId = new SelectList(langElements.ToList(), "ID", "Value");
            ViewBag.LexemeId = new SelectList(langElements.ToList(), "ID", "Value");
            return View();
        }

        // POST: /LangElementGroup/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="LexemeId,LangElementId")] LangElementGroup langelementgroup)
        {
            if (ModelState.IsValid)
            {
                db.LangElementGroups.Add(langelementgroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LangElementId = new SelectList(db.LangElements, "ID", "Value", langelementgroup.LangElementId);
            ViewBag.LexemeId = new SelectList(db.LangElements, "ID", "Value", langelementgroup.LexemeId);
            return View(langelementgroup);
        }

        // GET: /LangElementGroup/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LangElementGroup langelementgroup = db.LangElementGroups.Find(id);
            if (langelementgroup == null)
            {
                return HttpNotFound();
            }
            ViewBag.LangElementId = new SelectList(db.LangElements, "ID", "Value", langelementgroup.LangElementId);
            ViewBag.LexemeId = new SelectList(db.LangElements, "ID", "Value", langelementgroup.LexemeId);
            return View(langelementgroup);
        }

        // POST: /LangElementGroup/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="LexemeId,LangElementId")] LangElementGroup langelementgroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(langelementgroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LangElementId = new SelectList(db.LangElements, "ID", "Value", langelementgroup.LangElementId);
            ViewBag.LexemeId = new SelectList(db.LangElements, "ID", "Value", langelementgroup.LexemeId);
            return View(langelementgroup);
        }

        // GET: /LangElementGroup/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LangElementGroup langelementgroup = db.LangElementGroups.Find(id);
            if (langelementgroup == null)
            {
                return HttpNotFound();
            }
            return View(langelementgroup);
        }

        // POST: /LangElementGroup/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LangElementGroup langelementgroup = db.LangElementGroups.Find(id);
            db.LangElementGroups.Remove(langelementgroup);
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
