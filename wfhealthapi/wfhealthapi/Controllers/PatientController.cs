using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using wfhealthapi.Classes;
using wfhealthapi.Classes.Input_Modals;
using wfhealthapi.Classes.Output_Modals;
using wfhealthapi.Models;

namespace wfhealthapi.Controllers
{
    public class PatientController : ApiController
    {
        EncryptDecrypt encDec = new EncryptDecrypt();
        [HttpPost]
        [ActionName("Login")]
        public LoginResultClass Login(LoginInputClass User)
        {
            LoginResultClass lr = new LoginResultClass();
            Utility ut = new Utility();
            try
            {
                using (wfhealthdbEntities obj = new wfhealthdbEntities())
                {
                    if (String.IsNullOrEmpty(User.Hospital_Id))
                    {
                        lr.IsSuccess = false;
                        lr.ErrMessage = "Hospital Id not provided.";
                        lr.AccessToken = null;
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(User.UserName))
                        {
                            lr.IsSuccess = false;
                            lr.ErrMessage = "Username not provided.";
                            lr.AccessToken = null;
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(User.Password))
                            {
                                lr.IsSuccess = false;
                                lr.ErrMessage = "Password not provided.";
                                lr.AccessToken = null;
                            }
                            else
                            {
                                // got everything going in for validation
                                string encUsername = encDec.Encrypt(User.UserName);
                                string encPassword = encDec.Encrypt(User.Password);

                                var UserRecord =
                                    (from c in obj.UsersMasters
                                     where
                                         c.UserName == encUsername && c.Password == encPassword && c.Role_Id == 4 &&
                                         c.IsActive == true
                                     select c).SingleOrDefault();


                                if (UserRecord != null)
                                {
                                    int hospId = Convert.ToInt16(User.Hospital_Id);
                                    // got user, checking hospital
                                    var hospRecord =
                                        (from c in obj.UsersInHospitals
                                         where
                                             c.Hospital_Id == hospId && c.User_Id == UserRecord.Id &&
                                             c.IsActive == true
                                         select c).SingleOrDefault();
                                    if (hospRecord != null)
                                    {
                                        // valid user, setting device type, token and other stuff
                                        DateTime currentTime = DateTime.Now;
                                        UserRecord.LastSeenOn = currentTime;
                                        UserRecord.DeviceType = User.DeviceType.Trim();
                                        UserRecord.NotificationToken = User.NotificationToken.Trim();
                                        UserRecord.AccessToken = ut.GetRandomToken();

                                        obj.SaveChanges();

                                        lr.IsSuccess = true;
                                        lr.ErrMessage = "OK";
                                        lr.AccessToken = UserRecord.AccessToken;
                                        lr.UserId = UserRecord.Id;

                                    }
                                    else
                                    {
                                        // user not associated with hospital
                                        lr.IsSuccess = false;
                                        lr.ErrMessage = "User not found in given hospital.";
                                        lr.AccessToken = null;
                                    }

                                }
                                else
                                {
                                    // invalid credentials
                                    lr.IsSuccess = false;
                                    lr.ErrMessage = "Invalid Username or Password.";
                                    lr.AccessToken = null;
                                }
                            }

                        }
                    }
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
