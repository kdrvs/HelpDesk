using System;
using System.Collections.Generic;

namespace HelpDesk.Data
{
    public partial class Accesses
    {
        public int IdRole { get; set; }
        public int IdPartition { get; set; }
        public bool? Performer { get; set; }

        public virtual Partitions IdPartitionNavigation { get; set; }
        public virtual Roles IdRoleNavigation { get; set; }
    }
}
