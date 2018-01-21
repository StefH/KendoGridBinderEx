namespace KendoGridBinder.UnitTests.Entities
{
    public class Company : Entity
    {
        public string Name { get; set; }

        public MainCompany MainCompany { get; set; }
    }
}