using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CollectIt.Web.Models
{
    public class CollectItDbContext : IdentityDbContext<CollectItUser>
    {
        public CollectItDbContext() : base("DefaultSqlConnection", throwIfV1Schema: false) { }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CollectItUser>().ToTable("Users");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
        }

        public static CollectItDbContext Create()
        {
            return new CollectItDbContext();
        }

        public static void Initialize()
        {
            Create().Database.Initialize(false);
        }
    }
}