using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wfhealthapi.Classes.Output_Modals
{
    public class DoctorAppointmentsResultClass
    {

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

    }
}