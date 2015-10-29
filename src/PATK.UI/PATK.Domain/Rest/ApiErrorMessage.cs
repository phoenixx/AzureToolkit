using System;
using System.Runtime.Serialization;

namespace PATK.Domain.Rest
{
    [DataContract]
    [Serializable]
    public class ApiErrorMessage
    {
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Target { get; set; }
        [DataMember]
        public string Message { get; set; }
    }
}