using AuthService.Data.Auth;
using AuthService.Data.EmailModel;
using AuthService.Data.UserDatas.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<PersistedLogin> PersistedLogins { get; set; }
        public DbSet<VerificationModel> Verifications { get; set; }
        public DbSet<SentEmailOtp> SentEmailOtps { get; set; }
    }

}
