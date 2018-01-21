using System.Data.Entity.ModelConfiguration;

namespace KendoGridBinderEx.Examples.Business.Entities.Mapping
{
    public class RoleMap : EntityTypeConfiguration<Role>
    {
        public RoleMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            ToTable("KendoGrid_Role");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.Description).HasColumnName("Description");

            // Relationships
            HasMany(t => t.Users)
                .WithMany(t => t.Roles)
                .Map(m =>
                    {
                        m.ToTable("KendoGrid_UserRoles");
                        m.MapLeftKey("RoleId");
                        m.MapRightKey("UserId");
                    });
        }
    }
}