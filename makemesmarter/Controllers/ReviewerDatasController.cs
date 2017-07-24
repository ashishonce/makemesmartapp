using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using makemesmarter.Models;

namespace makemesmarter.Controllers
{
    public class ReviewerDatasController : Controller
    {
        private makemesmarterContext db = new makemesmarterContext();

        // GET: ReviewerDatas
        public async Task<ActionResult> Index()
        {
            return View(await db.ReviewerDatas.ToListAsync());
        }

        // GET: ReviewerDatas/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReviewerData reviewerData = await db.ReviewerDatas.FindAsync(id);
            if (reviewerData == null)
            {
                return HttpNotFound();
            }
            return View(reviewerData);
        }

        // GET: ReviewerDatas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReviewerDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ReviewerID,Alias,FirstName,LastName")] ReviewerData reviewerData)
        {
            if (ModelState.IsValid)
            {
                db.ReviewerDatas.Add(reviewerData);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(reviewerData);
        }

        // GET: ReviewerDatas/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReviewerData reviewerData = await db.ReviewerDatas.FindAsync(id);
            if (reviewerData == null)
            {
                return HttpNotFound();
            }
            return View(reviewerData);
        }

        // POST: ReviewerDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ReviewerID,Alias,FirstName,LastName")] ReviewerData reviewerData)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reviewerData).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(reviewerData);
        }

        // GET: ReviewerDatas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReviewerData reviewerData = await db.ReviewerDatas.FindAsync(id);
            if (reviewerData == null)
            {
                return HttpNotFound();
            }
            return View(reviewerData);
        }

        // POST: ReviewerDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ReviewerData reviewerData = await db.ReviewerDatas.FindAsync(id);
            db.ReviewerDatas.Remove(reviewerData);
            await db.SaveChangesAsync();
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
