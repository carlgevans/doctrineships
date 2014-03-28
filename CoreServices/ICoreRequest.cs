using System;

namespace CoreServices
{
    public interface ICoreRequest
    {
        DateTime Timestamp { get; set; }
        string Uri { get; set; }
        string Body { get; set; }
        string ServiceId { get; set; }
        ICoreKey CoreKey { get; set; }

        /// <summary>
        /// This method canonicalises and signs a request, sending and then validating the response.
        /// </summary>
        /// <returns>Returns a string containing the response or an empty string on failure.</returns>
        string Send();
    }
}
