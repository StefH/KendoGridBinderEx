using System.Data.Entity.ModelConfiguration;

namespace KendoGridBinderEx.Examples.Business.Entities.Mapping
{
    public class EmployeeViewMap : EntityTypeConfiguration<EmployeeView>
    {
        public EmployeeViewMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            // View & Column Mappings
            ToTable("VW_EmployeeDetails");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.EmployeeNumber).HasColumnName("EmployeeNumber");
            Property(t => t.FirstName).HasColumnName("FirstName");
            Property(t => t.LastName).HasColumnName("LastName");
            Property(t => t.Email).HasColumnName("Email");
            Property(t => t.HireDate).HasColumnName("HireDate");
            Property(t => t.Assigned).HasColumnName("Assigned");

            Property(t => t.IsAssigned).HasColumnName("IsAssigned");
            Property(t => t.IsManager).HasColumnName("IsManager");
            Property(t => t.FullName).HasColumnName("FullName");

            Property(t => t.CountryCode).HasColumnName("CountryCode");
            Property(t => t.CountryName).HasColumnName("CountryName");
        }
    }
}