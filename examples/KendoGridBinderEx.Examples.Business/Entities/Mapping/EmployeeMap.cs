using System.Data.Entity.ModelConfiguration;

namespace KendoGridBinderEx.Examples.Business.Entities.Mapping
{
    public class EmployeeMap : EntityTypeConfiguration<Employee>
    {
        public EmployeeMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            ToTable("KendoGrid_Employee");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.EmployeeNumber).HasColumnName("EmployeeNumber");
            Property(t => t.FirstName).HasColumnName("FirstName");
            Property(t => t.LastName).HasColumnName("LastName");
            Property(t => t.Email).HasColumnName("Email");
            Property(t => t.HireDate).HasColumnName("HireDate");
            Property(t => t.Assigned).HasColumnName("Assigned");
            Property(t => t.CompanyId).HasColumnName("Company_Id");
            Property(t => t.CountryId).HasColumnName("Country_Id");
            Property(t => t.FunctionId).HasColumnName("Function_Id");
            Property(t => t.SubFunctionId).HasColumnName("SubFunction_Id");

            // Relationships
            HasOptional(t => t.Company)
                .WithMany(t => t.Employees)
                .HasForeignKey(d => d.CompanyId);
            HasOptional(t => t.Country)
                .WithMany(t => t.Employees)
                .HasForeignKey(d => d.CountryId);
            HasOptional(t => t.SubFunction)
                .WithMany(t => t.Employees)
                .HasForeignKey(d => d.SubFunctionId);
            HasOptional(t => t.Function)
                .WithMany(t => t.Employees)
                .HasForeignKey(d => d.FunctionId);
        }
    }
}