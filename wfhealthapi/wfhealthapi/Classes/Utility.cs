using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using wfhealthapi.Models;

namespace wfhealthapi.Classes
{
    public static class Utility
    {
        public static string GetRandomToken()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var randomId = new string(
                Enumerable.Repeat(chars, 15)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            return randomId;
        }


        public static int GetRoleId(string RoleName)
        {
            using (wfhealthdbEntities obj = new wfhealthdbEntities())
            {
                return
                    (from c in obj.RoleMasters where c.RoleName == RoleName && c.IsActive == true select c)
                        .SingleOrDefault().Id;
            }
            
        }
    }

}