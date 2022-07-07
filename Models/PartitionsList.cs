using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpDesk.Data;
using Microsoft.AspNetCore.Http;

namespace HelpDesk.Models
{
    public class PartitionsList
    {        
        public List<Partitions> Partitions { get; set; }
        public Users CurrentUser { get; set; }

        public string NewPar { get; set; }

        public string Message { get; set; }

        public PartitionsList(HttpContext httpContext)
        {
            CurrentUser = new UserModel(httpContext).User;
            Partitions = getPartitionsFromDb();
        }

        private List<Partitions> getPartitionsFromDb()
        {
            List<Partitions> list = new List<Partitions>();
            using (HelpdeskContext db = new HelpdeskContext())
            {
                list = db.Partitions
                    .OrderByDescending(p => p.Id)
                    .ToList();
            }
            return list;
        }



    }
}
