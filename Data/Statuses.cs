using System;
using System.Collections.Generic;

namespace HelpDesk.Data
{
    public partial class Statuses
    {
        public Statuses()
        {
            StatusOfRequests = new HashSet<StatusOfRequests>();
        }

        public int Id { get; set; }
        public int IdPartition { get; set; }
        public string Notation { get; set; }

        public virtual Partitions IdPartitionNavigation { get; set; }
        public virtual ICollection<StatusOfRequests> StatusOfRequests { get; set; }
    }
}
