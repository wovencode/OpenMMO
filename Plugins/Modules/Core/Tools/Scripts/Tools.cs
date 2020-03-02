//by Fhiz
using OpenMMO;
using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Security.Cryptography;
using System.IO;
using System.Text.RegularExpressions;
using OpenMMO.Chat;

namespace OpenMMO
{
	
	/// <summary>
	/// Partial class Tools contains a collection of utility methods that can be accessed via code anywhere.
	/// </summary>
	public partial class Tools
	{
	
		protected const char 	CONST_DELIMITER 	= ';';
		
		internal const int 	MIN_LENGTH_NAME		= 4;
		internal const int 	MAX_LENGTH_NAME 	= 16;
		
        #region TrimExcessWhitespace
        //157 ms - SOURCE: https://stackoverflow.com/questions/6442421/c-sharp-fastest-way-to-remove-extra-white-spaces
        /// <summary>Trims duplicate whitespace characters if they are next to each other...can also trim all whitespace.</summary>
        /// <param name="input">The string to trim</param> <param name="trimAllWhitespace">Trim out ALL whitespace?</param>
        /// <returns>A string without extra whitespaces.</returns>
        public static string TrimExcessWhitespace(string input, bool trimAllWhitespace = false)
        {
            int len = input.Length,
                index = 0,
                i = 0;
            var src = input.ToCharArray();
            bool skip = false;
            char ch;
            for (; i < len; i++)
            {
                ch = src[i];
                switch (ch)
                {
                    case '\u0020':
                    case '\u00A0':
                    case '\u1680':
                    case '\u2000':
                    case '\u2001':
                    case '\u2002':
                    case '\u2003':
                    case '\u2004':
                    case '\u2005':
                    case '\u2006':
                    case '\u2007':
                    case '\u2008':
                    case '\u2009':
                    case '\u200A':
                    case '\u202F':
                    case '\u205F':
                    case '\u3000':
                    case '\u2028':
                    case '\u2029':
                    case '\u0009':
                    case '\u000A':
                    case '\u000B':
                    case '\u000C':
                    case '\u000D':
                    case '\u0085':
                        if (skip || trimAllWhitespace) continue;
                        src[index++] = ch;
                        skip = true;
                        continue;
                    default:
                        skip = false;
                        src[index++] = ch;
                        continue;
                }
            }

            return new string(src, 0, index);
        }
        #endregion

       	/// <summary>
		/// Returns a path to the fileName, based on the build type (Editor, Android, iOS or other)
		/// </summary>
        public static string GetPath(string fileName) {
#if UNITY_EDITOR
        	return Path.Combine(Directory.GetParent(Application.dataPath).FullName, fileName);
#elif UNITY_ANDROID
        	return Path.Combine(Application.persistentDataPath, fileName);
#elif UNITY_IOS
        	return Path.Combine(Application.persistentDataPath, fileName);
#else
			return Path.Combine(Application.dataPath, fileName);
#endif
		}
		
		/// <summary>
		/// Hashes the provided text using the provided salt
		/// </summary>
		public static string GenerateHash(string encryptText, string saltText)
		{
			return Tools.PBKDF2Hash(encryptText, ProjectConfigTemplate.singleton.securitySalt + saltText);
		}
		
		/// <summary>
		/// Generates a hashcode from an array, not compatible with the GetDeterministicHashCode function of strings not suited for permanent storage.
		/// </summary>
		public static int GetArrayHashCode(object[] array)
		{
			if (array != null)
			{
				unchecked
				{
					int hash = 17;

					foreach (var item in array)
						hash = hash * 23 + ((item != null) ? item.GetHashCode() : 0);

					return hash;
				}
			}
			return 0;
		}
		
		/// <summary>
		/// Returns a simple, alphanumeric string of the provided length.
		/// </summary>
		public static string GetRandomAlphaString(int length=4)
		{
			string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    		return new string(Enumerable.Repeat(chars, length).Select(s => s[UnityEngine.Random.Range(0, s.Length)]).ToArray());
		}
		
		/// <summary>
		/// Returns the device Id of the current machine.
		/// </summary>
		public static string GetDeviceId
		{
			get
			{
				return SystemInfo.deviceUniqueIdentifier.ToString();
			}
		}
		
		/// <summary>
		/// Retrieves a integer value that is part of command line arguments this process was started with.
		/// </summary>
		/// <remarks>
		/// Note: Arguments are always null on android - their usage only makes sense on an OS capable of hosting a server
		/// </remarks>
		public static int GetArgumentInt(string name)
		{
			string[] args = System.Environment.GetCommandLineArgs();
			
			int idx = args.ToList().FindIndex(x => x == name);
			
			if (idx == -1 || idx == args.Length)
				return -1;
			
			return int.Parse(args[idx+1]);
			
		}
		
		/// <summary>
		/// Retrieves a string value that is part of command line arguments this process was started with.
		/// </summary>
		/// <remarks>
		/// Note: Arguments are always null on android - their usage only makes sense on an OS capable of hosting a server
		/// </remarks>
		public static string GetArgumentsString
		{
			get {
				string[] args = System.Environment.GetCommandLineArgs();
				return args != null ? String.Join(" ", args.Skip(1).ToArray()) : "";
			}
		}
		
		/// <summary>
		/// Returns the path this process has been started with. The first argument is the fileName itself.
		/// </summary>
		/// <remarks>
		/// Note: Arguments are always null on android - their usage only makes sense on an OS capable of hosting a server.
		/// </remarks>
		public static string GetProcessPath
		{
			get
			{
				string[] args = System.Environment.GetCommandLineArgs();
				
				if (args != null)
					if (!String.IsNullOrWhiteSpace(args[0]))
						return args[0];

				return String.Empty;
			}
		}

