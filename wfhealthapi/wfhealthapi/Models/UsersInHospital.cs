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
    
    public partial class UsersInHospital
    {
        public int Id { get; set; }
        public Nullable<int> Hospital_Id { get; set; }
        public Nullable<int> User_Id { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        public virtual HospitalsMaster HospitalsMaster { get; set; }
        public virtual UsersMaster UsersMaster { get; set; }
    }
}
