/// Tim Tryzbiak, ootii, LLC
using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace com.ootii.Helpers
{
    /// <summary>
    /// Static functions to help us
    /// </summary>
    public class StringHelper
    {
        /// <summary>
        /// Converts the data to a string value
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="rInput">Value to convert</param>
        public static string ToString(Vector3 rInput)
        {
            return String.Format("[m:{0:f6} x:{1:f6} y:{2:f6} z:{3:f6}]", rInput.magnitude, rInput.x, rInput.y, rInput.z);
            //return String.Format("[{0:f4},{1:f4},{2:f4}]", rInput.x, rInput.y, rInput.z);
        }

        /// <summary>
        /// Converts the data to a string value
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="rInput">Value to convert</param>
        public static string ToString(Quaternion rInput)
        {
            Vector3 lEuler = rInput.eulerAngles;

            float lAngle = 0f;
            Vector3 lAxis = Vector3.zero;
            rInput.ToAngleAxis(out lAngle, out lAxis);

            return String.Format("[p:{0:f4} y:{1:f4} r:{2:f4} x:{3:f7} y:{4:f7} z:{5:f7} w:{6:f7} angle:{7:f7} axis:{8}]", lEuler.x, lEuler.y, lEuler.z, rInput.x, rInput.y, rInput.z, rInput.w, lAngle, ToString(lAxis));
        }

        /// <summary>
        /// Adds a space between camel cased text
        /// </summary>
        /// <param name="rInput"></param>
        /// <returns></returns>
        public static string FormatCamelCase(string rInput)
        {
            return Regex.Replace(Regex.Replace(rInput, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
        }

        /// <summary>
        /// Splits a string, but respecting qualifiers around the whole delimited segments so
        /// we can use the delimiter (say a comma) in the segment.
        /// </summary>
        /// <param name="rString"></param>
        /// <param name="rDelimiter"></param>
        /// <param name="rQualifier"></param>
        /// <param name="rIgnoreCase"></param>
        /// <returns></returns>
        public static string[] Split(string rString, string rDelimiter, string rQualifier, bool rIgnoreCase)
        {
            int lStartIndex = 0;
            bool lQualifierState = false;
            ArrayList lValues = new ArrayList();

            // Walk the original string one character at a time
            for (int lIndex = 0; lIndex < rString.Length - 1; lIndex++)
            {
                // Check if the qualifier exists for this character
                if (rQualifier != null && string.Compare(rString.Substring(lIndex, rQualifier.Length), rQualifier, rIgnoreCase) == 0)
                {
                    lQualifierState = !(lQualifierState);
                }
                // If we're not in a qualifier, check for a delimiter
                else if (!lQualifierState && (string.Compare(rString.Substring(lIndex, rDelimiter.Length), rDelimiter, rIgnoreCase) == 0))
                {
                    lValues.Add(rString.Substring(lStartIndex, lIndex - lStartIndex));
                    lStartIndex = lIndex + rDelimiter.Length;
                }
            }

            // Add the last part of the string
            if (lStartIndex < rString.Length)
            {
                lValues.Add(rString.Substring(lStartIndex, rString.Length - lStartIndex));
            }

            // Copy the results into an array
            string[] lReturnValues = new string[lValues.Count];
            lValues.CopyTo(lReturnValues);

            // Return the array
            return lReturnValues;
        }
    }

    /// <summary>
    /// String extensions
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Allows us to check if a subsctring exists with insensativity
        /// </summary>
        /// <param name="source"></param>
        /// <param name="toCheck"></param>
        /// <param name="comp"></param>
        /// <returns></returns>
        public static bool Contains(this string rSource, string rValue, StringComparison rComparison)
        {
            return rSource.IndexOf(rValue, rComparison) >= 0;
        }
    }
}

