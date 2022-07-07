using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HelpDesk.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Models
{
    public class RequestModel
    {
        public Requests Request { get; set; }

        public Users Creator { get;  }
        public UserModel CurrentUser { get; set; }

        public String Status { get; set; }

        public int RequestId { get; set; }
        public int CurrentUserId { get; set; }

        [DataType(DataType.MultilineText)]
        public string NewComment { get; set; }

        public RequestModel() { }

        public RequestModel(int requestId, HttpContext httpContext)
        {
            CurrentUser = new UserModel(httpContext);
            Request = getRequestOnId(requestId);
            Creator = new Users(Request.IdCreator);
        }

        private Requests getRequestOnId(int requestId)
        {
            Requests _request;
            using(HelpdeskContext db = new HelpdeskContext())
            {
                try
                {
                    var req = db.Requests   
                        .Include(r => r.IdPartitionNavigation)
                        .Include(r => r.Comments)
                            .ThenInclude(c => c.IdUserNavigation)
                        .SingleOrDefault(r => r.Id == requestId);

                    if (req != null)
                    {
                        _request = req;
                        
                    }
                    else _request = new Requests();
                }
                catch { _request = new Requests(); }
            }
            return _request;
        }

        public int saveChangeFromToDb()
        {
            int result = -1;
            if (Status != null || NewComment != null)
            {         
                Requests request;
                Users writer;
                Statuses currentStatus;
                Statuses nextStatus;
                int newStatusId = 0;
                if (Status != null)
                    int.TryParse(Status, out newStatusId);

                using (HelpdeskContext db = new HelpdeskContext())
                {

                    request = db.Requests.Include(r => r.StatusOfRequests).Where(r => r.Id == RequestId).SingleOrDefault();                    
                    writer = db.Users.Find(CurrentUserId);

                    if (request != null && writer != null)
                    {
                        try
                        {                                                      
                            if (newStatusId != 0
                                && db.Statuses.Where(s => s.IdPartition == request.IdPartition)
                                    .Select(s => s.Id)
                                    .Contains(newStatusId)
                                && newStatusId != request.StatusOfRequests.LastOrDefault().IdStatus
                               )
                            {
                                try
                                {
                                    currentStatus = db.Statuses.Find(request.StatusOfRequests.LastOrDefault().IdStatus);
                                    nextStatus = db.Statuses.Find(newStatusId);
                                    db.StatusOfRequests.Add(new StatusOfRequests()
                                    { ChangeTime = DateTime.Now, IdRequest = request.Id, IdStatus = newStatusId });
                                    NewComment = $"{writer.Name} {writer.Surname}: {currentStatus.Notation} -> {nextStatus.Notation} \r\n\n"
                                        + NewComment;
                                }
                                catch { }
                            }
                        }
                        catch { }

                        if (NewComment != null)
                        {                           
                            db.Comments.Add(new Comments()
                            {
                                AddTime = DateTime.Now,
                                IdRequest = request.Id,
                                IdUser = writer.Id,
                                Comment = NewComment
                            });
                        }
                        db.SaveChanges();
                        result = 1;
                    }
                }
            }
            return result;
        }
    }
}
