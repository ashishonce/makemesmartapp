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
    public class CommentThreadsController : Controller
    {
        private makemesmarterContext db = new makemesmarterContext();

        // GET: CommentThreads
        public async Task<ActionResult> Index()
        {
            return View(await db.CommentThreads.ToListAsync());
        }

        // GET: CommentThreads/Details/5
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

        // GET: CommentThreads/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CommentThreads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CommentId,JoinedComments,Status,ThreadCount,IsUseful,CumlativeLikes,FileType,FilePath,SentimentValue,PrAuthorId,PullRequestId")] CommentThread commentThread)
        {
            if (ModelState.IsValid)
            {
                db.CommentThreads.Add(commentThread);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(commentThread);
        }

        [HttpPost]
        public async Task CreateUnique([Bind(Include = "CommentId,JoinedComments,Status,ThreadCount,IsUseful,CumlativeLikes,FileType,FilePath,SentimentValue,PrAuthorId,PullRequestId")] CommentThread commentThread)
        {
            if (ModelState.IsValid)
            {
                if(db.CommentThreads.Where(x => x.CommentId.Equals(commentThread.CommentId)).ToList().Count > 0)
                {
                    return ;
                }
                db.CommentThreads.Add(commentThread);
                await db.SaveChangesAsync();
                return ;
            }

            return ;
        }

        // GET: CommentThreads/Edit/5
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

        // POST: CommentThreads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CommentId,JoinedComments,Status,ThreadCount,IsUseful,CumlativeLikes,FileType,FilePath,SentimentValue,PrAuthorId,PullRequestId")] CommentThread commentThread)
        {
            if (ModelState.IsValid)
            {
                db.Entry(commentThread).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(commentThread);
        }

        // GET: CommentThreads/Delete/5
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

        // POST: CommentThreads/Delete/5
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
