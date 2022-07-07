using System;
using System.Collections.Generic;

namespace HelpDesk.Data
{
    public partial class Partitions
    {
        public Partitions()
        {
            Accesses = new HashSet<Accesses>();
            InverseIdSuperPartitionNavigation = new HashSet<Partitions>();
            Requests = new HashSet<Requests>();
            Statuses = new HashSet<Statuses>();
        }

        public int Id { get; set; }
        public string Notation { get; set; }
        public bool? ForRequest { get; set; }
        public int? IdSuperPartition { get; set; }

        //public Partitions getPartitionsFromDB()
        //{
        //    var partitions = new Partitions();
        //    using(HelpdeskContext db = new HelpdeskContext())
        //    {
        //        partitions = db.Partitions.ToList(); -- Почему-то не работает.
        //    }
        //}

        public virtual Partitions IdSuperPartitionNavigation { get; set; }
        public virtual ICollection<Accesses> Accesses { get; set; }
        public virtual ICollection<Partitions> InverseIdSuperPartitionNavigation { get; set; }
        public virtual ICollection<Requests> Requests { get; set; }
        public virtual ICollection<Statuses> Statuses { get; set; }
    }
}
