using System;
using System.Collections.Generic;

namespace HelpDesk.Data
{
    public partial class Roles
    {
        public Roles()
        {
            Accesses = new HashSet<Accesses>();
            UserRoles = new HashSet<UserRoles>();
        }

        public int Id { get; set; }
        public string Notation { get; set; }

        public virtual ICollection<Accesses> Accesses { get; set; }
        public virtual ICollection<UserRoles> UserRoles { get; set; }
    }
}
