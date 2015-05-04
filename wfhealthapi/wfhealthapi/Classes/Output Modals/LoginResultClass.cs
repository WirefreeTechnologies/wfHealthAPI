using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wfhealthapi.Classes.Output_Modals
{
    public class LoginResultClass
    {
        public int UserId { get; set; }
        public bool IsSuccess { get; set; }
        public string AccessToken { get; set; }
        public string ErrMessage { get; set; }
    }
}