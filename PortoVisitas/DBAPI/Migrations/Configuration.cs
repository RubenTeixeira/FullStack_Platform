namespace DBAPI.Migrations
{
    using ClassLibrary.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DBAPI.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DBAPI.Models.ApplicationDbContext context)
        {

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            IdentityResult roleResult;

            string userRoleName = "User";
            // Create User Role
            // Check to see if Role Exists, if not create it
            if (!roleManager.RoleExists(userRoleName))
            {
                roleResult = roleManager.Create(new IdentityRole(userRoleName));
            }

            string editorRoleName = "Gestor";
            // Create User Role
            // Check to see if Role Exists, if not create it
            if (!roleManager.RoleExists(editorRoleName))
            {
                roleResult = roleManager.Create(new IdentityRole(editorRoleName));
            }
        }
    }
}
