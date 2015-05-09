using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using wfhealthapi.Classes.Input_Modals;
using wfhealthapi.Models;

namespace wfhealthapi.Classes
{
    public class TokenAuthorization
    {
        

        public string GetNewAccessToken()
        {
            EncryptDecrypt obj = new EncryptDecrypt();
            string s = Utility.GetRandomToken();
            string TokenString = obj.Encrypt(s + DateTime.Now.ToShortDateString());
            return TokenString;
        }


        public AccessTokenValidationModel IsAccessTokenValid(int UserId, string AccessToken, string Lati, string Longi, string DeviceType, string NotificationToken)
        {
            AccessTokenValidationModel re = new AccessTokenValidationModel();


            using (wfhealthdbEntities obj = new wfhealthdbEntities())
            {
                var r = (from s in obj.UsersMasters
                         where s.Id == UserId && s.AccessToken == AccessToken && s.IsActive == true
                         select
                             s).SingleOrDefault();

                if (r != null)
                {

                    re.IsTokenValid = true;


                    DateTime currentTime = DateTime.UtcNow;

                    r.Lati = Lati;
                    r.Longi = Longi;
                    r.DeviceType = DeviceType;
                    r.NotificationToken = NotificationToken;
                   
                    DateTime lastseen = currentTime;
                    TimeSpan t;
                    if (r.LastSeenOn != null)
                    {
                        t = currentTime - Convert.ToDateTime(r.LastSeenOn);
                    }
                    else
                    {
                        t = currentTime - lastseen;
                    }


                    if (t.TotalMinutes > 20)
                    {
                        re.AccessToken = GetNewAccessToken();
                      
                        r.AccessToken = re.AccessToken;
                        r.LastSeenOn = lastseen;
                        re.IsTokenUpdated = true;
                        re.IsTokenValid = true;
                        re.ErrMessage = "OK";
                        obj.SaveChanges();


                    }


                    else
                    {

                        re.IsTokenUpdated = false;
                        re.AccessToken = AccessToken;
                        re.IsTokenValid = true;
                        re.ErrMessage = "OK";

                        
                    }


                }
                else
                {
                    re.IsTokenValid = false;
                    re.IsTokenUpdated= false;
                    re.ErrMessage = "Invalid access token or user id";
                }
                
                return re;
            }
        }

    }
}