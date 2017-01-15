using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using MySql.Data.Entity;
using ClassLibrary.Models;
using DBAPI.DAL;

namespace DBAPI.Models
{

    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        static ApplicationDbContext()
        {
            Database.SetInitializer(new MySqlInitializer());
            DbConfiguration.SetConfiguration(new MySql.Data.Entity.MySqlEFConfiguration());
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           modelBuilder.Entity<POI>().HasMany(p => p.ConnectedPOIs).WithMany().Map(m =>
           {
               m.MapLeftKey("POIID");
               m.MapRightKey("ConnectedPOIID");
               m.ToTable("Caminho");
           });

            modelBuilder.Entity<Percurso>().HasMany(p => p.PercursoPOIs).WithMany().Map(m =>
            {
                m.MapLeftKey("PercursoID");
                m.MapRightKey("POIID");
                m.ToTable("Percurso_POI");
            });

            //modelBuilder.Entity<POI>().HasMany(p => p.Hashtags).WithMany().Map(m =>
            //{
            //    m.MapLeftKey("Hashtag_HashtagID");
            //    m.MapRightKey("POI_POIID");
            //    m.ToTable("HashtagPOI");
            //});

            modelBuilder.Entity<ApplicationUser>().Property(u => u.UserName).IsUnicode(false);
            modelBuilder.Entity<ApplicationUser>().Property(u => u.Email).IsUnicode(false);
            modelBuilder.Entity<IdentityRole>().Property(r => r.Name).HasMaxLength(255);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<POI> POIs { get; set; }
        public DbSet<Percurso> Percursos { get; set; }
        public DbSet<Hashtag> Hashtags { get; set; }
        public DbSet<Visita> Visitas { get; set; }

    }
}