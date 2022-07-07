using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpDesk.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Models
{
    public class UsersListModel
    {
        public List<Users> UsersList { get; set; }
        public Users CurrentUser { get; set; }

        public UsersListModel(HttpContext httpContext)
        {
            CurrentUser = new UserModel(httpContext).User;
            UsersList = getUsersFromDb();
        }

        private List<Users> getUsersFromDb()
        {
            List<Users> users = new List<Users>();
            using (HelpdeskContext db = new HelpdeskContext())
            {
                users = db.Users.OrderBy(u => u.Surname).ToList();
            }
            return users;
        }
    }
}