        /// <summary>
        /// Validates a name by simply checking length and allowed characters
		/// Could be expanded here if required
        /// </summary>
        /// <param name="textToCheck"></param>
        /// <returns></returns>
		public static bool IsAllowedName(string textToCheck)
		{
            return textToCheck.Length >= MIN_LENGTH_NAME &&
                    textToCheck.Length <= MAX_LENGTH_NAME &&
                    Regex.IsMatch(textToCheck, @"^[a-zA-Z0-9_" + " " + "]+$") && //ALLOWED LETTERS & SYMBOLS & BLANK SPACES
                    (textToCheck[textToCheck.Length - 1] != ' ') && //LAST CHARACTER NOT WHITESPACE
                    (textToCheck[0] != ' ') && //FIRST CHARACTER NOT WHITESPACE
                    (!ChatManager.singleton.profanityFilter.FilterText(textToCheck).Contains(ChatManager.ProfanityMask)); //DOES NOT CONTAIN PROFANITY
                    
					//Regex.IsMatch(_text, @"^[a-zA-Z0-9_]+$"); // && //OLD WAY //DEPRECIATED
					//!ArrayContains(BadwordsTemplate.singleton.badwords, _text); //DEPRECIATED? NOTE: This was commented out already?
		}
		
		/// <summary>
		/// Very simple password validation (must not be empty) so it works with hashed passwords as well. Could be expanded here if required.
		/// </summary>
		public static bool IsAllowedPassword(string _text)
		{
			return !String.IsNullOrWhiteSpace(_text);
		}

		/// <summary>
		/// Hashes the provided string using the provided salt.
		/// </summary>
		public static string PBKDF2Hash(string text, string salt)
		{
			byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
			Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(text, saltBytes, 10000);
			byte[] hash = pbkdf2.GetBytes(20);
			return BitConverter.ToString(hash).Replace("-", string.Empty);
		}
		
		/// <summary>
		/// Converts a integer array into a CONST_DELIMITER separated string. Used to store array values in the database.
		/// </summary>
		public static string IntArrayToString(int[] array)
		{
			if (array == null || array.Length == 0) return null;
			string arrayString = "";
			for (int i = 0; i < array.Length; i++)
			{
				arrayString += array[i].ToString();
				if (i < array.Length - 1)
					arrayString += CONST_DELIMITER;
			}
			return arrayString;
		}

		/// <summary>
		/// Converts a string into an array of integers. Requires the string to be a list of integers separated by the global CONST_DELIMITER.
		/// </summary>
		public static int[] IntStringToArray(string array)
		{
			if (string.IsNullOrWhiteSpace(array)) return null;
			string[] tokens = array.Split(CONST_DELIMITER);
			int[] arrayInt = Array.ConvertAll<string, int>(tokens, int.Parse);
			return arrayInt;
		}

		/// <summary>
		/// Checks if a integer array contains a integer.
		/// </summary>
		public static bool ArrayContains(int[] array, int number)
		{
			foreach (int element in array)
			{
				if (element == number)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Checks if a string array contains a string.
		/// </summary>
		public static bool ArrayContains(string[] array, string text, bool toLower=true)
		{
			foreach (string element in array)
			{
				if (toLower && text.ToLower().IndexOf(element.ToLower()) != -1)
					return true;
				else if (text.IndexOf(element) != -1)
						return true;
			}
			return false;
		}

		/// <summary>
		/// Removes a string from a string array and returns the modified array again.
		/// </summary>
		public static string[] RemoveFromArray(string[] array, string text)
		{
			return array.Where(x => x != text).ToArray();
		}
	
		/// <summary>
		/// Removes an integer from an integer array and returns the modified array again.
		/// </summary>
		public static int[] RemoveFromArray(int[] array, int number)
		{
			return array.Where(x => x != number).ToArray();
		}
		
		/// <summary>
		/// Sets a string on player preferences. Only set if exists (and is equal to "oldValue"") or "set" is true.
		/// </summary>
		public static void PlayerPrefsSetString(string keyName, string newValue, string oldValue="", bool set=false)
		{
			if (PlayerPrefs.HasKey(keyName) || set)
				if ((!set && PlayerPrefs.GetString(keyName) == oldValue) || (!set && oldValue == "") || set)
					PlayerPrefs.SetString(keyName, newValue);
		}
		
		/// <summary>
		/// Sets a integer on player preferences. Only set if exists (and is equal to "oldValue"") or "set" is true.
		/// </summary>
		public static void PlayerPrefsSetInt(string keyName, int newValue, int oldValue=-1, bool set=false)
		{
			if (PlayerPrefs.HasKey(keyName) || set)
				if ((!set && PlayerPrefs.GetInt(keyName) == oldValue) || (!set && oldValue == -1) || set)
					PlayerPrefs.SetInt(keyName, newValue);
		}
		
		/// <summary>
		/// Checks if any input is focused (using the new allSelectablesArray from Unity 2019)
		/// </summary>
		public static bool AnyInputFocused
    	{
    		get {
        		foreach (Selectable selectable in Selectable.allSelectablesArray)
            		if (selectable is InputField && ((InputField)selectable).isFocused)
           		     	return true;
        		return false;
        	}
    	}
    	
		/// <summary>
		/// Clamps an camera angle between min and max.
		/// </summary>
		public static float ClampAngleBetweenMinAndMax(float angle, float min, float max)
		{
			if (angle < -360)
				angle += 360;
			if (angle > 360)
				angle -= 360;
			return Mathf.Clamp(angle, min, max);
		}
		
	}

}