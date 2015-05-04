using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wfhealthapi.Classes
{
    public class Utility
    {
        public string GetRandomToken()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var randomId = new string(
                Enumerable.Repeat(chars, 15)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            return randomId;
        }

    }
}