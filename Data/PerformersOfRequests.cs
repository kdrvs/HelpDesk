using System;
using System.Collections.Generic;

namespace HelpDesk.Data
{
    public partial class PerformersOfRequests
    {
        public int IdRequest { get; set; }
        public int IdPerformer { get; set; }

        public virtual Users IdPerformerNavigation { get; set; }
        public virtual Requests IdRequestNavigation { get; set; }
    }
}
