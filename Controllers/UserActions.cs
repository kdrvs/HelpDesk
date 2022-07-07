using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HelpDesk.Data;

namespace HelpDesk.Controllers
{
    public class UserActions
    {
        //public List<IActionResult> actionResults = new List<IActionResult>();
        public Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

        public UserActions(Users user)
        {
            setAction(user.ServerRole);
        }

        private void setAction(string role)
        {
            keyValuePairs.Add("Разделы", "../Partitions/List");

            switch (role)
            {
                case "admin":
                    keyValuePairs.Add("Учетные записи", "../Users/List");
                    keyValuePairs.Add("Роли", "../Roles/List");
                    break;

            }
                
        }
    }
}
