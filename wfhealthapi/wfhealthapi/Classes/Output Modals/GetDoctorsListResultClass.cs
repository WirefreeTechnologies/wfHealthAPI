using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using wfhealthapi.Classes.Input_Modals;

namespace wfhealthapi.Classes.Output_Modals
{
    public class GetDoctorsListResultClass
    {
        public AccessTokenValidationModel Access { get; set; }
        public List<DoctorProfileOutputClass> Doctors { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrMessage { get; set; }
    }

    public class DoctorProfileOutputClass
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string DoctorEmail { get; set; }
        public string DoctorWebsite { get; set; }
        public string DoctorSkype { get; set; }

    }


    public class GetPatientsListResultClass
    {
        public AccessTokenValidationModel Access { get; set; }
        public List<PatientProfileOutputClass> Patients { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrMessage { get; set; }
    }

    public class PatientProfileOutputClass
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string PatientEmail { get; set; }
        public string PatientWebsite { get; set; }
        public string PatientSkype { get; set; }

    }


    public class BookAppointmentResult
    {
        public AccessTokenValidationModel Access { get; set; }
        public bool IsAppointmentBooked { get; set; }
        public int AptId { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrMessage { get; set; }
    }

}