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
using makemesmarter.Helpers;

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
        public async Task CreateUnique([Bind(Include = "CommentId,JoinedComments,Status,ThreadCount,IsUseful,CumlativeLikes,FileType,FilePath,SentimentValue,CommentInitiator,PrAuthorId,PullRequestId")] CommentThread commentThread)
        {
            if (ModelState.IsValid)
            {
                if(db.CommentThreads.Where(x => x.CommentId.Equals(commentThread.CommentId)).ToList().Count > 0)
                {
                    await db.SaveChangesAsync();
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
        public async Task<ActionResult> Edit([Bind(Include = "Id,CommentId,JoinedComments,Status,ThreadCount,IsUseful,CumlativeLikes,FileType,FilePath,SentimentValue,PrAuthorId,PullRequestId")] CommentThread commentThread)
        {
            if (ModelState.IsValid)
            {
                var comment = db.CommentThreads.Find(commentThread.Id);
                comment.commentCategory = commentThread.commentCategory;
                db.Entry(comment).State = EntityState.Modified;
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

        public async Task DeleteAllUnique()
        {

            db.CommentThreads.RemoveRange(db.CommentThreads.AsEnumerable());
            await db.SaveChangesAsync();
            this.Response.StatusCode = (int) HttpStatusCode.OK;
            return;
        }

        public async Task<ActionResult> GetFilteredComments(string text, string author, string commenter, string fileType)
        {
            ViewData["text"] = text ?? "";
            ViewData["author"] = author ?? "";
            ViewData["commenter"] = commenter ?? "";
            if(!string.IsNullOrWhiteSpace(text))
            {
                var resultModel = db.CommentThreads.Where(x => x.JoinedComments.Contains(text)).ToList();

                if(!string.IsNullOrWhiteSpace(commenter))
                {
                    resultModel = resultModel.Where(x => x.CommentInitiator.Contains(commenter)).ToList();
                }

                if (!string.IsNullOrWhiteSpace(author))
                {
                    resultModel = resultModel.Where(x => x.PrAuthorId.Contains(author)).ToList();
                }

                resultModel.ForEach(x => x.JoinedComments = x.JoinedComments.Replace(text, "<span style='background-color:yellow'>"+ text + "</span>"));
                ViewData["Model"] = resultModel;
                return View("CommentSearch");
            }
            else{
                return View("CommentSearch");
            }
        }

        public async Task UpdateCommentUsefulnessScores()
        {
            var statusBySortedList = db.CommentThreads.GroupBy(g => g.Status).Select(t => new { count = t.Count(), key = t.Key });
            var count = db.CommentThreads.Count();
            var StatusWeight = new List<Tuple<string, double>>();
            foreach(var item in statusBySortedList)
            {
                var tempWeight = ((float)item.count) / count;
                StatusWeight.Add(new Tuple<string, double>(item.key, tempWeight));
            }

            foreach (var comment in db.CommentThreads)
            {
                    comment.IsUseful = CommentValueAnalyser.IsValuable(
                    comment.commentCategory,
                    comment.Status,
                    comment.initiatorCommentLength,
                    comment.CumlativeLikes,
                    comment.ThreadCount,
                    StatusWeight);
                    db.Entry(comment).State = EntityState.Modified;
            }

                await db.SaveChangesAsync();
                return ;
        }

        public async Task GetSentimentsForALL(string text)
        {
            //await db.SaveChangesAsync();
            var count = 0;
            foreach (var comment in db.CommentThreads)
            {
                count++;
                if (count < 100)
                    continue;
                comment.SentimentValue = await SentimentDetector.GetSentiment(comment.JoinedComments);
                db.Entry(comment).State = EntityState.Modified;
                
            }

            await db.SaveChangesAsync();
            return;
        }

        public async Task UpdateCommentCategories()
        {
            var count = 0;
             foreach (var comment in db.CommentThreads)
            {
                if(string.IsNullOrWhiteSpace(comment.commentCategory))
                    {
                    comment.commentCategory = "Design";
                    db.Entry(comment).State = EntityState.Modified;
                }
            }

            await db.SaveChangesAsync();
            return;
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
