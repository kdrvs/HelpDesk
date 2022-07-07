using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpDesk.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.SqlServer;

namespace HelpDesk.Models
{
    public class NewRequestModel
    {
        public NewRequestModel() {}
        public NewRequestModel(HttpContext httpContext)
        {
            CurrentUser = new UserModel(httpContext);
        }
        private int partitionId
        {
            get
            {
                int part;
                int.TryParse(Partitions, out part);
                return part;
            }
        }
        
        public string Partitions { get; set; }

        public UserModel CurrentUser { get; set; }

        public string Heading { get; set; }

        private int statusId;
        public string StatusId
        {
            set { int.TryParse(value, out statusId); }
        }

        private DateTime creationTime;

        public string Comment { get; set; }

        public string ErrorMessage { get; set; }

        public int saveChangeToDb(out string message)
        {
            message = "";
            if (partitionId == 0)
            {
                message = "Не выбран раздел!";
                return 0;
            }
            if(Heading == null)
            {
                message = "Заполните поле \"Описание\"";
                return 0;
            }
            if(Heading.Length > 1000)
            {
                message = "В описании должно быть не более 1000 знаков!";
                return 0;
            }
            if(Comment != null && Comment.Length > 1000)
            {
                message = "В комментарии должно быть не более 1000 знаков!";
                return 0;
            }

            int result = 0;
            try
            {
                using (HelpdeskContext db = new HelpdeskContext())
                {

                    db.Requests.Add(new Requests()
                    {
                        CreationTime = DateTime.Now,
                        Heading = this.Heading,
                        IdCreator = this.CurrentUser.User.Id,
                        IdPartition = this.partitionId
                    });
                    db.SaveChanges();

                    Requests request = db.Requests.Where(r => r.IdCreator == this.CurrentUser.User.Id).Last();

                    db.StatusOfRequests.Add(new StatusOfRequests()
                    {
                        ChangeTime = request.CreationTime,
                        IdRequest = request.Id,
                        IdStatus = db.Statuses.Where(s => s.IdPartition == request.IdPartition).FirstOrDefault().Id
                    });

                    if (Comment != null)
                    {
                        db.Comments.Add(new Comments()
                        {
                            AddTime = request.CreationTime,
                            Comment = this.Comment,
                            IdRequest = request.Id,
                            IdUser = CurrentUser.User.Id
                        });
                    }
                    db.SaveChanges();
                    result = request.Id;
                }
            }
            catch(Exception e)
            {
                message = e.Message;
            }
            return result;

        }
    }
}
