using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class CityFamilyDbContext : DbContext
    {
        public DbSet<Building> Building { get; set; }
        public DbSet<Layout> Layout { get; set; }
        public DbSet<Decorate> Decorate { get; set; }
        public DbSet<Admins> Admins { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<FuncResult> FuncResult { get; set; }
        public DbSet<Styles> Styles { get; set; }
        public DbSet<StyleDetails> StyleDetails { get; set; }
        public DbSet<StyleThird> StyleThird { get; set; }
        public DbSet<DIYResult> DIYResult { get; set; }
        public DbSet<SpotPics> SpotPics { get; set; }
        public DbSet<SpaceCate> SpaceCate { get; set; }

        public DbSet<FurnitureStyle> FurnitureStyle { get; set; }

        public DbSet<T_AuthInfo> T_AuthInfo { get; set; }

        public DbSet<T_DepartmentInfo> T_DepartmentInfo { get; set; }

        public DbSet<T_MenuInfo> T_MenuInfo { get; set; }

        public DbSet<T_RoleInfo> T_RoleInfo { get; set; }

        public DbSet<T_UserDepartment> T_UserDepartment { get; set; }

        public DbSet<T_UserRole> T_UserRole { get; set; }

        public DbSet<T_CompanyInfo> T_CompanyInfo { get; set; }

        public DbSet<T_StyleFurniturePics> T_StyleFurniturePics { get; set; }

        public DbSet<T_FurnitureCover> T_FurnitureCover { get; set; }

        public DbSet<FStyleID> FStyleID { get; set; }

        public DbSet<FCoverID> FCoverID { get; set; }

        public DbSet<StylesID> StylesID { get; set; }
        public DbSet<UpdateRecord> UpdateRecord { get; set; }

        public CityFamilyDbContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer<CityFamilyDbContext>(null);
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
        }
    }
}