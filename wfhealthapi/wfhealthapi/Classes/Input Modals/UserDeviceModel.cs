using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wfhealthapi.Classes.Input_Modals
{
    public class UserDeviceModel
    {
        public int UserId { get; set; }
        
        public string Password { get; set; }
        
        public string Name { get; set; }
        public string Gender { get; set; }
        public string ContactNum { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        
        public string DeviceType { get; set; }
        public string NotificationToken { get; set; }
        public string Lati { get; set; }
        public string Longi { get; set; }
        public string eMail { get; set; }
        
        public string ImagePath { get; set; }
        public string DOB { get; set; }
        public string AccessToken { get; set; }
    }



    public class UserLoginModel
    {
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Lati { get; set; }
        public string Longi { get; set; }

        public int HospitalId { get; set; }


    }

    public class AccessTokenValidationModel
    {
        public bool IsTokenUpdated { get; set; }
        public bool IsTokenValid { get; set; }
        public string AccessToken { get; set; }
        
        public string ErrMessage { get; set; }

        


    }


}