using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpDesk.Data;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace HelpDesk.Models
{
    public class PartitionModel
    {
        public Users CurrentUser { get; set; }
        private Partitions partition;
        public Partitions Partition
        {
            get { return partition; }
            set { partition = value; }
        }

        public int ParId { get; set; }

        public string Notation { get; set; }

        public List<string> Statuses { get; set; }

        public bool isExist;

        public string message;

        public string Status0 { get; set; }
        public string Status1 { get; set; }
        public string Status2 { get; set; }
        public string Status3 { get; set; }
        public string Status4 { get; set; }
        public string Status5 { get; set; }
        public string Status6 { get; set; }
        public string Status7 { get; set; }
        public string Status8 { get; set; }
        public string Status9 { get; set; }

        public PartitionModel() { }
        public PartitionModel(string id, HttpContext httpContext)
        {
            CurrentUser = new UserModel(httpContext).User;
            getParFromDb(id);
        }

        public PartitionModel(HttpContext httpContext)
        {
            CurrentUser = new UserModel(httpContext).User;
            Statuses = receivedEmptyStatuses();
        }

        private void getParFromDb(string id)
        {
            int Id = 0;
            if(int.TryParse(id, out Id))
            {
                try
                {
                    using (HelpdeskContext db = new HelpdeskContext())
                    {
                        var par = db.Partitions
                            .Include(p => p.Statuses)
                            .Where(p => p.Id == Id)
                            .FirstOrDefault();
                        if (par == null)
                        {
                            Partition = new Partitions();
                            isExist = false;
                        }
                        else
                        {
                            Partition = par;
                            Notation = par.Notation;
                            ParId = par.Id;
                            isExist = true;
                        }

                        List<string> statuses = new List<string>();
                        try
                        {
                            statuses = par.Statuses.Select(s => s.Notation).ToList();
                        }
                        catch { }
                        if (statuses != null)
                            Statuses = statuses;
                        else
                            Statuses = new List<string>();
                        for (int i = Statuses.Count; i < 10; i++)
                            Statuses.Add("");
                    }
                }
                catch { }
            }
        }

        public int saveChangesToDb()
        {
            int result = -1;
            if(this.Notation == null)
            {
                message = "Заполните имя раздела";
                return result;
            }
            try
            {
                using(HelpdeskContext db = new HelpdeskContext())
                {
                    Partitions par;
                    try
                    {
                        par = db.Partitions
                            .Include(p => p.Statuses)
                            .Where(p => p.Id == this.ParId)
                            .FirstOrDefault();
                    }
                    catch
                    {
                        message = "Ошибка";
                        return result;
                    }

                    if (par.Notation != this.Notation)
                        par.Notation = this.Notation;
                    
                    var receivedStat = receivedStatuses();

                    try
                    {
                        for (int i = 0; i < par.Statuses.Count; i++)
                        {
                            par.Statuses.ElementAt(i).Notation = receivedStat[i];
                        }
                    }
                    catch
                    {
                        message = "Статус нельзя удалить";
                        return result;
                    }

                    for(int i = par.Statuses.Count; i < receivedStat.Count; i++)
                    {
                        db.Statuses.Add(new Statuses()
                        {
                            IdPartition = par.Id,
                            Notation = receivedStat[i]
                        });
                    }
                    db.SaveChanges();
                    result = par.Id;
                }
            }
            catch(Exception e)
            {
                message = "Ошибка! "+e.Message;
            }

            return result;
        }

        private List<string> receivedStatuses()
        {
            List<string> result = new List<string>();
            if (Status0 != null)
                result.Add(Status0);
            if (Status1 != null)
                result.Add(Status1);
            if (Status2 != null)
                result.Add(Status2);
            if (Status3 != null)
                result.Add(Status3);
            if (Status4 != null)
                result.Add(Status4);
            if (Status5 != null)
                result.Add(Status5);
            if (Status6 != null)
                result.Add(Status6);
            if (Status7 != null)
                result.Add(Status7);
            if (Status8 != null)
                result.Add(Status8);
            if (Status9 != null)
                result.Add(Status9);
            return result;
        }

        private List<string> receivedEmptyStatuses()
        {
            List<string> result = new List<string>();
            for(int i = 0; i < 10; i++)
            {
                result.Add("");
            }
            return result;
        }

        public int SaveNewPar()
        {
            int result = -1;
            if(this.Notation == null)
            {
                message = "Заполните имя раздела";
                return result;
            }

            this.Statuses = receivedStatuses();
            try
            {
                using(HelpdeskContext db = new HelpdeskContext())
                {
                    if (db.Partitions.Select(p => p.Notation).Contains(this.Notation))
                    {
                        message = "Такой раздел уже существует";
                        return -1;
                    }

                    db.Partitions.Add(new Partitions()
                    {
                        Notation = this.Notation
                    });
                    db.SaveChanges();

                    var par = db.Partitions.Where(p => p.Notation == this.Notation).FirstOrDefault();
                    foreach(string stat in Statuses)
                    {
                        db.Statuses.Add(new Statuses()
                        {
                            IdPartition = par.Id,
                            Notation = stat
                        });
                    }
                    db.SaveChanges();
                    result = par.Id;
                }

            }
            catch(Exception e)
            {
                message = "Ошибка! " + e.Message;
                return -1;
            }
            return result;
        }

    }
}
