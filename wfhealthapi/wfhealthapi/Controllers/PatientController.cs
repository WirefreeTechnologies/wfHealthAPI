using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using wfhealthapi.Classes.Input_Modals;
using wfhealthapi.Classes.Output_Modals;
using wfhealthapi.Models;

namespace wfhealthapi.Controllers
{
    public class PatientController : ApiController
    {
        [HttpPost]
        [ActionName("Login")]
        public LoginResultClass Login(LoginInputClass User)
        {
            LoginResultClass lr = new LoginResultClass();
            try
            {
                using (wfhealthdbEntities obj = new wfhealthdbEntities())
                {
                    
                }
                return lr;
            }
            catch (Exception)
            {

                lr.IsSuccess = false;
                lr.ErrMessage = "Server Error";
                lr.AccessToken = null;
                return lr;
            }
        }
    }
}
