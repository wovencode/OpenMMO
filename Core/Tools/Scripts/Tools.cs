
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
	
	// ===================================================================================
	// Tools
	// ===================================================================================
	public partial class Tools
	{
	
		protected const char 	CONST_DELIMITER 	= ';';
		
		internal const int 	MIN_LENGTH_NAME		= 4;
		internal const int 	MAX_LENGTH_NAME 	= 16;
		
		protected static string sOldChecksum, sNewChecksum	= "";

        // ============================ STRING MODIFICATIONS ===============================

        // -------------------------------------------------------------------------------
        // TrimExcessWhitespace
        // -------------------------------------------------------------------------------
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

        // ============================ PATH & DIRECTORIES ===============================

        // -------------------------------------------------------------------------------
        // GetPath
        // -------------------------------------------------------------------------------
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
		
		// =========================== (WEAK) LOCAL SECURITY =============================
		
		// -------------------------------------------------------------------------------
		// GenerateHash
		// -------------------------------------------------------------------------------
		public static string GenerateHash(string encryptText, string saltText)
		{
			return Tools.PBKDF2Hash(encryptText, ProjectConfigTemplate.singleton.securitySalt + saltText);
		}
		
		// -------------------------------------------------------------------------------
		// GetChecksum
		// -------------------------------------------------------------------------------
		public static bool GetChecksum(string filepath)
		{
			
			sNewChecksum = CalculateMD5(filepath);
		
			sOldChecksum = PlayerPrefs.GetString(Constants.PlayerPrefsChecksum, "");
			
			if (string.IsNullOrWhiteSpace(sOldChecksum))
				SetChecksum(filepath);
			
			return (sOldChecksum == sNewChecksum);
			
		}
		
		// -------------------------------------------------------------------------------
		// SetChecksum
		// -------------------------------------------------------------------------------
		public static void SetChecksum(string filepath)
		{
			sNewChecksum = CalculateMD5(filepath);
			PlayerPrefs.SetString(Constants.PlayerPrefsChecksum, sNewChecksum);
		}
		
		// -------------------------------------------------------------------------------
		// CalculateMD5
		// not recommended on very large files
		// -------------------------------------------------------------------------------
		public static string CalculateMD5(string filepath)
		{
			using (var md5 = MD5.Create())
			{
				using (var stream = File.OpenRead(filepath))
				{
					var hash = md5.ComputeHash(stream);
					return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
				}
			}
		}
		
		// -------------------------------------------------------------------------------
		// GetArrayHashCode
		// for arrays, not compatible with the GetDeterministicHashCode function of strings
		// not suited for permanent storage !
		// -------------------------------------------------------------------------------
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
		
		// -------------------------------------------------------------------------------
		// GetRandomAlphaString
		// -------------------------------------------------------------------------------
		public static string GetRandomAlphaString(int length=4)
		{
			string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    		return new string(Enumerable.Repeat(chars, length).Select(s => s[UnityEngine.Random.Range(0, s.Length)]).ToArray());
		}
		
		// ================================= OTHER =======================================
		
		// -------------------------------------------------------------------------------
		// GetDeviceId
		// -------------------------------------------------------------------------------
		public static string GetDeviceId
		{
			get
			{
				return SystemInfo.deviceUniqueIdentifier.ToString();
			}
		}
		
		// -------------------------------------------------------------------------------
		// GetArgumentInt
		// retrieves an int value that is part of command line arguments this process was
		// started with 
		// Note: Arguments are always null on android - their usage only makes sense on
		// an OS capable of hosting a server
		// -------------------------------------------------------------------------------
		public static int GetArgumentInt(string name)
		{
			string[] args = System.Environment.GetCommandLineArgs();
			
			int idx = args.ToList().FindIndex(x => x == name);
			
			if (idx == -1 || idx == args.Length)
				return -1;
			
			return int.Parse(args[idx+1]);
			
		}

		// -------------------------------------------------------------------------------
		// GetArgumentsString
		// Note: The first argument is always the process name or empty
		// Note: Arguments are always null on android - their usage only makes sense on
		// an OS capable of hosting a server
		// -------------------------------------------------------------------------------
		public static string GetArgumentsString
		{
			get {
				string[] args = System.Environment.GetCommandLineArgs();
				return args != null ? String.Join(" ", args.Skip(1).ToArray()) : "";
			}
		}

		// -------------------------------------------------------------------------------
		// GetProcessPath
		// Note: Arguments are always null on android - their usage only makes sense on
		// an OS capable of hosting a server
		// -------------------------------------------------------------------------------
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

		// ============================== VALIDATION =====================================
		
		// -------------------------------------------------------------------------------
		// -------------------------------------------------------------------------------
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

		// -------------------------------------------------------------------------------
		// Very simple password validation (must not be empty) so it works with hashed
		// passwords as well.
		// Could be expanded here if required
		// -------------------------------------------------------------------------------
		public static bool IsAllowedPassword(string _text)
		{
			return !String.IsNullOrWhiteSpace(_text);
		}

		// -------------------------------------------------------------------------------
		public static string PBKDF2Hash(string text, string salt)
		{
			byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
			Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(text, saltBytes, 10000);
			byte[] hash = pbkdf2.GetBytes(20);
			return BitConverter.ToString(hash).Replace("-", string.Empty);
		}
		
		// ============================== CONVERSION =====================================
		
		// -------------------------------------------------------------------------------
		// ConvertToUnixTimestamp
		// -------------------------------------------------------------------------------
		public static double ConvertToUnixTimestamp(DateTime date)
		{
			DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			TimeSpan diff = date.ToUniversalTime() - origin;
			return Math.Floor(diff.TotalSeconds);
		}
		
		// -------------------------------------------------------------------------------
		// IntArrayToString
		// -------------------------------------------------------------------------------
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

		// -------------------------------------------------------------------------------
		// IntStringToArray
		// -------------------------------------------------------------------------------
		public static int[] IntStringToArray(string array)
		{
			if (string.IsNullOrWhiteSpace(array)) return null;
			string[] tokens = array.Split(CONST_DELIMITER);
			int[] arrayInt = Array.ConvertAll<string, int>(tokens, int.Parse);
			return arrayInt;
		}

		// -------------------------------------------------------------------------------
		// ArrayContains (int)
		// -------------------------------------------------------------------------------
		public static bool ArrayContains(int[] array, int number)
		{
			foreach (int element in array)
			{
				if (element == number)
					return true;
			}
			return false;
		}

		// -------------------------------------------------------------------------------
		// ArrayContains (string)
		// -------------------------------------------------------------------------------
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

		// -------------------------------------------------------------------------------
		// RemoveFromArray (string)
		// -------------------------------------------------------------------------------
		public static string[] RemoveFromArray(string[] array, string text)
		{
			return array.Where(x => x != text).ToArray();
		}
	
		// -------------------------------------------------------------------------------
		// RemoveFromArray (int)
		// -------------------------------------------------------------------------------
		public static int[] RemoveFromArray(int[] array, int number)
		{
			return array.Where(x => x != number).ToArray();
		}
		
		// ========================= PLAYER PREFERENCES ==================================
		
		// -------------------------------------------------------------------------------
		// PlayerPrefsSetString
		// Only set if exists (and is equal to "oldValue"") or "set" is true
		// -------------------------------------------------------------------------------
		public static void PlayerPrefsSetString(string keyName, string newValue, string oldValue="", bool set=false)
		{
			if (PlayerPrefs.HasKey(keyName) || set)
				if ((!set && PlayerPrefs.GetString(keyName) == oldValue) || (!set && oldValue == "") || set)
					PlayerPrefs.SetString(keyName, newValue);
		}
		
		// -------------------------------------------------------------------------------
		// PlayerPrefsSetInt
		// Only set if exists (and is equal to "oldValue"") or "set" is true
		// -------------------------------------------------------------------------------
		public static void PlayerPrefsSetInt(string keyName, int newValue, int oldValue=-1, bool set=false)
		{
			if (PlayerPrefs.HasKey(keyName) || set)
				if ((!set && PlayerPrefs.GetInt(keyName) == oldValue) || (!set && oldValue == -1) || set)
					PlayerPrefs.SetInt(keyName, newValue);
		}
		
		// ============================== UI STUFF =======================================
		
		// -------------------------------------------------------------------------------
		// AnyInputFocused
		// -------------------------------------------------------------------------------
		public static bool AnyInputFocused
    	{
    		get {
        		foreach (Selectable selectable in Selectable.allSelectablesArray)
            		if (selectable is InputField && ((InputField)selectable).isFocused)
           		     	return true;
        		return false;
        	}
    	}
    	
		// ========================= CAMERA & 3d STUFF ===================================
		
		// -------------------------------------------------------------------------------
		// ClampAngleBetweenMinAndMax
		// -------------------------------------------------------------------------------
		public static float ClampAngleBetweenMinAndMax(float angle, float min, float max)
		{
			if (angle < -360)
				angle += 360;
			if (angle > 360)
				angle -= 360;
			return Mathf.Clamp(angle, min, max);
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================