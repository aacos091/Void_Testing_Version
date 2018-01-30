using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

namespace TeaspoonTools.Utils 
{
    public static class StringExtensions {

        public static bool hasLettersOrDigits(this string str)
        {
            for (int i = 0; i < str.Length; i++)
                if (Char.IsLetterOrDigit(str[i]))
                    return true;

            return false;
        }

        ///
        /// <summary>
        /// Returns a copy of the string with a number of the passed char S removed. If 
        /// howMany is -1, it removes all Ss of the passed char.
        /// </summary>
        public static string RemoveChar(this string str, char charToRemove, int howMany = -1)
        {
        
            int amountRemoved = 0;
            StringBuilder result = new StringBuilder();
            char currentChar;
            bool removeThisChar;

            for (int i = 0; i < str.Length; i++)
            {
                if (amountRemoved == howMany)
                {
                    // just add the rest of the string to the result and return it
                    result.Append(str.Substring(i));
                    return result.ToString();
                }
                else
                {
                    currentChar = str[i];
                    removeThisChar = currentChar == charToRemove;

                    if (removeThisChar)
                    {
                        amountRemoved++;
                        continue;
                    }
                    else
                        result.Append(currentChar);
                }
            }

            return result.ToString();
        }
        
    }
}