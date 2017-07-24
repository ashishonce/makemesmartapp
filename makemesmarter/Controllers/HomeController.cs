using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using makemesmarter.Models;
using System.Threading.Tasks;
using makemesmarter.Helpers;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace makemesmarter.Controllers
{
    public class HomeController : Controller
    {
        private makemesmarterContext db = new makemesmarterContext();

        // GET: Home
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Home/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // Post: Home/IsValuable/
        public bool IsValuable(CommentCategory category, CommentStatus status, int commentLength, int numUpVotes, int threadLength, bool IsCodeChange)
        {
            var weight = 0;
            weight += CommentAnalyser.NumUpVotesWeight(numUpVotes);
            weight += CommentAnalyser.StatusWeight(status);
            weight += CommentAnalyser.CodeChangeWeight(IsCodeChange);
            weight += CommentAnalyser.CategoryWeight(category);
            weight += CommentAnalyser.ThreadLengthWeight(threadLength);

            var weightedAverage = weight / 5;
            if (weightedAverage >= 30 && weightedAverage <= 54)
            {
                return true;
            }

            return false;
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,Name,Token")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Home/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Home/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,Name,Token")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Home/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> GetSuggestions(string id)
        {
            var data = await SuggestionModel.GetSuggestions(id);
            var suggestionsString = FinalSuggestionGenerator.Generate(data);
            return Json(suggestionsString, JsonRequestBehavior.AllowGet);
        }

        public async Task<string> GetSentiments(string text)
        {
            var resultString = await SentimentDetector.GetSentiment(text);
            return resultString.ToString();
        }

        public async Task<string> SendMail()
        {
            var listmail = new List<EmailAddress>();
            var mailAddressList = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("ashkuma@microsoft.com", "ashish kumar"),
                new Tuple<string, string>("nazikh@microsoft.com", "nazia khan"),
                new Tuple<string, string>("umkan@microsoft.com", "umesh kanoja"),
                new Tuple<string, string>("riniga@microsoft.com", "richa nigam")
            };

            foreach (var mailAddress in mailAddressList)
            {
                EmailAddress testemail = new EmailAddress();
                testemail.Email = mailAddress.Item1;
                testemail.Name = mailAddress.Item2;
                listmail.Add(testemail);
            }

            var content = MailContentCreator.CreatePendingCommentResponse(); ;

            await SendEmail(listmail, "Pending CR comments", content);
            this.Response.StatusCode = (int)HttpStatusCode.OK;
            return content;
        }

        private static async Task SendEmail(List<EmailAddress> recipients, string subject, string mailContent)
        {
            var client = new SendGridClient("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");
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("noreply@azure.com", "CodeFlow Stalker"),
                Subject = subject,
                HtmlContent = mailContent
            };

            msg.AddTos(recipients);
            var response = await client.SendEmailAsync(msg);
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
