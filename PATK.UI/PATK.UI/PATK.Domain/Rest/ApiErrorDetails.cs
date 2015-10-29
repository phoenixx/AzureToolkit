using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PATK.Domain.Rest
{
    [DataContract]
    [Serializable]
    public class ApiErrorDetails
    {
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public IList<ApiErrorMessage> Details { get; set; }
    }
}