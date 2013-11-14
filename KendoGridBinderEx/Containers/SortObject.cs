namespace KendoGridBinderEx.Containers
{
    public class SortObject
    {
        public SortObject(string field, string direction)
        {
            Field = field;
            Direction = direction;
        }

        public string Field { get; set; }
        public string Direction { get; set; }
    }
}
