using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Refma.Models;

namespace Refma.Controllers
{
    public class SentencesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Sentences
        public ActionResult Index()
        {
            var sentences = db.Sentences.Include(s => s.Lang);
            return View(sentences.ToList());
        }

        // GET: Sentences/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sentence sentence = db.Sentences.Find(id);
            if (sentence == null)
            {
                return HttpNotFound();
            }
            return View(sentence);
        }

        // GET: Sentences/Create
        public ActionResult Create()
        {
            ViewBag.LangId = new SelectList(db.Langs, "ID", "Code");
            return View();
        }

        // POST: Sentences/Create
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,LangId,Pattern")] Sentence sentence)
        {
            if (ModelState.IsValid)
            {
                db.Sentences.Add(sentence);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LangId = new SelectList(db.Langs, "ID", "Code", sentence.LangId);
            return View(sentence);
        }

        // GET: Sentences/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sentence sentence = db.Sentences.Find(id);
            if (sentence == null)
            {
                return HttpNotFound();
            }
            ViewBag.LangId = new SelectList(db.Langs, "ID", "Code", sentence.LangId);
            return View(sentence);
        }

        // POST: Sentences/Edit/5
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,LangId,Pattern")] Sentence sentence)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sentence).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LangId = new SelectList(db.Langs, "ID", "Code", sentence.LangId);
            return View(sentence);
        }

        // GET: Sentences/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sentence sentence = db.Sentences.Find(id);
            if (sentence == null)
            {
                return HttpNotFound();
            }
            return View(sentence);
        }

        // POST: Sentences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sentence sentence = db.Sentences.Find(id);
            db.Sentences.Remove(sentence);
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
