using System;
using System.Collections.Generic;

namespace HelpDesk.Data
{
    public partial class Users
    {
        public Users()
        {
            Comments = new HashSet<Comments>();
            PerformersOfRequests = new HashSet<PerformersOfRequests>();
            PerformersOfTasks = new HashSet<PerformersOfTasks>();
            Requests = new HashSet<Requests>();
            Tasks = new HashSet<Tasks>();
            UserRoles = new HashSet<UserRoles>();
            Visits = new HashSet<Visits>();
        }

        public Users(int Id)
        {
            Users u = getUserFromDB(Id);
            if (u is null)
                return;
            this.Id = u.Id;
            this.Email = u.Email;
            this.Surname = u.Surname;
            this.Name = u.Name;
            this.Post = u.Post;
            this.ServerRole = u.ServerRole;
        }

        public Users getUserFromDB(int Id)
        {
            Users u = new Users();
            using(HelpdeskContext db = new HelpdeskContext())
            {
                u = db.Users.Find(Id);
            }

            return u;
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Post { get; set; }
        public string ServerRole { get; set; }

        public virtual ServerRoles ServerRoleNavigation { get; set; }
        public virtual Authentication Authentication { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<PerformersOfRequests> PerformersOfRequests { get; set; }
        public virtual ICollection<PerformersOfTasks> PerformersOfTasks { get; set; }
        public virtual ICollection<Requests> Requests { get; set; }
        public virtual ICollection<Tasks> Tasks { get; set; }
        public virtual ICollection<UserRoles> UserRoles { get; set; }
        public virtual ICollection<Visits> Visits { get; set; }
    }
}
