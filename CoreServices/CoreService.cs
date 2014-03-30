namespace CoreServices
{
    using System;
    using System.Net;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// Provides methods for interacting with core services.
    /// </summary>
    public class CoreService : ICoreService
    {
        private readonly ICoreApplication app;
        private readonly string baseUrl;

        /// <summary>
        /// Initialises a new instance of a core service.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="baseUrl">The main base url of the core service, in the format 'https://core.bravecollective.net/'.</param>
        public CoreService(ICoreApplication app, string baseUrl)
        {
            this.app = app;
            this.baseUrl = baseUrl;
        }

        /// <summary>
        /// This method canonicalises and signs a request, sending and then validating the response.
        /// </summary>
        /// <param name="api">The core service api endpoint to use.</param>
        /// <param name="args">An array of arguments to be appended to the url.</param>
        /// <param name="data">An array of named keywords and their values separated by an equals sign. Example: success=http://successurl.com.</param>
        /// <returns>Returns a string containing the response or an empty string on failure.</returns>
        public string Request(string api, string[] args, string[] data)
        {
            WebClient request = new WebClient();
            Uri uri = new Uri(this.baseUrl + "/" + api + args.ToString());
            string response = string.Empty;
            
            try
            {
                System.Diagnostics.Debug.WriteLine(uri);
                System.Diagnostics.Debug.WriteLine(data.ToString());
                //response = request.UploadString(uri, "POST", data.ToString());
            }
            catch (System.Net.WebException e)
            {
                System.Diagnostics.Debug.WriteLine("An error occured while issuing a request to the core services uri: " + uri);
                System.Diagnostics.Debug.WriteLine(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (Exception)
            {
                throw;
            }
            
            // Validate the response.
            if (this.IsValid(response) == true)
            {
                return response;
            } 
            else
            {
                return string.Empty;
            }
        }

        internal bool IsValid(string response)
        {
            // Todo: Validation.
            return true;
        }
    }
}
