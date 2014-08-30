using System.Data.Entity.ModelConfiguration;

namespace KendoGridBinderEx.Examples.Business.Entities.Mapping
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            ToTable("KendoGrid_User");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.IdentityName).HasColumnName("IdentityName");
            Property(t => t.DisplayName).HasColumnName("DisplayName");
            Property(t => t.EmailAddress).HasColumnName("EmailAddress");
        }
    }
}