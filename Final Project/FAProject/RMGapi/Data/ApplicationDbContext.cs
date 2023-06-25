using Microsoft.EntityFrameworkCore;
using ResourceManageGroup.Models;
namespace ResourceManageGroup.Data
{
    public class ApplicationDbContext:DbContext{
        
        public virtual DbSet<Sample> ? Sample_Details {get;set;}
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options){
            
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder){

            modelBuilder.Entity<Sample>()
            .HasKey(u => u.EmployeeId);
        }
    }
}