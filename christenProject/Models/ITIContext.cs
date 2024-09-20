using christenProject.Authontication;
using christenProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace christenProject.Model
{
    public class ITIContext:IdentityDbContext<AppUser>
    {
        public ITIContext(DbContextOptions<ITIContext> dbContext):base(dbContext) { }
        public DbSet<DepartmentModel> Department { get; set; }
        public DbSet<ProductModel> Product { get; set; }
        public DbSet<EmployeeModel> Employee { get; set; }
        public DbSet<CategoryModel> Category { get; set; }
    }
    //public class Y(DbContextOptions<ITIContext> dbContextOptions) : DbContext(dbContextOptions)
    //{
    //    public DbSet<Department> Department => Set<Department>();
    //}
}
