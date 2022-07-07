using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpDesk.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Models
{
    public class ListRequestsModel
    {
        public List<Requests> Requests { get; set; }
        public Users User { get; set; }

       

        public ListRequestsModel(HttpContext httpContext)
        {
            User = new UserModel(httpContext).User;
            Requests = getRequestFromDbOnUserId(User.Id);
        }

        public ListRequestsModel(HttpContext httpContext, string partitions)
        {
            User = new UserModel(httpContext).User;
            Requests = getRequestFromDbOnPart(partitions);
        }
       

        private List<Requests> getRequestFromDbOnUserId(int userId)
        {
            List<Requests> myRequests;
            using(HelpdeskContext db = new HelpdeskContext())
            {
                var myReqestsOnId = db.Requests
                    .OrderByDescending(r => r.Id)
                    .Include(r=> r.IdPartitionNavigation)
                    .Include(r=> r.IdCreatorNavigation)
                    .Where(r => r.IdCreator == userId);
                if (myReqestsOnId != null)
                {
                    myRequests = myReqestsOnId.ToList();
                }
                else myRequests = new List<Requests>();
                
            }
            return myRequests;
        }

        private List<Requests> getRequestFromDbOnPart(string partition)
        {
            List<Requests> myRequests = new List<Requests>();
            using(HelpdeskContext db = new HelpdeskContext())
            {
                Partitions part = db.Partitions.Where(par => par.Notation == partition).FirstOrDefault();

                var req = db.Requests
                    .OrderByDescending(r => r.Id)
                    .Include(r => r.IdPartitionNavigation)
                    .Include(r => r.IdCreatorNavigation)
                    .Where(re => re.IdPartition == db.Partitions.Where(p => p.Notation == partition).FirstOrDefault().Id);
                myRequests = req.ToList();
            }
            return myRequests;
        }
        
    }
}
