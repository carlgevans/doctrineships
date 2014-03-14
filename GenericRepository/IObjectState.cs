
using System.ComponentModel.DataAnnotations.Schema;

namespace GenericRepository
{
    public interface IObjectState
    {
        [NotMapped]
        ObjectState ObjectState { get; set; }
    }
}