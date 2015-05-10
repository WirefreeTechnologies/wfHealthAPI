//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace wfhealthapi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Appointment
    {
        public Appointment()
        {
            this.PrescriptionAttachments = new HashSet<PrescriptionAttachment>();
            this.Prescriptions = new HashSet<Prescription>();
            this.ReferralsSentByDoctors = new HashSet<ReferralsSentByDoctor>();
        }
    
        public int Id { get; set; }
        public Nullable<System.DateTime> AppointmentDate { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public string AptReason { get; set; }
        public Nullable<bool> IsApprovedByDoc { get; set; }
        public Nullable<bool> IsCancelledByPat { get; set; }
        public Nullable<bool> IsMeetingHeld { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string DoctorRemarks { get; set; }
        public string PatientRemarks { get; set; }
        public Nullable<int> Doctor_Id { get; set; }
        public Nullable<int> Patient_Id { get; set; }
        public Nullable<int> Hospital_Id { get; set; }
        public string AptType { get; set; }
        public Nullable<bool> IsAptForFamilyMember { get; set; }
        public Nullable<int> FamilyMember_Id { get; set; }
        public string FamilyMemberName { get; set; }
    
        public virtual HospitalsMaster HospitalsMaster { get; set; }
        public virtual ICollection<PrescriptionAttachment> PrescriptionAttachments { get; set; }
        public virtual ICollection<Prescription> Prescriptions { get; set; }
        public virtual ICollection<ReferralsSentByDoctor> ReferralsSentByDoctors { get; set; }
        public virtual UsersMaster UsersMaster { get; set; }
        public virtual UsersMaster UsersMaster1 { get; set; }
    }
}
