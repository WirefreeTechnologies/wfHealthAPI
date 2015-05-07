using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using wfhealthapi.Classes.Input_Modals;

namespace wfhealthapi.Classes.Output_Modals
{
    public class DoctorAppointmentsResultClass
    {
        public AccessTokenValidationModel Access { get; set; }
        public List<DoctorAppointmentsResultClassInternal> Appointments { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrMessage { get; set; }
    }


    public class DoctorAppointmentsResultClassInternal
    {
        public string aptDate { get; set; }

        public List<DoctorAppointmentsResultClassInternalTimingsInDate> appointments { get; set; }
        
    }

    public class DoctorAppointmentsResultClassInternalTimingsInDate
    {
        
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public string AptReason { get; set; }
        public string AptId { get; set; }
        public string AptType { get; set; }

    }
}