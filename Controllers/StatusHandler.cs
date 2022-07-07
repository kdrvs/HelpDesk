using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpDesk.Data;
using Microsoft.AspNetCore;

namespace HelpDesk.Controllers
{
    public class StatusHandler
    {
        public List<Statuses> StatusesOfPartition { get; set; }
        public Statuses CurrentStatus { get; set; }

        public StatusHandler(Requests request)
        {
            getStatusesFromDb(request);
        }

        private void getStatusesFromDb(Requests request)
        {
            int currentStatusId = 0;
            using (HelpdeskContext db = new HelpdeskContext())
            {
                try
                {
                    currentStatusId = db.StatusOfRequests.Where(st => st.IdRequest == request.Id).LastOrDefault().IdStatus;
                }
                catch { }
                if(currentStatusId != 0)
                {
                    try
                    {
                        var curStat = db.Statuses.Find(currentStatusId);
                        if (curStat != null)
                            CurrentStatus = curStat;
                        else
                        {
                            CurrentStatus = new Statuses();
                            CurrentStatus.Id = 0;
                            CurrentStatus.Notation = "Статус не выбран!";
                        }
                        
                    }
                    catch { }
                }
                else
                {
                    CurrentStatus = new Statuses();
                    CurrentStatus.Id = 0;
                    CurrentStatus.Notation = "Статус не выбран!";
                }
                StatusesOfPartition = db.Statuses.Where(s => s.IdPartition == request.IdPartition).ToList();
            }
        }
    }
}
