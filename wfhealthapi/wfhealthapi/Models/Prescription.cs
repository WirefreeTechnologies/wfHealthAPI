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
    
    public partial class Prescription
    {
        public Prescription()
        {
            this.PrescriptionAttachments = new HashSet<PrescriptionAttachment>();
        }
    
        public int Id { get; set; }
        public Nullable<int> Appointment_Id { get; set; }
        public Nullable<int> Hospital_Id { get; set; }
        public Nullable<int> Doctor_Id { get; set; }
        public Nullable<int> Patient_Id { get; set; }
        public string PrescriptionText { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<bool> IsArchivedByDoc { get; set; }
        public Nullable<bool> IsArchivedByPat { get; set; }
        public Nullable<bool> IsForFamilyMember { get; set; }
        public Nullable<int> FamilyMemberId { get; set; }
    
        public virtual Appointment Appointment { get; set; }
        public virtual HospitalsMaster HospitalsMaster { get; set; }
        public virtual ICollection<PrescriptionAttachment> PrescriptionAttachments { get; set; }
        public virtual UsersMaster UsersMaster { get; set; }
        public virtual UsersMaster UsersMaster1 { get; set; }
    }
}
