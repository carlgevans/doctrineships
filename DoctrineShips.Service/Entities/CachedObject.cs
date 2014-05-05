namespace DoctrineShips.Service.Entities
{
    using System;

    public class CachedObject<T> where T : class
    {
        private DateTime expires;
        private T value;

        public DateTime Expires
        {
            get
            {
                return this.expires;
            }
        }

        public bool HasExpired
        {
            get
            {
                return DateTime.UtcNow >= this.expires;
            }
        }

        public T Value
        {
            get
            {
                return this.value;
            }
        }

        public CachedObject(T value, DateTime expires)
        {
            this.value = value;
            this.expires = expires;
        }
    }
}
