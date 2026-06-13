using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKemit.Core.Services.Contract
{
    public interface IResponseCacheService
    {
        //Set CachedResponse
        Task CacheResponseAsync(string Key, object Response, TimeSpan timeToLive);

        //Get CachedResponse
        Task<string?> GetCachedResponseAsync(string Key);
    }
}
