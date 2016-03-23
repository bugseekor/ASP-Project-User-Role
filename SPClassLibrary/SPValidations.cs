// File Name : SPValidations.cs
// Validation class that edits user input
//
// Author : Sam Sangkyun Park
// Date Created : Nov. 17, 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPClassLibrary
{
    public class SPValidations
    {
        /// <summary>
        /// Shifts all letters to lower case
        /// Removes all spaces before and after string
        /// Capitalises only first letter
        /// </summary>
        /// <param name="text">any string field</param>
        /// <returns>edited string</returns>
        public static string Capitalise(string text)
        {
            if (text == null)
            {
                return text;
            }
            else
            {
                text = text.ToLower();
                text = text.Trim();
                text = char.ToUpper(text[0]) + text.Substring(1);
            }
            return text;
        }
    }
}
