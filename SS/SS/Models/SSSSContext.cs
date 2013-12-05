using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace SS.Models
{

    public class SSSSContext : DbContext
    {
        public DbSet<Instructor> Instructor { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<CourseRule> Rule { get; set; }
        public DbSet<Section> Section { get; set; }
        public DbSet<Semester> Semester { get; set; }
        public DbSet<Room> Room { get; set; }
        public DbSet<SectionConflict> SectionConflict { get; set; }
        public DbSet<UserModel> User { get; set; }
        public DbSet<AccountModel> Account { get; set; }
        public DbSet<RoleModel> Role { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Either entirely
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            // or for one model object
            modelBuilder.Entity<SectionConflict>()
               .HasRequired(p => p.parentSection)
               .WithMany()
               .HasForeignKey(p => p.SectionId)
               .WillCascadeOnDelete(true);
        }
    }
}