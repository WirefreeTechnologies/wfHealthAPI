using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Objects.SqlClient;
using System.Diagnostics.CodeAnalysis;
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

        // to login with facebook
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
                                // checking if user already exists or not

                                var UserRecord =
                                    (from c in obj.UsersMasters
                                     where
                                         c.IsFBConnect == true && c.FbId == User.FBId && c.eMail == User.FBeMail && c.Role_Id == 4 &&
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
                                    // make new entry
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

        // getting list of doctors for patient
        [HttpPost]
        [ActionName("GetDoctorsList")]
        public GetDoctorsListResultClass GetDoctorsList(GetAppointmentInputModel aptmnt)
        {

            GetDoctorsListResultClass res = new GetDoctorsListResultClass();
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
                            (from c in obj.UsersMasters where c.Id == aptmnt.UserId && c.Role_Id == 4 && c.IsActive == true select c)
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

                                // getting all doctors in given hospital
                                var allDoctors = (from c in obj.UsersMasters
                                                  join d in obj.UsersInHospitals on c.Id equals d.User_Id





                                                  where c.Role_Id == 2 && d.Hospital_Id == aptmnt.HospitalId && d.IsActive == true
                                                  select new DoctorProfileOutputClass
                                                  {
                                                      DoctorEmail = c.eMail,
                                                      DoctorId = c.Id,
                                                      DoctorName = c.Name


                                                  }).ToList();

                                foreach (DoctorProfileOutputClass doc in allDoctors)
                                {
                                    var docdetails = (from c in obj.UsersDetails where c.User_Id == doc.DoctorId select c).SingleOrDefault();

                                    if (docdetails != null)
                                    {
                                        doc.DoctorSkype = docdetails.skypeId;
                                        doc.DoctorWebsite = docdetails.Website;
                                    }
                                }

                                res.IsSuccess = true;
                                res.ErrMessage = "OK";
                                res.Doctors = allDoctors;


                            }
                            else
                            {
                                res.ErrMessage = "Given user is not associated with Given hospital.";
                                res.IsSuccess = false;

                            }


                        }
                        else
                        {
                            res.ErrMessage = "Given user is not a Patient.";
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

        [HttpPost]
        [ActionName("AddFamilyMember")]
        public AddFamilyMemberResultClass AddFamilyMember(AddFamilyMemberInputClass aptmnt)
        {
            AddFamilyMemberResultClass res = new AddFamilyMemberResultClass();

            try
            {
                AccessTokenValidationModel tokencheck = TokAuth.IsAccessTokenValid(aptmnt.UserId, aptmnt.AccessToken,
                    aptmnt.Lati, aptmnt.Longi, aptmnt.DeviceType, aptmnt.NotificationToken);

                res.Access = tokencheck;
                if (res.Access.IsTokenValid == true)
                {
                    if (String.IsNullOrEmpty(aptmnt.FamilyMember.Name) == true)
                    {
                        res.IsSuccess = false;
                        res.ErrMessage = "Family Member name required.";
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(aptmnt.FamilyMember.RelationWithPatient) == true)
                        {
                            res.IsSuccess = false;
                            res.ErrMessage = "Family Relation with patient required.";
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(aptmnt.FamilyMember.Gender) == true)
                            {
                                res.IsSuccess = false;
                                res.ErrMessage = "Family Member Gender required.";
                            }
                            else
                            {
                                if ((aptmnt.FamilyMember.DOB) == null)
                                {
                                    res.IsSuccess = false;
                                    res.ErrMessage = "Family Member DOB required.";
                                }
                                else
                                {

                                    PatientsFamilyMember pfm = new PatientsFamilyMember();
                                    pfm.Name = aptmnt.FamilyMember.Name.Trim();
                                    pfm.Gender = aptmnt.FamilyMember.Gender.Trim();
                                    pfm.RelationWithPatient = aptmnt.FamilyMember.RelationWithPatient.Trim();

                                    pfm.DOB = aptmnt.FamilyMember.DOB;

                                    pfm.PatientId = aptmnt.UserId;
                                    pfm.HospitalId = aptmnt.HospitalId;

                                    pfm.CreatedOn = DateTime.UtcNow;

                                    pfm.IsActive = true;

                                    using (wfhealthdbEntities obj = new wfhealthdbEntities())
                                    {
                                        obj.PatientsFamilyMembers.Add(pfm);
                                        obj.SaveChanges();
                                        res.IsSuccess = true;
                                        res.ErrMessage = "Family Member added succesfully.";

                                    }




                                }
                            }
                        }
                    }
                }
                else
                {
                    res.IsSuccess = false;
                    res.ErrMessage = "Invalid access token.";
                }
                return res;

            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.ErrMessage = ex.ToString();
                return res;
            }
        }


        // to delete given family member
        [HttpPost]
        [ActionName("DeleteFamilyMember")]
        public AddFamilyMemberResultClass DeleteFamilyMember(DeleteFamilyMemberInputClass aptmnt)
        {
            AddFamilyMemberResultClass res = new AddFamilyMemberResultClass();

            try
            {
                AccessTokenValidationModel tokencheck = TokAuth.IsAccessTokenValid(aptmnt.UserId, aptmnt.AccessToken,
                    aptmnt.Lati, aptmnt.Longi, aptmnt.DeviceType, aptmnt.NotificationToken);

                res.Access = tokencheck;
                if (res.Access.IsTokenValid == true)
                {
                    if (aptmnt.FamilyMemberId == 0 || aptmnt.FamilyMemberId == null)
                    {
                        res.IsSuccess = false;
                        res.ErrMessage = "Family Member Id required.";
                    }
                    else
                    {
                        // cehcking if given family member is associated with given patien id
                        using (wfhealthdbEntities obj = new wfhealthdbEntities())
                        {
                            var familyMemberDetail = (from c in obj.PatientsFamilyMembers
                                                      where
                                                          c.IsActive == true && c.HospitalId == aptmnt.HospitalId &&
                                                          c.PatientId == aptmnt.UserId && c.Id == aptmnt.FamilyMemberId
                                                      select c).SingleOrDefault();


                            if (familyMemberDetail != null)
                            {
                                familyMemberDetail.IsActive = false;
                                obj.SaveChanges();
                                res.IsSuccess = true;
                                res.ErrMessage = "Given family member deleted successfully.";
                            }
                            else
                            {
                                //not found
                                res.IsSuccess = false;
                                res.ErrMessage = "Given family member not found or invalid id supplied.";
                            }

                        }


                    }

                }

                else
                {
                    res.IsSuccess = false;
                    res.ErrMessage = "Invalid access token.";
                }
                return res;

            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.ErrMessage = ex.ToString();
                return res;
            }

        }



        // to get all family members of patient
        [HttpPost]
        [ActionName("GetAllFamilyMembers")]
        public GetFAmilyMembersResultClass GetAllFamilyMembers(GetAppointmentInputModel aptmnt)
        {
            GetFAmilyMembersResultClass r = new GetFAmilyMembersResultClass();

            try
            {
                AccessTokenValidationModel tokencheck = TokAuth.IsAccessTokenValid(aptmnt.UserId, aptmnt.AccessToken,
                    aptmnt.Lati, aptmnt.Longi, aptmnt.DeviceType, aptmnt.NotificationToken);
                r.Access = tokencheck;
                if (r.Access.IsTokenValid == true)
                {
                    //getting all family members of patient
                    using (wfhealthdbEntities obj = new wfhealthdbEntities())
                    {
                        var allfamilyMembers = (from c in obj.PatientsFamilyMembers
                                                where c.IsActive == true && c.HospitalId == aptmnt.HospitalId && c.PatientId == aptmnt.UserId
                                                select


                                                    c).ToList();

                        List<FamillyMemberInput> mm = new List<FamillyMemberInput>();
                        foreach (PatientsFamilyMember pfm in allfamilyMembers)
                        {
                            FamillyMemberInput f = new FamillyMemberInput();
                            f.Name = pfm.Name;
                            f.Gender = pfm.Gender;

                            f.DOB = Convert.ToDateTime(pfm.DOB);

                            f.RelationWithPatient = pfm.RelationWithPatient;
                            f.Id = pfm.Id;
                            mm.Add(f);
                        }
                        r.FamilyMembers = mm;
                        r.ErrMessage = "OK";
                        r.IsSuccess = true;


                    }


                }
                else
                {
                    r.ErrMessage = "Invalid access token.";
                    r.IsSuccess = false;
                    r.FamilyMembers = null;
                }
                return r;
            }
            catch (Exception ex)
            {
                r.ErrMessage = "Server Error.";
                r.IsSuccess = false;
                r.FamilyMembers = null;
                return r;

            }

        }


        // to edit familyMember details
        [HttpPost]
        [ActionName("EditFamilyMember")]
        public AddFamilyMemberResultClass EditFamilyMember(AddFamilyMemberInputClass aptmnt)
        {
            AddFamilyMemberResultClass res = new AddFamilyMemberResultClass();

            try
            {
                AccessTokenValidationModel tokencheck = TokAuth.IsAccessTokenValid(aptmnt.UserId, aptmnt.AccessToken,
                    aptmnt.Lati, aptmnt.Longi, aptmnt.DeviceType, aptmnt.NotificationToken);

                res.Access = tokencheck;
                if (res.Access.IsTokenValid == true)
                {
                    if (aptmnt.FamilyMember.Id == 0 || aptmnt.FamilyMember.Id == null)
                    {
                        res.IsSuccess = false;
                        res.ErrMessage = "Family Member Id required.";
                    }
                    else
                    {
                        // cehcking if given family member is associated with given patien id
                        using (wfhealthdbEntities obj = new wfhealthdbEntities())
                        {
                            var familyMemberDetail = (from c in obj.PatientsFamilyMembers
                                                      where
                                                          c.IsActive == true && c.HospitalId == aptmnt.HospitalId &&
                                                          c.PatientId == aptmnt.UserId && c.Id == aptmnt.FamilyMember.Id
                                                      select c).SingleOrDefault();


                            if (familyMemberDetail != null)
                            {

                                if (String.IsNullOrEmpty(aptmnt.FamilyMember.Name) == true)
                                {
                                    res.IsSuccess = false;
                                    res.ErrMessage = "Family Member name required.";
                                }
                                else
                                {
                                    if (String.IsNullOrEmpty(aptmnt.FamilyMember.RelationWithPatient) == true)
                                    {
                                        res.IsSuccess = false;
                                        res.ErrMessage = "Family Relation with patient required.";
                                    }
                                    else
                                    {
                                        if (String.IsNullOrEmpty(aptmnt.FamilyMember.Gender) == true)
                                        {
                                            res.IsSuccess = false;
                                            res.ErrMessage = "Family Member Gender required.";
                                        }
                                        else
                                        {
                                            if ((aptmnt.FamilyMember.DOB) == null)
                                            {
                                                res.IsSuccess = false;
                                                res.ErrMessage = "Family Member DOB required.";
                                            }
                                            else
                                            {

                                                familyMemberDetail.Name = aptmnt.FamilyMember.Name.Trim();
                                                familyMemberDetail.Gender = aptmnt.FamilyMember.Gender.Trim();
                                                familyMemberDetail.RelationWithPatient =
                                                    aptmnt.FamilyMember.RelationWithPatient.Trim();

                                                familyMemberDetail.DOB = aptmnt.FamilyMember.DOB;

                                                familyMemberDetail.HospitalId = aptmnt.HospitalId;





                                                obj.SaveChanges();
                                                res.IsSuccess = true;
                                                res.ErrMessage = "Given family details updated successfully.";
                                            }



                                        }
                                    }
                                }
                            }
                            else
                            {
                                //not found
                                res.IsSuccess = false;
                                res.ErrMessage = "Given family member not found or invalid id supplied.";
                            }

                        }


                    }

                }

                else
                {
                    res.IsSuccess = false;
                    res.ErrMessage = "Invalid access token.";
                }
                return res;

            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.ErrMessage = ex.ToString();
                return res;
            }

        }


        [HttpGet]
        [ActionName("testServe")]
        public void testServe()
        {
            string appointmentDayName = DateTime.Now.DayOfWeek.ToString();
            string appointmentTimeChoosen = "9:30 AM";


           
            DateTime aptmntTo =
                Convert.ToDateTime(appointmentTimeChoosen)
                    .AddMinutes(Convert.ToInt16(Utility.GetSettingValue("AptSlot")));
            string TimeTo = aptmntTo.ToShortTimeString();   // need to verify

            //using (wfhealthdbEntities obj = new wfhealthdbEntities())
            //{

            //    var allTimings =
            //        (from c in obj.DoctorTimings where c.IsActive == true && c.Doctor_Id == 1 select c).ToList();

            //    foreach (DoctorTiming dt in allTimings)
            //    {
            //        if (dt.Weekday == appointmentDayName)
            //        {
            //            DateTime fromTime = Convert.ToDateTime(dt.TimeFrom);
            //            DateTime toTime = Convert.ToDateTime(dt.TimeTo);

            //            DateTime giventimenTime = Convert.ToDateTime("11:30 AM");




            //            // checking if given time lies between from and to time
            //            if (fromTime < giventimenTime && toTime > giventimenTime)
            //            {
            //                string came = "";
            //            }
            //            else
            //            {
            //                string cameFalse = "";

            //            }

            //        }
            //    }
            //}
        }

        // to book appointment with doctor
        [HttpPost]
        [ActionName("BookAppointment")]
        public AddFamilyMemberResultClass BookAppointment(BookAppointmentInputClass aptmnt)
        {
            AddFamilyMemberResultClass res = new AddFamilyMemberResultClass();
            using (wfhealthdbEntities obj = new wfhealthdbEntities())
            {
                try
                {

                    AccessTokenValidationModel tokencheck = TokAuth.IsAccessTokenValid(aptmnt.UserId, aptmnt.AccessToken,
                   aptmnt.Lati, aptmnt.Longi, aptmnt.DeviceType, aptmnt.NotificationToken);

                    res.Access = tokencheck;
                    if (res.Access.IsTokenValid == true)
                    {
                        if (aptmnt.DoctorId == 0)
                        {
                            res.IsSuccess = false;
                            res.ErrMessage = "Doctor id not supplied.";
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(aptmnt.AppointmentReason))
                            {
                                res.IsSuccess = false;
                                res.ErrMessage = "Appointment reason required.";
                            }
                            else
                            {
                                if (aptmnt.IsForFamMember == true && aptmnt.FamMemberId == 0)
                                {
                                    res.IsSuccess = false;
                                    res.ErrMessage = "Family member not specified.";
                                }
                                else
                                {
                                    if (aptmnt.AppointmentDate == null)
                                    {
                                        res.IsSuccess = false;
                                        res.ErrMessage = "Appointment date required.";
                                    }
                                    else
                                    {
                                        if (String.IsNullOrEmpty(aptmnt.AppointmentTime))
                                        {
                                            res.IsSuccess = false;
                                            res.ErrMessage = "Appointment time required.";
                                        }
                                        else
                                        {
                                            // checking valid doctor

                                            int docRoleId = Utility.GetRoleId("Doctor");
                                            var doctorDetails = (from c in obj.UsersMasters
                                                                 where
                                                                     c.Role_Id == docRoleId && c.IsActive == true && c.Id == aptmnt.DoctorId
                                                                 select c).SingleOrDefault();

                                            if (doctorDetails == null)
                                            {
                                                res.IsSuccess = false;
                                                res.ErrMessage = "Invalid Doctor id supplied.";
                                                return res;
                                            }

                                            // cehcking doctor and hospital association
                                            var docHospCheck = (from d in obj.UsersInHospitals
                                                                where
                                                                    d.IsActive == true && d.Hospital_Id == aptmnt.HospitalId &&
                                                                    d.User_Id == aptmnt.DoctorId
                                                                select d).SingleOrDefault();

                                            if (docHospCheck == null)
                                            {
                                                res.IsSuccess = false;
                                                res.ErrMessage = "Doctor not found in given hospital.";
                                                return res;
                                            }

                                            // checking valid patient

                                            int patRoleId = Utility.GetRoleId("Patient");
                                            var patientDetails = (from c in obj.UsersMasters
                                                                  where
                                                                      c.Role_Id == patRoleId && c.IsActive == true && c.Id == aptmnt.UserId
                                                                  select c).SingleOrDefault();

                                            if (patientDetails == null)
                                            {
                                                res.IsSuccess = false;
                                                res.ErrMessage = "Invalid Patient id supplied.";
                                                return res;
                                            }
                                            // checking patient and hospital association
                                            // cehcking doctor and hospital association
                                            var patHospCheck = (from d in obj.UsersInHospitals
                                                                where
                                                                    d.IsActive == true && d.Hospital_Id == aptmnt.HospitalId &&
                                                                    d.User_Id == aptmnt.UserId
                                                                select d).SingleOrDefault();

                                            if (patHospCheck == null)
                                            {
                                                res.IsSuccess = false;
                                                res.ErrMessage = "Patient not found in given hospital.";
                                                return res;
                                            }


                                            // if appoinemtn is for famiy member
                                            if (aptmnt.IsForFamMember == true)
                                            {
                                                // checking ig given member is associated with given patient id
                                                var famPatAssoc = (from e in obj.PatientsFamilyMembers
                                                                   where e.IsActive == true && e.PatientId == aptmnt.UserId &&
                                                                       e.Id == aptmnt.FamMemberId
                                                                   select e).SingleOrDefault();

                                                if (famPatAssoc == null)
                                                {
                                                    res.IsSuccess = false;
                                                    res.ErrMessage = "Given family is not assosicated with given patient.";
                                                    return res;
                                                }
                                            }
                                            // checking doctor schedule
                                            // getting day from date of appointment

                                            string appointmentDayName = aptmnt.AppointmentDate.DayOfWeek.ToString();
                                            DateTime appointmentTimeChoosen = Convert.ToDateTime(aptmnt.AppointmentTime);

                                            // checking if doctor is open for given weekday and timing
                                            var allTimings =
                    (from c in obj.DoctorTimings where c.IsActive == true && c.Doctor_Id == aptmnt.DoctorId select c).ToList();

                                            foreach (DoctorTiming dt in allTimings)
                                            {
                                                if (dt.Weekday == appointmentDayName)
                                                {
                                                    DateTime fromTime = Convert.ToDateTime(dt.TimeFrom);
                                                    DateTime toTime = Convert.ToDateTime(dt.TimeTo);

                                                    DateTime giventimenTime = Convert.ToDateTime(aptmnt.AppointmentTime);




                                                    // checking if given time lies between from and to time
                                                    if (fromTime < giventimenTime && toTime > giventimenTime)
                                                    {
                                                        // checking if doctor has any other appointment on given time
                                                        var existingaptmntongiventime = (from a in obj.Appointments
                                                                                         where (a.IsMeetingHeld == null || a.IsMeetingHeld == false)
                                                                                             && (a.IsCancelledByPat == null || a.IsCancelledByPat == false)
                                                                                             && a.AppointmentDate == aptmnt.AppointmentDate
                                                                                             && a.TimeFrom == aptmnt.AppointmentTime
                                                                                         select a).ToList();
                                                        if (existingaptmntongiventime.Count == 0)
                                                        {
                                                            // book appointemtn here
                                                            Appointment ap = new Appointment();
                                                            ap.AppointmentDate = aptmnt.AppointmentDate;

                                                            ap.Patient_Id = aptmnt.UserId;
                                                            ap.AptReason = encDec.Encrypt(aptmnt.AppointmentReason);
                                                            ap.CreatedOn = DateTime.UtcNow;
                                                            ap.Doctor_Id = aptmnt.DoctorId;
                                                            ap.Hospital_Id = aptmnt.HospitalId;
                                                            ap.AptType = aptmnt.AptType;



                                                            ap.TimeFrom = aptmnt.AppointmentTime;
                                                            DateTime aptmntTo =
                                                                Convert.ToDateTime(aptmnt.AppointmentTime)
                                                                    .AddMinutes(Convert.ToInt16(Utility.GetSettingValue("AptSlot")));
                                                            ap.TimeTo = aptmntTo.ToString("hh:mm tt");


                                                            if (aptmnt.IsForFamMember == true)
                                                            {
                                                                ap.IsAptForFamilyMember = aptmnt.IsForFamMember;
                                                                ap.FamilyMember_Id = aptmnt.FamMemberId;
                                                                ap.FamilyMemberName = "";

                                                            }



                                                            obj.Appointments.Add(ap);
                                                            obj.SaveChanges();

                                                            res.IsSuccess = true;
                                                            res.ErrMessage = "Appointment booked successfully.";
                                                            return res;


                                                        }
                                                        else
                                                        {
                                                            res.IsSuccess = false;
                                                            res.ErrMessage = "Doctor is busy on given time on given day.";
                                                            return res;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        res.IsSuccess = false;
                                                        res.ErrMessage = "Doctor is not available for given time on given day.";
                                                        return res;

                                                    }

                                                }
                                            }


                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        res.IsSuccess = false;
                        res.ErrMessage = "Invalid access token.";
                    }

                }
                catch (Exception)
                {

                    throw;
                }

            }
            return res;
        }
    }
}
