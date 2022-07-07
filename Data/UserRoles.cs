using System;
using System.Collections.Generic;

namespace HelpDesk.Data
{
    public partial class UserRoles
    {
        public int IdUser { get; set; }
        public int IdRole { get; set; }

        public virtual Roles IdRoleNavigation { get; set; }
        public virtual Users IdUserNavigation { get; set; }
    }
}
