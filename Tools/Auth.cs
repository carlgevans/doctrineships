namespace Tools
{
    using System.Security.Principal;

    /// <summary>
    /// Assorted authentication tools.
    /// </summary>
    public static class Auth
    {
        /// <summary>
        /// Check if a user is in a list of roles.
        /// </summary>
        /// <param name="roles">A string array of roles.</param>
        /// <returns>Returns a true if the user is in one of the groups.</returns>
        public static bool IsInAnyRole(this IPrincipal principal, params string[] roles)
        {
            foreach (var role in roles)
            {
                if (principal.IsInRole(role))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
