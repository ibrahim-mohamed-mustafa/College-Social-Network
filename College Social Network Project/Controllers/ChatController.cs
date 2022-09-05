using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;

using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using College_Social_Network_Project.Hubs;
using College_Social_Network_Project.Models;
using Microsoft.AspNet.Identity;


namespace College_Social_Network_Project.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private Social_NetworkEntities db = new Social_NetworkEntities();
      
        public ActionResult Index()
        { 
            return View();
        }
    
        [HttpPost]
        public ActionResult GetChatbox(string toUserId)
        {
            ChatBoxModel chatBoxModel = GetChatbox1(toUserId);
            return PartialView("~/Views/Chat/_ChatBox.cshtml", chatBoxModel); 
        }

        [HttpPost]
        public ActionResult SendMessage(string toUserId, string message)
        {
            return Json(SendMessage1(toUserId, message));
        }

        [HttpPost]
        public ActionResult LazyLoadMssages(string toUserId, int skip)
        {   
            return Json(LazyLoadMssages1(toUserId, skip));
        }

        public List<UserDTO> GetUsersToChat()
        {
            string IDd = System.Web.HttpContext.Current.User.Identity.GetUserId() ;
            return db.AspNetUsers
                .Include("UserConnections")
                .Where(x => x.Id != IDd)
                .Select(x => new UserDTO
                {
                    UserId = x.Id,
                    UserName = x.UserName,
                    //IsOnline = true,
                    IsOnline = x.UserConnections.Count > 0,
                }).ToList();
        }

        internal string AddUserConnection(Guid ConnectionId)
        {  
           string userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            db.UserConnections.Add(new UserConnection
            {
                ConnectionId = ConnectionId,
                UserId = userId,
            });
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
            }
            return userId;
        }

        internal string RemoveUserConnection(Guid ConnectionId)
        {
            string userId = "";
            var current = db.UserConnections.FirstOrDefault(x => x.ConnectionId == ConnectionId);
            if (current != null)
            {
                userId = current.UserId ?? "" ;
                db.UserConnections.Remove(current);
                db.SaveChanges();
            }
            return userId;
        }

        internal IList<string> GetUSerConnections(string uSerId)
        {
            return db.UserConnections.Where(x => x.UserId == uSerId).Select(x => x.ConnectionId.ToString()).ToList();
        }

        internal void RemoveAllUserConnections(string userId)
        {
            var current = db.UserConnections.Where(x => x.UserId == userId);
            db.UserConnections.RemoveRange(current);
            db.SaveChanges();
        }

        internal ChatBoxModel GetChatbox1(string toUserId)
        { 
            string userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var toUser = db.AspNetUsers.FirstOrDefault(x => x.Id == toUserId);
            var messages = db.Messages.Where(x => (x.FromUser == userId && x.ToUser == toUserId) || (x.FromUser == toUserId && x.ToUser == userId))
                .OrderByDescending(x => x.Date)
                .Skip(0)
                .Take(50)
                .Select(x => new MessageDTO
                {
                    ID = x.Id,
                    Message = x.Message1,
                    Class = x.FromUser == userId ? "from" : "to",
                })
                .OrderBy(x => x.ID)
                .ToList();

            return new ChatBoxModel
            {
                ToUser = ToUserDTO(toUser),
                Messages = messages,
            };
        }

        internal bool SendMessage1(string toUserId, string message)
        {
            try
            {
                string USER_ID = System.Web.HttpContext.Current.User.Identity.GetUserId();
                db.Messages.Add(new Message
                {
                    FromUser = USER_ID,
                    ToUser = toUserId,
                    Message1 = message,
                    Date = DateTime.Now
                });
                db.SaveChanges();
                ChatHub.RecieveMessage(USER_ID, toUserId, message);
                return true;
            }
            catch { return false; }
        }

        public UserDTO ToUserDTO(AspNetUser user)
        {
            return new UserDTO
            {
              
                UserId = user.Id,
                UserName = user.UserName,
            };
        }

        internal List<MessageDTO> LazyLoadMssages1(string toUserId, int skip)
        {
            string userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var messages = db.Messages.Where(x => (x.FromUser == userId && x.ToUser == toUserId) || (x.FromUser == toUserId && x.ToUser == userId))
                .OrderByDescending(x => x.Date)
                .Skip(skip)
                .Take(50)
                .Select(x => new MessageDTO
                {
                    ID = x.Id,
                    Message = x.Message1,
                    Class = x.FromUser == userId ? "from" : "to",
                })
                .OrderByDescending(x => x.ID)
                .ToList();
            return messages;
        }

    }
}