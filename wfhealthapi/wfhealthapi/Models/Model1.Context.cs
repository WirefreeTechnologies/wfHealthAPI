﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class wfhealthdbEntities : DbContext
    {
        public wfhealthdbEntities()
            : base("name=wfhealthdbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<DoctorTiming> DoctorTimings { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<HospitalsMaster> HospitalsMasters { get; set; }
        public DbSet<PrescriptionAttachment> PrescriptionAttachments { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<ReferralsSentByDoctor> ReferralsSentByDoctors { get; set; }
        public DbSet<ReferralsSentByPatient> ReferralsSentByPatients { get; set; }
        public DbSet<RoleMaster> RoleMasters { get; set; }
        public DbSet<SuperAdminUser> SuperAdminUsers { get; set; }
        public DbSet<UsersInHospital> UsersInHospitals { get; set; }
        public DbSet<UsersMaster> UsersMasters { get; set; }
        public DbSet<UsersDetail> UsersDetails { get; set; }
        public DbSet<LabTests_LKP> LabTests_LKP { get; set; }
        public DbSet<PatientsFamilyMember> PatientsFamilyMembers { get; set; }
        public DbSet<Specialities_LKP> Specialities_LKP { get; set; }
    }
}
