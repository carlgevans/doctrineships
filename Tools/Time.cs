namespace Tools
{
    using System;

    /// <summary>
    /// Assorted time tools.
    /// </summary>
    public static class Time
    {
        /// <summary>
        /// Check to see if a length of time has elapsed since a reference time.
        /// </summary>
        /// <param name="referenceTime">The reference time to compare against.</param>
        /// <param name="lengthOfTime">The length of time to be checked against the reference time.</param>
        /// <returns>Returns a boolean value.</returns>
        public static bool HasElapsed(DateTime referenceTime, TimeSpan lengthOfTime)
        {
            bool elapsedStatus;

            if ((DateTime.UtcNow - referenceTime) <= lengthOfTime)
            {
                elapsedStatus = false;
            }
            else
            {
                elapsedStatus = true;
            }
            
            return elapsedStatus;
        }
    }
}
