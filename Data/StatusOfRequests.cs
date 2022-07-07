using System;
using System.Collections.Generic;

namespace HelpDesk.Data
{
    public partial class StatusOfRequests
    {
        public DateTime ChangeTime { get; set; }
        public int IdRequest { get; set; }
        public int IdStatus { get; set; }

        public virtual Requests IdRequestNavigation { get; set; }
        public virtual Statuses IdStatusNavigation { get; set; }
    }
}
