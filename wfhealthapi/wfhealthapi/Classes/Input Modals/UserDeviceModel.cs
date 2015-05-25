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


    public class GetAppointmentInputModel
    {

        public int UserId { get; set; }

        public int HospitalId { get; set; }

        public string AccessToken { get; set; }
        public string DeviceType { get; set; }
        public string NotificationToken { get; set; }

        public string Lati { get; set; }

        public string Longi { get; set; }

    }

    public class BookAppointmentInput
    {

        public int UserId { get; set; }

        public int HospitalId { get; set; }

        public string AccessToken { get; set; }
        public string DeviceType { get; set; }
        public string NotificationToken { get; set; }

        public string Lati { get; set; }

        public string Longi { get; set; }

        public int DoctorID { get; set; }

    }



    public class AddFamilyMemberInputClass
    {
        public int UserId { get; set; }

        public int HospitalId { get; set; }

        public string AccessToken { get; set; }
        public string DeviceType { get; set; }
        public string NotificationToken { get; set; }

        public string Lati { get; set; }

        public string Longi { get; set; }

        public FamillyMemberInput FamilyMember { get; set; }
    }



    public class BookAppointmentInputClass
    {
        public int UserId { get; set; }

        public int HospitalId { get; set; }

        public string AccessToken { get; set; }
        public string DeviceType { get; set; }
        public string NotificationToken { get; set; }

        public string Lati { get; set; }

        public string Longi { get; set; }

        public string AptType { get; set; }


        public int DoctorId { get; set; }

        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string AppointmentReason { get; set; }

        public bool IsForFamMember { get; set; }
        public int FamMemberId { get; set; }

    }

    public class WritePrescriptionInputClass
    {
        public int UserId { get; set; }

        public int HospitalId { get; set; }
        public int AppointmentId { get; set; }
        public int PrescriptionId { get; set; }

        public string AccessToken { get; set; }
        public string DeviceType { get; set; }
        public string NotificationToken { get; set; }

        public string Lati { get; set; }

        public string Longi { get; set; }
        public string PrescriptionNotes { get; set; }


        public int DoctorId { get; set; }


        public bool IsForFamMember { get; set; }
        public int FamMemberId { get; set; }

        public List<PrescriptopmImageClass> PrescriptionAttachments { get; set; }

    }


    public class PrescriptionOutputInternal
    {
        public string PrescriptionNotes { get; set; }

        public bool IsForFamMember { get; set; }
        public int FamMemberId { get; set; }

        public int PrescriptionId { get; set; }
        public DateTime? WrittenOn { get; set; }

        public List<PrescriptopmImageClass> PrescriptionAttachments { get; set; }
    }

    public class PrescriptopmImageClass
    {
        public string PrescriptionImg { get; set; }
        public int AttachmentId { get; set; }
        public string attachementNotes { get; set; }
    }

    public class CancelAppointmentInputClass
    {
        public userInfoInputClass user { get; set; }
        public int AppointmentId { get; set; }
    }


    public class userInfoInputClass
    {
        public int UserId { get; set; }

        public int HospitalId { get; set; }

        public string AccessToken { get; set; }
        public string DeviceType { get; set; }
        public string NotificationToken { get; set; }

        public string Lati { get; set; }

        public string Longi { get; set; }
    }


    public class GetFAmilyMembersResultClass
    {
        public AccessTokenValidationModel Access { get; set; }

        public bool IsSuccess { get; set; }
        public string ErrMessage { get; set; }

        public List<FamillyMemberInput> FamilyMembers { get; set; }

    }

    public class DeleteFamilyMemberInputClass
    {
        public int UserId { get; set; }

        public int HospitalId { get; set; }

        public string AccessToken { get; set; }
        public string DeviceType { get; set; }
        public string NotificationToken { get; set; }

        public string Lati { get; set; }

        public string Longi { get; set; }

        public int FamilyMemberId { get; set; }
    }

    public class FamillyMemberInput
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public string RelationWithPatient { get; set; }
        public DateTime DOB { get; set; }
        public int Id { get; set; }
    }



    // get prescriptions result output class
    public class GetPrescriptionResultClass
    {
        public AccessTokenValidationModel Access { get; set; }

        public bool IsSuccess { get; set; }
        public string ErrMessage { get; set; }

        public List<PrescriptionOutputInternal> prescriptions { get; set; }



    }


}