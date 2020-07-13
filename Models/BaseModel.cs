using System.Runtime.Serialization;

namespace Models
{
    [DataContract]
    public abstract class BaseModel
    {
        public long? Id { get; set; }
    }
}
