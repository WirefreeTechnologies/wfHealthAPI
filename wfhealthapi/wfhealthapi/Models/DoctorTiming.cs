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
    
    public partial class DoctorTiming
    {
        public int Id { get; set; }
        public Nullable<int> Doctor_Id { get; set; }
        public string Weekday { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public Nullable<bool> IsAvailable { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        public virtual UsersMaster UsersMaster { get; set; }
    }
}
