﻿using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web.Http;
using wfhealthapi.Classes;
using wfhealthapi.Classes.Input_Modals;
using wfhealthapi.Classes.Output_Modals;
using wfhealthapi.Models;

namespace wfhealthapi.Controllers
{
    public class DoctorController : ApiController
    {
        EncryptDecrypt encDec = new EncryptDecrypt();
        TokenAuthorization TokAuth = new TokenAuthorization();
        [HttpPost]
        [ActionName("Login")]
        public LoginResultClass Login(LoginInputClass User)
        {
            LoginResultClass lr = new LoginResultClass();
           
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
                                         c.UserName == encUsername && c.Password == encPassword && c.Role_Id == 2 &&
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
                                        DateTime currentTime = DateTime.UtcNow;
                                        UserRecord.LastSeenOn = currentTime;
                                        UserRecord.DeviceType = User.DeviceType.Trim();
                                        UserRecord.NotificationToken = User.NotificationToken.Trim();
                                        UserRecord.AccessToken = Utility.GetRandomToken();

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


        // login with fb
        [HttpPost]
        [ActionName("LoginWithFB")]
        public LoginResultClass LoginWithFB(LoginInputClass User)
        {
            LoginResultClass lr = new LoginResultClass();

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
                        if (String.IsNullOrEmpty(User.FBeMail))
                        {
                            lr.IsSuccess = false;
                            lr.ErrMessage = "eMail not provided.";
                            lr.AccessToken = null;
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(User.FBId))
                            {
                                lr.IsSuccess = false;
                                lr.ErrMessage = "FBId not provided.";
                                lr.AccessToken = null;
                            }
                            else
                            {
                                // got everything going in for validation
                               

                                var UserRecord =
                                    (from c in obj.UsersMasters
                                     where
                                         c.IsFBConnect == true && c.FbId == User.FBId && c.eMail == User.FBeMail && c.Role_Id == 2 &&
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
                                        DateTime currentTime = DateTime.UtcNow;
                                        UserRecord.LastSeenOn = currentTime;
                                        UserRecord.DeviceType = User.DeviceType.Trim();
                                        UserRecord.NotificationToken = User.NotificationToken.Trim();
                                        UserRecord.AccessToken = Utility.GetRandomToken();

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
                                    lr.ErrMessage = "Given eMail id not registered with us.";
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


        // to get current week appointments of doctor
        [HttpPost]
        [ActionName("GetAppointments")]
        public DoctorAppointmentsResultClass GetAppointments(GetAppointmentInputModel aptmnt)
        {

            DoctorAppointmentsResultClass res = new DoctorAppointmentsResultClass();
            try
            {
                AccessTokenValidationModel tokencheck = TokAuth.IsAccessTokenValid(aptmnt.UserId, aptmnt.AccessToken,
                    aptmnt.Lati, aptmnt.Longi, aptmnt.DeviceType, aptmnt.NotificationToken);

                res.Access = tokencheck;
                if (res.Access.IsTokenValid == true)
                {
                    // getting appointments of docotr in given 
                    // checking if user is doctor and valid in given hospital
                    using (wfhealthdbEntities obj = new wfhealthdbEntities())
                    {
                        var userResult =
                            (from c in obj.UsersMasters where c.Id == aptmnt.UserId && c.Role_Id == 2 && c.IsActive == true select c)
                                .SingleOrDefault();
                        if (userResult != null)
                        {
                            // checking if given doctor is active in given hospital
                            var userInHospcheck = (from d in obj.UsersInHospitals
                                                   where
                                                       d.User_Id == aptmnt.UserId && d.Hospital_Id == aptmnt.HospitalId &&
                                                       d.IsActive == true
                                                   select d).SingleOrDefault();
                            if (userInHospcheck != null)
                            {

                                List<DoctorAppointmentsResultClassInternal> Appointments = new List<DoctorAppointmentsResultClassInternal>();
                                // getting all appointments of doctor
                                var allaptDates = (from r in obj.Appointments
                                                   where
                                                       r.Doctor_Id == aptmnt.UserId && r.Hospital_Id == aptmnt.HospitalId &&
                                                       (r.IsCancelledByPat == null || r.IsCancelledByPat == false) &&
                                                       (r.IsMeetingHeld == null || r.IsMeetingHeld == false)
                                                   select EntityFunctions.TruncateTime(r.AppointmentDate)).Distinct().ToList();

                                foreach (DateTime? aptDate in allaptDates)
                                {
                                    DoctorAppointmentsResultClassInternal aptInternal = new DoctorAppointmentsResultClassInternal();
                                    var allaptmntsOnDate = (from r in obj.Appointments
                                                            where
                                                               EntityFunctions.TruncateTime(r.AppointmentDate) == aptDate && r.Doctor_Id == aptmnt.UserId &&
                                                                r.Hospital_Id == aptmnt.HospitalId &&
                                                                (r.IsCancelledByPat == null || r.IsCancelledByPat == false) &&
                                                                (r.IsMeetingHeld == null || r.IsMeetingHeld == false)
                                                            select r).ToList();
                                    List<DoctorAppointmentsResultClassInternalTimingsInDate> appointmentsOfdate = new List<DoctorAppointmentsResultClassInternalTimingsInDate>();
                                    foreach (Appointment apt in allaptmntsOnDate)
                                    {
                                        DoctorAppointmentsResultClassInternalTimingsInDate currentApt = new DoctorAppointmentsResultClassInternalTimingsInDate();
                                        currentApt.AptId = apt.Id.ToString();
                                        currentApt.AptReason = encDec.Decrypt(apt.AptReason);

                                        currentApt.AptType = apt.AptType;
                                        currentApt.PatientId = apt.Patient_Id.ToString();
                                        currentApt.PatientName = apt.UsersMaster1.Name;

                                        currentApt.FromTime = apt.TimeFrom;
                                        currentApt.ToTime = apt.TimeTo;
                                        appointmentsOfdate.Add(currentApt);

                                    }

                                    aptInternal.aptDate = Convert.ToDateTime(aptDate).ToShortDateString();
                                    aptInternal.appointments = appointmentsOfdate;

                                    Appointments.Add(aptInternal);

                                    res.Appointments = Appointments;
                                    res.IsSuccess = true;
                                    res.ErrMessage = "OK";

                                }


                            }
                            else
                            {
                                res.ErrMessage = "Given user is not associated with Given hospital.";
                                res.IsSuccess = false;

                            }


                        }
                        else
                        {
                            res.ErrMessage = "Given user is not a Doctor.";
                            res.IsSuccess = false;

                        }
                    }


                }

                return res;
            }
            catch
            {
                res.ErrMessage = "Error occurred at server.";
                res.IsSuccess = false;
                return res;
            }


        }


        // getting list patients
        [HttpPost]
        [ActionName("GetPatientsList")]
        public GetPatientsListResultClass GetPatientsList(GetAppointmentInputModel aptmnt)
        {

            GetPatientsListResultClass res = new GetPatientsListResultClass();
            try
            {
                AccessTokenValidationModel tokencheck = TokAuth.IsAccessTokenValid(aptmnt.UserId, aptmnt.AccessToken,
                    aptmnt.Lati, aptmnt.Longi, aptmnt.DeviceType, aptmnt.NotificationToken);

                res.Access = tokencheck;
                if (res.Access.IsTokenValid == true)
                {
                    // getting appointments of docotr in given 
                    // checking if user is doctor and valid in given hospital
                    using (wfhealthdbEntities obj = new wfhealthdbEntities())
                    {
                        int roleid = Utility.GetRoleId("Doctor");
                        var userResult =
                            (from c in obj.UsersMasters where c.Id == aptmnt.UserId && c.Role_Id == roleid && c.IsActive == true select c)
                                .SingleOrDefault();
                        if (userResult != null)
                        {
                            // checking if given doctor is active in given hospital
                            var userInHospcheck = (from d in obj.UsersInHospitals
                                                   where
                                                       d.User_Id == aptmnt.UserId && d.Hospital_Id == aptmnt.HospitalId &&
                                                       d.IsActive == true
                                                   select d).SingleOrDefault();
                            if (userInHospcheck != null)
                            {
                                roleid = Utility.GetRoleId("Patient");
                                // getting all doctors in given hospital
                                var allDoctors = (from c in obj.UsersMasters
                                                  join d in obj.UsersInHospitals on c.Id equals d.User_Id





                                                  where c.Role_Id == roleid  && d.Hospital_Id == aptmnt.HospitalId && d.IsActive == true
                                                  select new PatientProfileOutputClass
                                                  {
                                                      PatientEmail = c.eMail,
                                                      PatientId = c.Id,
                                                      PatientName = c.Name


                                                  }).ToList();

                                foreach (PatientProfileOutputClass doc in allDoctors)
                                {
                                    var docdetails = (from c in obj.UsersDetails where c.User_Id == doc.PatientId select c).SingleOrDefault();

                                    if (docdetails != null)
                                    {
                                        doc.PatientSkype = docdetails.skypeId;
                                        doc.PatientWebsite = docdetails.Website;
                                    }
                                }

                                res.IsSuccess = true;
                                res.ErrMessage = "OK";
                                res.Patients = allDoctors;


                            }
                            else
                            {
                                res.ErrMessage = "Given user is not associated with Given hospital.";
                                res.IsSuccess = false;

                            }


                        }
                        else
                        {
                            res.ErrMessage = "Given user is not a Doctor.";
                            res.IsSuccess = false;

                        }
                    }


                }

                return res;
            }
            catch
            {
                res.ErrMessage = "Error occurred at server.";
                res.IsSuccess = false;
                return res;
            }



        }
    }
}
