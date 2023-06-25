using Microsoft.EntityFrameworkCore;
using ResourceManageGroup.Models;
namespace ResourceManageGroup.Data
{
    public class ApplicationDbContext:DbContext{
        public virtual DbSet<Recruiter> ? Recruiter_Details {get;set;}
        public virtual DbSet<Manager> ? Manager_Details {get;set;}
        public virtual DbSet<Employee> ? Employee_Details {get;set;}
        public virtual DbSet<Project> ? Project_Details {get;set;}
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options){
            
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder){

            modelBuilder.Entity<Recruiter>()
            .Property(m => m.RecruiterId)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("CONCAT('23HR', RIGHT('00' + CAST(NEXT VALUE FOR RecruiterSequence AS VARCHAR(2)), 2))");

            modelBuilder.HasSequence<int>("RecruiterSequence", schema: "dbo")
            .StartsAt(1)
            .IncrementsBy(1);
            
            modelBuilder.Entity<Manager>()
            .Property(m => m.ManagerId)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("CONCAT('23PM', RIGHT('00' + CAST(NEXT VALUE FOR ManagerSequence AS VARCHAR(2)), 2))");

            modelBuilder.HasSequence<int>("ManagerSequence", schema: "dbo")
            .StartsAt(1)
            .IncrementsBy(1);

             modelBuilder.Entity<Employee>()
            .Property(m => m.EmployeeId)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("CONCAT('23EM', RIGHT('00' + CAST(NEXT VALUE FOR EmployeeSequence AS VARCHAR(2)), 2))");

            modelBuilder.HasSequence<int>("EmployeeSequence", schema: "dbo")
            .StartsAt(1)
            .IncrementsBy(1);

            modelBuilder.Entity<Project>()
            .HasKey(u => u.project_id);
        }
    }
}