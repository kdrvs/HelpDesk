using System;
using System.Collections.Generic;

namespace HelpDesk.Data
{
    public partial class Requests
    {
        public Requests()
        {
            Comments = new HashSet<Comments>();
            PerformersOfRequests = new HashSet<PerformersOfRequests>();
            StatusOfRequests = new HashSet<StatusOfRequests>();
            Tasks = new HashSet<Tasks>();
            Visits = new HashSet<Visits>();
        }

        public int Id { get; set; }
        public DateTime CreationTime { get; set; }
        public string Heading { get; set; }
        public int IdCreator { get; set; }
        public int IdPartition { get; set; }

        public virtual Users IdCreatorNavigation { get; set; }
        public virtual Partitions IdPartitionNavigation { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<PerformersOfRequests> PerformersOfRequests { get; set; }
        public virtual ICollection<StatusOfRequests> StatusOfRequests { get; set; }
        public virtual ICollection<Tasks> Tasks { get; set; }
        public virtual ICollection<Visits> Visits { get; set; }
    }
}
