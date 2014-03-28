using System;

namespace CoreServices
{
    public class CoreRequest : ICoreRequest
    {
        public string Uri { get; set; }
        public string Body { get; set; }
        public string ServiceId { get; set; }
        public ICoreKey CoreKey { get; set; }
        
        public DateTime Timestamp 
        { 
            get 
            { 
                return DateTime.UtcNow; 
            } 
        }

        /// <summary>
        /// This method canonicalises and signs a request, sending and then validating the response.
        /// </summary>
        /// <returns>Returns a string containing the response or an empty string on failure.</returns>
        public string Send()
        {
            throw new NotImplementedException();
        }
    }
}
