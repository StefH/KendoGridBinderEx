using System.ComponentModel.DataAnnotations;

namespace KendoGridBinderEx.Examples.Business.Entities
{
    public abstract class Entity : IEntity
    {
        [Key]
        public virtual long Id { get; set; }
    }
}