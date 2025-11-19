using LibraryManagement.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace LibraryManagement.Data
{
    public class AppDbContext : IdentityDbContext


    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<BookModel> Books{ get; set; }  
    }
}
