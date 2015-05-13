using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wfhealthapi.Classes.Input_Modals
{
    public class LoginInputClass
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Hospital_Id { get; set; }
        public string DeviceType { get; set; }

        public string NotificationToken { get; set; }
        public string FBId { get; set; }
        public string FBeMail { get; set; }
        
    }
}