using System;
using System.Collections.Generic;

namespace HelpDesk.Data
{
    public partial class Visits
    {
        public int Id { get; set; }
        public DateTime VisitTime { get; set; }
        public int IdRequest { get; set; }
        public int IdUser { get; set; }

        public virtual Requests IdRequestNavigation { get; set; }
        public virtual Users IdUserNavigation { get; set; }
    }
}
