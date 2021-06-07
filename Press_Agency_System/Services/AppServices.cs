using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Press_Agency_System.Hubs;
using Press_Agency_System.Models;

namespace Press_Agency_System.Services
{
    public class AppServices
    {
        ApplicationDbContext _Context;


        public string FromUser { get; private set; }
        public string ToUser { get; private set; }

        public AppServices()
        {
            _Context = new ApplicationDbContext();
            _Context.Configuration.ProxyCreationEnabled = false;
        }
        public List<UserDTO> GetUsersToChat()
        {
            var userId = HttpContext.Current.User.Identity.Name;
            return _Context.Users
            .Where(x => x.Id != userId)
            .Select(x => new UserDTO
            {
                UserId = x.Id,
                UserName = x.UserName,
                FullName = x.FirstName + " " + x.LastName,
                Photo = x.PhotoPath,
            }).ToList();
        }



        internal bool SendMessage(string toUserId, string message)
        {
            try
            {

                var USER_ID = HttpContext.Current.User.Identity.GetUserId();
                _Context.Message.Add(new Message
                {
                    FromUser = _Context.Users.FirstOrDefault(e => e.Id == USER_ID),
                    ToUser = _Context.Users.FirstOrDefault(e => e.Id == toUserId),
                    Message1 = message,
                    Date = DateTime.Now,

                });
                _Context.SaveChanges();
                ChatHub.RecieveMessage(USER_ID, toUserId, message);
                return true;
            }
            catch { return false; }
        }

        internal string AddUserConnection(string ConnectionId)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            _Context.UserConnections.Add(new UserConnection
            {
                ConnectionId = ConnectionId,
                UserId = _Context.Users.FirstOrDefault(e => e.Id == userId),
            });
            _Context.SaveChanges();
            return userId;
        }


        internal IList<string> GetUserConnections(string userId)
        {
            return _Context.UserConnections.Where(x => x.UserId.Id == userId).Select(x => x.ConnectionId).ToList();
        }

        internal void RemoveAllUserConnections(string userId)
        {
            var current = _Context.UserConnections.Where(x => x.UserId.Id == userId);
            _Context.UserConnections.RemoveRange(current);
            _Context.SaveChanges();
        }

        internal ChatBoxModel GetChatbox(string toUserId)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var toUser = _Context.Users.FirstOrDefault(x => x.Id == toUserId);
            var messages = _Context.Message.Where(x => (x.FromUser.Id == userId && x.ToUser.Id == toUserId) || (x.FromUser.Id == toUserId && x.ToUser.Id == userId))
                .OrderByDescending(x => x.Date)
                .Skip(0)
                .Take(50)
                .Select(x => new MessageDTO
                {
                    ID = x.Id,
                    Message = x.Message1,
                    Class = x.FromUser.Id == userId ? "from" : "to",
                })
                .OrderBy(x => x.ID)
                .ToList();

            return new ChatBoxModel
            {
                ToUser = toUser,
                Messages = messages,
            };
        }

        public UserDTO ToUserDTO(ApplicationUser user)
        {
            return new UserDTO
            {
                FullName = user.FirstName + ' ' + user.LastName,
                UserId = user.Id,
                UserName = user.UserName,
            };
        }


    }
}