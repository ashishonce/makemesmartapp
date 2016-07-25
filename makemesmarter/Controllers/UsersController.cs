﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using makemesmarter.Models;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Threading.Tasks;
using System.Web.Mvc;


//public class Content 
//{
//        public List<String> registration_ids;
//        public Map<String, String> data;

//        public void addRegId(String regId)
//        {
//            if (registration_ids == null)
//                registration_ids = new LinkedList<String>();
//            registration_ids.add(regId);
//        }

//        public void createData(String title, String message)
//        {
//            if (data == null)
//                data = new HashMap<String, String>();

//            data.put("title", title);
//            data.put("message", message);
//        }
//}


namespace makemesmarter.Controllers
{
    public class UsersController : ApiController
    {
        private makemesmarterContext db = new makemesmarterContext();

        // GET: api/Users
        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(string id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(string id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserId)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Users
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult>  PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(string.IsNullOrWhiteSpace( user.Token ))
            {
                var newUser = db.Users.Find(user.UserId);
                var data = await SuggestionModel.GetSuggestions(user.Name);
                //var responseCODE = SendGCMNotification(newUser.Token, user.Name);//SendNotification(newUser.Token, user.Name, 10);
                
                var responseCODE = SendGCMNotification(newUser.Token, data);
                return StatusCode(responseCODE);
            }

            db.Users.Add(user);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(string id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(string id)
        {
            return db.Users.Count(e => e.UserId == id) > 0;
        }

        public HttpStatusCode SendNotification(string clientToken, string message, int badgeCount)
        {
            var GoogleAppID = "AIzaSyDxPQm7K6XCtwXPyDDgeOhmgbzdzxoedD4";
            var SenderID = "24788899907";

            HttpWebRequest gcmSendRequest = (HttpWebRequest) WebRequest.Create("https://android.googleapis.com/gcm/send");
            gcmSendRequest.Method = @"POST";
            gcmSendRequest.ContentType = @"application/json";
            gcmSendRequest.Headers.Add(string.Format("Authorization: key={0}", GoogleAppID));
            gcmSendRequest.Headers.Add(string.Format("Sender: id={0}", SenderID));

            string postData = string.Format("collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.message={0} &data.time={1} &data.badge={3} &data.sound={4}&registration_id={2}", message, DateTime.UtcNow, clientToken, badgeCount, "default");

            string postString = "{\"collapse_key\": \"Food-Promo\",\"data\": {\"Category\": \"FOOD\",\"Type\": VEG\"},\"registration_ids\":[ \"APA91bEk7GPFVxzOidvB3JKCMWq3FHpAaTj2dBv9VGOQkKtLAEiVGR8TDi1fsU4D1k293ODAFTJ8dN​ fE2gzJNfCvB1sjewZu2fGOIJmY8dgjcNTZQYi4QfyQH-AaO0qEmQnbEeEtsUQ5LzWrIHboAhJMx1bfdsO​9bg\"]}";


            Byte[] byteArray = Encoding.ASCII.GetBytes(postData);
            gcmSendRequest.ContentLength = byteArray.Length;
            Stream dataStream = gcmSendRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            //dataStream.Wri
            dataStream.Close();
            HttpWebResponse webResponse = (HttpWebResponse)gcmSendRequest.GetResponse();
            return webResponse.StatusCode;
            dataStream = webResponse.GetResponseStream();
            using (StreamReader streamReader = new StreamReader(dataStream))
            {
                string sResponseFromServer = streamReader.ReadToEnd();
                streamReader.Close();
                dataStream.Close();
                webResponse.Close();
            }
        }

        public HttpStatusCode SendGCMNotification( string deviceId, string message)
        {
            string postDataContentType = "application/json";
            var apiKey = "AIzaSyDxPQm7K6XCtwXPyDDgeOhmgbzdzxoedD4"; // hardcorded
            

            
            string tickerText = "example test GCM";
            string contentTitle = "content title GCM";
            string postData =
            "{ \"registration_ids\": [ \"" + deviceId + "\" ], " +
              "\"data\": {\"tickerText\":\"" + tickerText + "\", " +
                         "\"contentTitle\":\"" + contentTitle + "\", " +
                         "\"message\": \"" + message + "\"}}";


            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateServerCertificate);

            //
            //  MESSAGE CONTENT
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            //
            //  CREATE REQUEST
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("https://android.googleapis.com/gcm/send");
            Request.Method = "POST";
            Request.KeepAlive = false;
            Request.ContentType = postDataContentType;
            Request.Headers.Add(string.Format("Authorization: key={0}", apiKey));
            Request.ContentLength = byteArray.Length;

            Stream dataStream = Request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            //
            //  SEND MESSAGE
            try
            {
                WebResponse Response = Request.GetResponse();
                HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;
                if (ResponseCode.Equals(HttpStatusCode.Unauthorized) || ResponseCode.Equals(HttpStatusCode.Forbidden))
                {
                    var text = "Unauthorized - need new token";
                }
                else if (!ResponseCode.Equals(HttpStatusCode.OK))
                {
                    var text = "Response from web service isn't OK";
                }

                StreamReader Reader = new StreamReader(Response.GetResponseStream());
                string responseLine = Reader.ReadToEnd();
                Reader.Close();

                return ResponseCode;
            }
            catch (Exception e)
            {
            }
            return HttpStatusCode.InternalServerError;
        }

        public static bool ValidateServerCertificate(
        object sender,
        X509Certificate certificate,
        X509Chain chain,
        SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}