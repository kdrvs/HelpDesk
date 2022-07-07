using System;
using System.Collections.Generic;

namespace HelpDesk.Data
{
    public partial class Tasks
    {
        public Tasks()
        {
            InverseIdSuperTaskNavigation = new HashSet<Tasks>();
            PerformersOfTasks = new HashSet<PerformersOfTasks>();
        }

        public int Id { get; set; }
        public int IdRequest { get; set; }
        public DateTime AddTime { get; set; }
        public int IdCreator { get; set; }
        public string Task { get; set; }
        public bool? Done { get; set; }
        public int? IdSuperTask { get; set; }

        public virtual Users IdCreatorNavigation { get; set; }
        public virtual Requests IdRequestNavigation { get; set; }
        public virtual Tasks IdSuperTaskNavigation { get; set; }
        public virtual ICollection<Tasks> InverseIdSuperTaskNavigation { get; set; }
        public virtual ICollection<PerformersOfTasks> PerformersOfTasks { get; set; }
    }
}
