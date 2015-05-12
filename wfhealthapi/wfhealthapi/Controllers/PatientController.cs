using System;
using System.Collections.Generic;
using System.Data.Objects;
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


                            if (familyMemberDetail!=null)
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
        //[HttpPost]
        //[ActionName("GetAllFamilyMembers")]
        //public GetFAmilyMembersResultClass GetAllFamilyMembers(GetAppointmentInputModel aptmnt)
        //{
        //    GetFAmilyMembersResultClass r = new GetFAmilyMembersResultClass();

        //    try
        //    {
        //        AccessTokenValidationModel tokencheck = TokAuth.IsAccessTokenValid(aptmnt.UserId, aptmnt.AccessToken,
        //            aptmnt.Lati, aptmnt.Longi, aptmnt.DeviceType, aptmnt.NotificationToken);
        //    }
        //    catch (Exception ex)
        //    {
                
        //    }

        //}

        // to book appointment with doctor
        //[HttpPost]
        //[ActionName("BookAppointment")]
        //public 
    }
}
