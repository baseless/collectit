using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CollectIt.Web.Models
{
    public class CollectItDbInitializer : DropCreateDatabaseIfModelChanges<CollectItDbContext>
    {
        public override void InitializeDatabase(CollectItDbContext context)
        {
            base.InitializeDatabase(context);
        }

        protected override void Seed(CollectItDbContext context)
        {
            var adminRole = new IdentityRole {Name = "Administrator", Id = Guid.NewGuid().ToString()};
            var memberRole = new IdentityRole {Name = "Member", Id = Guid.NewGuid().ToString()};
            var admin1Id = Guid.NewGuid().ToString();
            var admin1 = new CollectItUser
            {
                Id = admin1Id,
                UserName = "daniel@ryhle.se",
                Email = "daniel@ryhle.se",
                PasswordHash = "AJA+loB28qP6AT6WqX7FLoZaSXWeflN3qviv2c7wRzAjsz7CnoF62iuknf4A46+Llw==",
                SecurityStamp = "5b5db254-03a3-4c96-891f-a6dd9d50f9bf",
                LockoutEnabled = false, //Går ej låsa detta kontot då det är admin
                Roles = { new IdentityUserRole { RoleId = adminRole.Id, UserId = admin1Id } }
            };
            context.Roles.Add(adminRole);
            context.Roles.Add(memberRole);
            context.Users.Add(admin1);
            base.Seed(context);
        }
    }
}