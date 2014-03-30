namespace CoreServices
{
    /// <summary>
    /// Provides methods for interacting with core services.
    /// </summary>
    public interface ICoreService
    {
        /// <summary>
        /// This method canonicalises and signs a request, sending and then validating the response.
        /// </summary>
        /// <param name="api">The core service api endpoint to use.</param>
        /// <param name="args">An array of arguments to be appended to the url.</param>
        /// <param name="data">An array of named keywords and their values separated by an equals sign. Example: success=http://successurl.com.</param>
        /// <returns>Returns a string containing the response or an empty string on failure.</returns>
        string Request(string api, string[] args, string[] data);
    }
}
