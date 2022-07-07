using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Data
{
    public partial class Comments
    {
        public DateTime AddTime { get; set; }
        public int IdUser { get; set; }
        public int IdRequest { get; set; }

        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        public virtual Requests IdRequestNavigation { get; set; }
        public virtual Users IdUserNavigation { get; set; }
    }
}
