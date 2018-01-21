using System.Data.Entity.ModelConfiguration;

namespace KendoGridBinderEx.Examples.Business.Entities.Mapping
{
    public class SubFunctionMap : EntityTypeConfiguration<SubFunction>
    {
        public SubFunctionMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            ToTable("KendoGrid_SubFunction");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.Code).HasColumnName("Code");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.FunctionId).HasColumnName("Function_Id");

            // Relationships
            HasOptional(t => t.Function)
                .WithMany(t => t.SubFunctions)
                .HasForeignKey(d => d.FunctionId);

        }
    }
}