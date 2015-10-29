using System.Runtime.Serialization;

namespace PATK.Domain.Rest
{
    [DataContract]
    public class ApiError
    {
        [DataMember]
        public ApiErrorDetails Error { get; set; }
    }
}