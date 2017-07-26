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
    public class CommentThreadsControlController : Controller
    {
        private makemesmarterContext db = new makemesmarterContext();

        // GET: CommentThreadsControl
        public async Task<ActionResult> Index()
        {
            return View(await db.CommentThreads.Where(x => x.commentCategory.Equals("Logical", StringComparison.OrdinalIgnoreCase)).ToListAsync());
        }

        // GET: CommentThreadsControl/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommentThread commentThread = await db.CommentThreads.FindAsync(id);
            if (commentThread == null)
            {
                return HttpNotFound();
            }
            return View(commentThread);
        }

        // GET: CommentThreadsControl/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CommentThreadsControl/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,CommentId,JoinedComments,Status,ThreadCount,IsUseful,CumlativeLikes,FileType,FilePath,SentimentValue,CommentInitiator,initiatorCommentLength,commentCategory,PrAuthorId,PullRequestId")] CommentThread commentThread)
        {
            if (ModelState.IsValid)
            {
                db.CommentThreads.Add(commentThread);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(commentThread);
        }

        // GET: CommentThreadsControl/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommentThread commentThread = await db.CommentThreads.FindAsync(id);
            if (commentThread == null)
            {
                return HttpNotFound();
            }
            return View(commentThread);
        }

        // POST: CommentThreadsControl/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,CommentId,JoinedComments,Status,ThreadCount,IsUseful,CumlativeLikes,FileType,FilePath,SentimentValue,CommentInitiator,initiatorCommentLength,commentCategory,PrAuthorId,PullRequestId")] CommentThread commentThread)
        {
            if (ModelState.IsValid)
            {
                db.Entry(commentThread).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(commentThread);
        }

        // GET: CommentThreadsControl/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommentThread commentThread = await db.CommentThreads.FindAsync(id);
            if (commentThread == null)
            {
                return HttpNotFound();
            }
            return View(commentThread);
        }

        // POST: CommentThreadsControl/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CommentThread commentThread = await db.CommentThreads.FindAsync(id);
            db.CommentThreads.Remove(commentThread);
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
