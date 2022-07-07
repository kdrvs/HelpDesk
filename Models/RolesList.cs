using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpDesk.Data;
using Microsoft.AspNetCore.Http;

namespace HelpDesk.Models
{
    public class RolesList
    {
        public Users CurrentUser { get; set; }

        public List<Roles> Roles { get; set; }

        public RolesList(HttpContext httpContext)
        {
            CurrentUser = new UserModel(httpContext).User;
            getRolesFromDb();
        }

        private void getRolesFromDb()
        {
            using(HelpdeskContext db = new HelpdeskContext())
            {
                Roles = db.Roles.OrderBy(r => r.Notation).ToList();
            }
        }
    }
}
