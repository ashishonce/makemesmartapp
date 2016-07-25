using System;
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
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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

        // POST: api/Users
        [ResponseType(typeof(void))]
        public IHttpActionResult PostMessage(string UserId, string Message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!UserExists(UserId))
            {
                return NotFound();
            }

            var user =  db.Users.Find(UserId);

            SendNotification(user.Token, Message, 1);

            return StatusCode(HttpStatusCode.NoContent);
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

        public void SendNotification(string clientToken, string message, int badgeCount)
        {
            var GoogleAppID = "AIzaSyDxPQm7K6XCtwXPyDDgeOhmgbzdzxoedD4";
            var SenderID = "24788899907";

            WebRequest gcmSendRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");
            gcmSendRequest.Method = "post";
            gcmSendRequest.Headers.Add(string.Format("Authorization: key={0}", GoogleAppID));
            gcmSendRequest.Headers.Add(string.Format("Sender: id={0}", SenderID));

            string postData = string.Format("collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.message={0} &data.time={1} &data.badge={3} &data.sound={4}&registration_id={2}", message, DateTime.UtcNow, clientToken, badgeCount, "default");
            Byte[] byteArray = Encoding.ASCII.GetBytes(postData);
            gcmSendRequest.ContentLength = byteArray.Length;
            Stream dataStream = gcmSendRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse webResponse = gcmSendRequest.GetResponse();
            dataStream = webResponse.GetResponseStream();
            using (StreamReader streamReader = new StreamReader(dataStream))
            {
                string sResponseFromServer = streamReader.ReadToEnd();
                streamReader.Close();
                dataStream.Close();
                webResponse.Close();
            }
        }
    }
}