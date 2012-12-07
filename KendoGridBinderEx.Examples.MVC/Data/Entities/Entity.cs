using System.ComponentModel.DataAnnotations;

namespace KendoGridBinder.Examples.MVC.Data.Entities
{
    public abstract class Entity : IEntity
    {
        [Key]
        public virtual long Id { get; set; }
    }
}