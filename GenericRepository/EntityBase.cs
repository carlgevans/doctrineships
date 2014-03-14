using System.ComponentModel.DataAnnotations.Schema;

namespace GenericRepository
{
    public abstract class EntityBase : IObjectState
    {
        [NotMapped]
        public ObjectState ObjectState { get; set; }
    }
}