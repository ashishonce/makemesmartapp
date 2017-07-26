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
    public class ReviewScoresController : Controller
    {
        private makemesmarterContext db = new makemesmarterContext();

        // GET: ReviewScores
        public async Task<ActionResult> Index()
        {
            return View(await db.ReviewScores.ToListAsync());
        }

        // GET: ReviewScores/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReviewScore reviewScore = await db.ReviewScores.FindAsync(id);
            if (reviewScore == null)
            {
                return HttpNotFound();
            }
            return View(reviewScore);
        }

        // GET: ReviewScores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReviewScores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ReviewerID,FileType,Score")] ReviewScore reviewScore)
        {
            if (ModelState.IsValid)
            {
                db.ReviewScores.Add(reviewScore);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(reviewScore);
        }

        // GET: ReviewScores/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReviewScore reviewScore = await db.ReviewScores.FindAsync(id);
            if (reviewScore == null)
            {
                return HttpNotFound();
            }
            return View(reviewScore);
        }

        // POST: ReviewScores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ReviewerID,FileType,Score")] ReviewScore reviewScore)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reviewScore).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(reviewScore);
        }

        // GET: ReviewScores/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReviewScore reviewScore = await db.ReviewScores.FindAsync(id);
            if (reviewScore == null)
            {
                return HttpNotFound();
            }
            return View(reviewScore);
        }

        // POST: ReviewScores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ReviewScore reviewScore = await db.ReviewScores.FindAsync(id);
            db.ReviewScores.Remove(reviewScore);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> GetReviewerScore(string fileType)
        {
            IList<DisplayReviewScore> model = new List<DisplayReviewScore>();
            if (!string.IsNullOrWhiteSpace(fileType) && !fileType.Equals("All"))
                {
                var comentsByFileType = db.CommentThreads.Where(x => x.FileType.Equals(fileType)).ToList();
                var reviewersByUsefulCommentsAndSentimentsValue = comentsByFileType.GroupBy(o => o.CommentInitiator)
                    .Select(g => new { reviewer = g.Key, totalUseful = g.Sum(i => i.IsUseful), totalSentiment = g.Sum(i => i.SentimentValue) , count = g.Count() });
                var count = reviewersByUsefulCommentsAndSentimentsValue.Count();
                foreach (var group in reviewersByUsefulCommentsAndSentimentsValue)
                {
                    var score = ((group.totalUseful * 10 + group.totalSentiment * 2) / 12)/ group.count;
                    model.Add(new DisplayReviewScore { Alias = group.reviewer, Score = Math.Round(score, 2) });
                }
            }
            else 
            {
                var reviewersByUsefulCommentsAndSentimentsValue = db.CommentThreads.GroupBy(o => o.CommentInitiator)
                    .Select(g => new { reviewer = g.Key, totalUseful = g.Sum(i => i.IsUseful), totalSentiment = g.Sum(i => i.SentimentValue) , count = g.Count() });
                var count = reviewersByUsefulCommentsAndSentimentsValue.Count();
                foreach (var group in reviewersByUsefulCommentsAndSentimentsValue)
                {
                    var score = ((group.totalUseful * 10 + group.totalSentiment * 2) / 12) / group.count ;
                    
                    model.Add(new DisplayReviewScore { Alias = group.reviewer, Score = Math.Round(score, 2) });
                }
            }

           var sortedModel = model.OrderByDescending(x => x.Score).ToList();
            ViewData["Model"] = sortedModel;
            ViewData["filetype"] = fileType;
            return View("ReviewerScore");
        }

        public async Task<ActionResult> GetTopReviewers(string fileType)
        {
            if (!string.IsNullOrWhiteSpace(fileType) && !fileType.Equals("default"))
            {
                var finalscore = (from rd in db.ReviewerDatas
                                  join rs in db.ReviewScores on rd.ReviewerID equals rs.ReviewerID
                                  where rs.FileType.Contains(fileType)
                                  orderby rs.Score descending
                                  select new DisplayReviewScore
                                  {
                                      Alias = rd.Alias,
                                      FirstName = rd.FirstName,
                                      LastName = rd.LastName,
                                      Score = rs.Score
                                  });
                ViewData["Model"] = finalscore;
                ViewData["filetype"] = fileType;
            }
            else
            {
                var total = db.ReviewerDatas.
                   SelectMany(rr => rr.Scores).
                   GroupBy(ri => ri.ReviewerID).
                   Select(g => new
                   {
                       ReviewerId = g.Key,

                       Score = g.Sum(ri => ri.Score)
                   });
                var finalscore = (from r in db.ReviewerDatas
                                  join t in total
                                  on r.ReviewerID equals t.ReviewerId
                                  orderby t.Score descending
                                  select new DisplayReviewScore
                                  {
                                      Alias = r.Alias,
                                      FirstName = r.FirstName,
                                      LastName = r.LastName,
                                      Score = t.Score
                                  });
                ViewData["Model"] = finalscore;
                ViewData["filetype"] = "default";
            }
            return View("ReviewerScore");
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
