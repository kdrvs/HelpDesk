using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace HelpDesk.Data
{
    public partial class Authentication
    {
        public int IdUser { get; set; }
        public string UserPassword { get; set; }

        public virtual Users IdUserNavigation { get; set; }

        public static string hashed(string pass)
        {
            byte[] arr = Encoding.ASCII.GetBytes("1Z20f45e");
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: pass,
                salt: arr,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return hashed;
        }
    }
}
