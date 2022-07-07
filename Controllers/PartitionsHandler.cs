using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpDesk.Data;

namespace HelpDesk.Controllers
{
    public class PartitionsHandler
    {
        public List<Partitions> Partitions { get; set; } 

        public PartitionsHandler()
        {
            getPartitionFromDb();
        }

        private void getPartitionFromDb()
        {
            using (HelpdeskContext db = new HelpdeskContext())
            {
                Partitions = db.Partitions.ToList();
            }
        }
    }
}
