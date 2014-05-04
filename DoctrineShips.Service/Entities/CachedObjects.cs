namespace DoctrineShips.Service.Entities
{
    using System;
    using System.Collections.Generic;

    public class CachedObjects<TKey, TValue> where TValue : class
    {
        private TimeSpan defaultTtl;
        private Dictionary<TKey, CachedObject<TValue>> cachedObjects = new Dictionary<TKey, CachedObject<TValue>>();

        public CachedObjects()
            : this(TimeSpan.FromMinutes(10))
        {
        }

        public CachedObjects(TimeSpan defaultTtl)
        {
            this.defaultTtl = defaultTtl;
        }

        public void AddCachedObject(TKey key, TValue value)
        {
            this.AddCachedObject(key, value, this.defaultTtl);
        }

        public void AddCachedObject(TKey key, TValue value, TimeSpan ttl)
        {
            this.cachedObjects.Add(key, new CachedObject<TValue>(value, DateTime.UtcNow + ttl));
        }

        public TValue GetCachedObject(TKey key)
        {
            CachedObject<TValue> cachedObject;

            this.cachedObjects.TryGetValue(key, out cachedObject);

            if (cachedObject != null)
            {
                if (cachedObject.HasExpired == false)
                {
                    return cachedObject.Value;
                }
                else
                {
                    this.cachedObjects.Remove(key);
                }
            }
                
            return null;
        }

        public void Flush()
        {
            this.cachedObjects.Clear();
        }
    }

}
