using System.Data.Entity.ModelConfiguration;

namespace KendoGridBinderEx.Examples.Business.Entities.Mapping
{
    public class CompanyMap : EntityTypeConfiguration<Company>
    {
        public CompanyMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            ToTable("KendoGrid_Company");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.MainCompanyId).HasColumnName("MainCompany_Id");

            // Relationships
            HasOptional(t => t.MainCompany)
                .WithMany(t => t.Companies)
                .HasForeignKey(d => d.MainCompanyId);

        }
    }
}