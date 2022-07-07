using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpDesk.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Models
{
    public class RoleModel
    {
        public Users CurrentUser { get; set; }

        public Roles role;
        public int RoleId { get; set; }
        public string Notation { get; set; }
        public string Partition { get; set; }
        public bool forPerform { get; set; }

        public String message;

        public List<Accesses> accesses;

        public List<Partitions> allPartitions;

        public RoleModel() { }
        public RoleModel(string id, HttpContext httpContext, out int Id)
        {
            CurrentUser = new UserModel(httpContext).User;
            Id = getRoleFromDb(id);
        }

        private int getRoleFromDb(string id)
        {
            int result = -1;           
            allPartitions = new List<Partitions>();
            int Id = 0;
            if (int.TryParse(id, out Id))
            {
                using (HelpdeskContext db = new HelpdeskContext())
                {
                    role = db.Roles
                        .Include(r => r.Accesses)
                            .ThenInclude(a=>a.IdPartitionNavigation)
                        .Where(r => r.Id == Id)
                        .FirstOrDefault();

                    result = role.Id;
                    accesses = role.Accesses.ToList();
                }
                if(role != null)
                {
                    Notation = role.Notation;
                }              
                
            }

            using(HelpdeskContext db = new HelpdeskContext())
            {
               allPartitions = db.Partitions.ToList();
            }

            return result;
        }

        
    }
}
