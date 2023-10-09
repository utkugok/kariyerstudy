using study.Services.Interfaces;
using System.Runtime.Caching;

namespace study.Services
{
    public class CacheService : ICacheService
    {
        private ObjectCache _memoryCache = MemoryCache.Default;

        public T GetData<T>(string key)
        {

            try
            {
                T item = (T)_memoryCache.Get(key);
                return item;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public object RemoveData(string key)
        {
            try
            {
                var response = false;

                if (!string.IsNullOrEmpty(key))
                {
                    _memoryCache.Remove(key);
                    response = true;
                }

                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            try
            {
                var response = false;

                if(!string.IsNullOrEmpty(key))
                {
                    _memoryCache.Set(key, value, expirationTime);
                    response = true;
                }

                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
