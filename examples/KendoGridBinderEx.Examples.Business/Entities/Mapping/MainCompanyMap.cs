using System.Data.Entity.ModelConfiguration;

namespace KendoGridBinderEx.Examples.Business.Entities.Mapping
{
    public class MainCompanyMap : EntityTypeConfiguration<MainCompany>
    {
        public MainCompanyMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            ToTable("KendoGrid_MainCompany");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.Name).HasColumnName("Name");
        }
    }
}