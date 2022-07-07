using System;
using System.Collections.Generic;

namespace HelpDesk.Data
{
    public partial class PerformersOfTasks
    {
        public int IdTask { get; set; }
        public int IdPerformer { get; set; }

        public virtual Users IdPerformerNavigation { get; set; }
        public virtual Tasks IdTaskNavigation { get; set; }
    }
}
