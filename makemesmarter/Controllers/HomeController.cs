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
using Newtonsoft.Json;

namespace makemesmarter.Controllers
{
    public class HomeController : Controller
    {
        private makemesmarterContext db = new makemesmarterContext();

        // GET: Home
        public ActionResult Index()
        {
            ViewData["Title"] = "Code-Review Analytics";
            AddChartLinks();
            return View(db.Users.ToList());
        }

        private void AddChartLinks()
        {
            var chartLinks = new List<Tuple<string, string>>();
            chartLinks.Add(new Tuple<string, string>("OverAll Distribution", "CommentsOverAll"));
            chartLinks.Add(new Tuple<string, string>("FileBased-Distribution", "CommentsByFileType"));
            ViewData["ChartListLink"] = chartLinks;
        }

        public ActionResult CommentsOverAll()
        {
            ViewData["Title"] = "Code-Review Analytics : Comments OverAll";
            ViewData["ChartType"] = "Pie";
            ViewBag.ChartTitle = "Comment OverAll Distribution";
            AddChartLinks();
            ViewBag.DataPoints = JsonConvert.SerializeObject(DataService.GetCommentOverAllPie(db.CommentThreads), _jsonSetting);
            return View("Index");
        }

        public ActionResult CommentsByFileType()
        {
            ViewData["Title"] = "Code-Review Analytics : Comments File Base";
            ViewData["ChartType"] = "Pie";
            ViewBag.ChartTitle = "Comments File Base";
            AddChartLinks();
            ViewBag.DataPoints = JsonConvert.SerializeObject(DataService.GetCommentByFileTypePie(db.CommentThreads), _jsonSetting);
            return View("Index");
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
            
            var whiteListedmailAddressList = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("ashkuma@microsoft.com", "ashish kumar"),
                new Tuple<string, string>("nazikh@microsoft.com", "nazia khan"),
                new Tuple<string, string>("umkan@microsoft.com", "umesh kanoja"),
                new Tuple<string, string>("riniga@microsoft.com", "richa nigam"),
                new Tuple<string, string>("namratar@microsoft.com", "namrata ramkrishna"),
                new Tuple<string, string>("ashikum@microsoft.com", "ashish kumar"),
                new Tuple<string, string>("amoagarw@microsoft.com", "amol agarwal"),
            };

            var pendingList = db.CommentThreads.Where(x => x.Status.Equals("pending") || x.Status.Equals("active")).ToList();
            var pendingListByauthor = pendingList.GroupBy(x => x.PrAuthorId);
            var content = string.Empty;
            foreach (var pendingComment in pendingListByauthor)
            {
                if(whiteListedmailAddressList.Where(x => x.Item1.Contains(pendingComment.Key)).ToList().Count() > 0)
                {
                    var listmail = new List<EmailAddress>();
                    EmailAddress testemail = new EmailAddress();
                    testemail.Email = "ashkuma@microsoft.com";
                    testemail.Name = "ashish kumar";
                    listmail.Add(testemail);
                    content = MailContentCreator.CreatePendingCommentResponse(pendingComment.ToList());
                    await SendEmail(listmail, "Pending CR comments", content);
                }
            }

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

        JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
    }
}
