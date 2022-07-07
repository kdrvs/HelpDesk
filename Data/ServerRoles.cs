using System;
using System.Collections.Generic;

namespace HelpDesk.Data
{
    public partial class ServerRoles
    {
        public ServerRoles()
        {
            Users = new HashSet<Users>();
        }

        public string ServerRole { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}
