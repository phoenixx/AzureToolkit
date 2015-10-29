using System.Collections.Generic;
using System.Threading.Tasks;
using PATK.Domain.Rest;

namespace PATK.Rest.RestConsumer
{
    public interface IRestConsumer
    {
        Task<ApiResponse<T>> Get<T>(string destination);
        Task<ApiResponse<TOut>> Put<T, TOut>(T model, string destination);
        Task<ApiResponse<TOut>> Patch<T, TOut>(T model, string destination, Dictionary<string, string> optionalHeaders = null);
        Task<ApiResponse<bool>> Delete(string destination, Dictionary<string, string> optionalHeaders = null);
    }
}