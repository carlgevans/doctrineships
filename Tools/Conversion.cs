namespace Tools
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Assorted string tools.
    /// </summary>
    public static class Conversion
    {
        /// <summary>
        /// Check each character of a passed string, returning false if a non-digit character is found.
        /// </summary>
        /// <param name="toBeChecked">The string to be checked.</param>
        /// <returns>Returns a boolean value.</returns>
        public static bool IsDigitsOnly(string toBeChecked)
        {
            foreach (char character in toBeChecked)
            {
                if (character < '0' || character > '9')
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Remove all non-alphanumeric characters from a string.
        /// </summary>
        /// <param name="toBeConverted">The string to be converted into an alphanumeric string.</param>
        /// <returns>Returns a string with all non-alphanumeric characters removed.</returns>
        public static string StringToSafeString(string toBeConverted)
        {
            string safeString = string.Empty;

            if (toBeConverted != null && toBeConverted != string.Empty)
            {
                safeString = Regex.Replace(toBeConverted, @"[^A-Za-z0-9_\s-.]", string.Empty);
            }

            return safeString;
        }

        /// <summary>
        /// Remove all non-numeric characters from a string and convert it to an int. If the conversion fails then an integer containing 0 (or the optional passed default value) is returned.
        /// </summary>
        /// <param name="toBeConverted">The string to be converted into an integer.</param>
        /// <param name="defaultValue">An optional default value should the conversion fail.</param>
        /// <returns>Returns the number within the passed string as an integer. If the conversion fails then an integer containing 0 (or the optional default value) is returned.</returns>
        public static int StringToInt32(string toBeConverted, int defaultValue = 0)
        {
            string charactersRemoved = string.Empty;
            int nowConverted = defaultValue;

            if (toBeConverted != null && toBeConverted != string.Empty)
            {
                charactersRemoved = Regex.Replace(toBeConverted, "[^.0-9]", string.Empty);
                int.TryParse(charactersRemoved, out nowConverted);
            }

            return nowConverted;
        }

        /// <summary>
        /// Convert a string to a datetime. If the conversion fails then a default value of '00/00/0001 00:00' (or the optional passed default value) is returned.
        /// </summary>
        /// <param name="toBeConverted">The string to be converted into a datetime.</param>
        /// <param name="defaultValue">An optional default value should the conversion fail.</param>
        /// <returns>Returns the date & time contained within the passed string as a datetime. If the conversion fails then a datetime containing '00/00/0001 00:00' (or the optional default value) is returned.</returns>
        public static DateTime StringToDateTime(string toBeConverted, DateTime defaultValue = default(DateTime))
        {
            DateTime nowConverted = defaultValue;

            if (toBeConverted != null && toBeConverted != string.Empty)
            {
                DateTime.TryParse(toBeConverted, out nowConverted);
            }

            return nowConverted;
        }

        /// <summary>
        /// Convert a string to a long. If the conversion fails then a default value of '0' (or the optional passed default value) is returned.
        /// </summary>
        /// <param name="toBeConverted">The string to be converted into a long.</param>
        /// <param name="defaultValue">An optional default value should the conversion fail.</param>
        /// <returns>Returns the value contained within the passed string as a long. If the conversion fails then a long containing '0' (or the optional default value) is returned.</returns>
        public static long StringToLong(string toBeConverted, long defaultValue = 0)
        {
            string charactersRemoved = string.Empty;
            long nowConverted = defaultValue;

            if (toBeConverted != null && toBeConverted != string.Empty)
            {
                charactersRemoved = Regex.Replace(toBeConverted, "[^.0-9]", string.Empty);
                long.TryParse(charactersRemoved, out nowConverted);
            }

            return nowConverted;
        }

        /// <summary>
        /// Convert a string to a double. If the conversion fails then a default value of '0' (or the optional passed default value) is returned.
        /// </summary>
        /// <param name="toBeConverted">The string to be converted into a double.</param>
        /// <param name="defaultValue">An optional default value should the conversion fail.</param>
        /// <returns>Returns the value contained within the passed string as a double. If the conversion fails then a double containing '0' (or the optional default value) is returned.</returns>
        public static double StringToDouble(string toBeConverted, double defaultValue = 0)
        {
            string charactersRemoved = string.Empty;
            double nowConverted = defaultValue;

            if (toBeConverted != null && toBeConverted != string.Empty)
            {
                charactersRemoved = Regex.Replace(toBeConverted, "[^.0-9]", string.Empty);
                double.TryParse(charactersRemoved, out nowConverted);
            }

            return nowConverted;
        }

        /// <summary>
        /// Convert a number (0 or 1) within a string to a boolean. If the conversion fails then a default value of 'false' (or the optional passed default value) is returned.
        /// </summary>
        /// <param name="toBeConverted">The string to be converted into a boolean.</param>
        /// <param name="defaultValue">An optional default value should the conversion fail.</param>
        /// <returns>Returns the value contained within the passed string as a boolean. If the conversion fails then a boolean containing 'false' (or the optional default value) is returned.</returns>
        public static bool StringToBool(string toBeConverted, bool defaultValue = false)
        {
            string charactersRemoved = string.Empty;
            bool nowConverted = defaultValue;

            if (toBeConverted != null && toBeConverted != string.Empty)
            {
                charactersRemoved = Regex.Replace(toBeConverted, "[^.0-1]", string.Empty);

                if (charactersRemoved == "0")
                {
                    nowConverted = false;
                }
                else if (charactersRemoved == "1")
                {
                    nowConverted = true;
                }
            }

            return nowConverted;
        }

        /// <summary>
        /// Convert a string to an enumerated type. If the conversion fails then the default passed value is returned.
        /// </summary>
        /// <param name="toBeConverted">The string to be converted into an enumerated type.</param>
        /// <param name="defaultValue">An optional default value should the conversion fail.</param>
        /// <returns>Returns the value contained within the passed string as an enumerated type. If the conversion fails then the default passed value is returned.</returns>
        public static T StringToEnum<T>(string toBeConverted, T defaultValue) where T : struct, IConvertible
        {
            T nowConverted = defaultValue;

            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            if (!string.IsNullOrEmpty(toBeConverted))
            {
                foreach (T item in Enum.GetValues(typeof(T)))
                {
                    if (item.ToString().ToLower().Equals(toBeConverted.Trim().ToLower()))
                    {
                        nowConverted = item;
                    }
                }
            }

            return nowConverted;
        }
    }
}