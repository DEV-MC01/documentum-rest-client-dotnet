using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System;

namespace Emc.Documentum.Rest.Test
{
	// ========================================================================================================================
	/// <summary>
	/// Class containing static utility functions only.
	/// </summary>
	// ========================================================================================================================
	internal static class Utl
	{
		//--------------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Converts the supplied object to a string value.
		/// If the object is not string compatible, the supplied default value is returned.
		/// </summary>
		/// <param name="stringValue">The object to be converted.</param>
		/// <param name="defaultValue">The value to be returned if the stringValue is not string compatible or is a zero length string.</param>
		/// <returns>The value converted to string, or the defaultValue.</returns>
		public static string SafeString(object stringValue, string defaultValue)
		{
			if (stringValue == null) return defaultValue ?? "";
			if (stringValue.GetType().IsValueType) return stringValue.ToString();
			if (!(stringValue is string) || ((string)stringValue).Length == 0) return defaultValue ?? "";
			return (string)stringValue;
		}

		//--------------------------------------------------------------------------------------------------------------------------------
		/// <summary>Return a safe version of supplied string, i.e. not a <c>null</c> value</summary>
		/// <param name="stringValue">The string to convert</param>
		/// <param name="defaultValue">The value to be returned if the stringValue is <c>null</c> or a zero length string.</param>
		/// <returns>The value as non-null string, or <paramref name="defaultValue"/>.</returns>
		public static string SafeString(string stringValue, string defaultValue)
		{
			return string.IsNullOrEmpty(stringValue) ? (defaultValue ?? "") : stringValue;
		}

		//--------------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Converts the supplied object to a string value.
		/// If the object is not string compatible, an empty string is returned.
		/// </summary>
		/// <param name="stringValue">The object to be converted.</param>
		/// <returns>The value converted to string, or an empty string.</returns>
		public static string SafeString(string stringValue)
		{
			return Utl.SafeString(stringValue, "");
		}

		//--------------------------------------------------------------------------------------------------------------------------------
		/// <summary>Convert the supplied string to a byte value. If the convertion fails, the defaultValue is returned.</summary>
		/// <param name="byteString">The string to be converted.</param>
		/// <param name="defaultValue">The value to return if the convertion fails.</param>
		/// <returns>The converted string or the defaultValue if the convertion fails.</returns>
		public static byte ToByte(string byteString, byte defaultValue)
		{
			if (string.IsNullOrEmpty(byteString))
			{
				return defaultValue;
			}

			byte byteResult;
			if (!byte.TryParse(byteString, out byteResult))
			{
				byteResult = 0;
			}
			return byteResult;
		}

		//--------------------------------------------------------------------------------------------------------------------------------
		/// <summary>Convert the supplied object to an byte value. If the convertion fails, the defaultValue is returned.</summary>
		/// <param name="byteValue">The object to be converted.</param>
		/// <param name="defaultValue">The value to return if the convertion fails.</param>
		/// <returns>The converted object value or the defaultValue if the convertion fails.</returns>
		public static byte ToByte(object byteValue, byte defaultValue)
		{
			if (byteValue == null)
			{
				return defaultValue;
			}
			else if (byteValue is byte)
			{
				return (byte)byteValue;
			}
			else if (byteValue is int)
			{
				return (byte)((int)byteValue & 255);
			}
			else if (byteValue is string)
			{
				return Utl.ToByte((string)byteValue, defaultValue);
			}
			else
			{
				try
				{
					return (byte)byteValue;
				}
				catch (InvalidCastException)
				{
					return defaultValue;
				}
			}
		}

		//--------------------------------------------------------------------------------------------------------------------------------
		/// <summary>Convert the supplied object to an byte value. If the convertion fails, zero is returned.</summary>
		/// <param name="byteValue">The object to be converted.</param>
		/// <returns>The converted object value or <c>0</c> if the convertion fails.</returns>
		public static byte ToByte(object byteValue)
		{
			return Utl.ToByte(byteValue, 0);
		}

		//--------------------------------------------------------------------------------------------------------------------------------
		/// <summary>Convert the supplied object to an integer value. If the convertion fails, zero is returned.</summary>
		/// <param name="intValue">The object to be converted.</param>
		/// <returns>The converted object value or <c>0</c> if the convertion fails.</returns>
		public static int ToInt(object intValue)
		{
			return Utl.ToInt(intValue, 0);
		}

		//--------------------------------------------------------------------------------------------------------------------------------
		/// <summary>Convert the supplied object to an integer value. If the convertion fails, the defaultValue is returned.</summary>
		/// <param name="intValue">The object to be converted.</param>
		/// <param name="defaultValue">The value to return if the convertion fails.</param>
		/// <returns>The converted object value or the defaultValue if the convertion fails.</returns>
		public static int ToInt(object intValue, int defaultValue)
		{
			if (intValue == null) return defaultValue;
			if (intValue is DBNull) return defaultValue;
			if (intValue is int) return (int)intValue;
			if (intValue is string) return Utl.ToInt((string)intValue, defaultValue);
			if (intValue is long) return Utl.ToInt(((long)intValue).ToString(), defaultValue);
			if (intValue is decimal) return decimal.ToInt32((decimal)intValue);

			return UtlHelper.FuncResultOrDefault(intValue, defaultValue, (val, def) => (int)intValue);
		}

		#region Crypt
		// ========================================================================================================================
		/// <summary>Class containing functions for crypt/decrypt of strings</summary>
		// ========================================================================================================================
		internal static class Crypt
		{
			/// <summary>
			///		8 random bytes to form the DES crypt key.
			///		****** IF THIS DEFINITION IS ALTERED, ALL EXISTING CRYPTED VALUES ARE AUTOMATICALLY CORRUPTED AND CAN NOT BE DECRYPTED.
			///	</summary>
			private static byte[] KEY_64 = new byte[] { 30, 18, 121, 156, 73, 2, 222, 31 };

			/// <summary>
			///		8 random bytes to form the DES Initialization Vector (IV).
			///		The IV is used to encrypt the first block of text so that any repetitive patterns are not apparent.
			///		****** IF THIS DEFINITION IS ALTERED, ALL EXISTING CRYPTED VALUES ARE AUTOMATICALLY CORRUPTED AND CAN NOT BE DECRYPTED.
			/// </summary>
			private static byte[] IV_64 = new byte[] { 45, 133, 226, 59, 86, 29, 107, 7 };

			/// <summary>
			///		24 random bytes to form the TripleDES crypt key.
			///		****** IF THIS DEFINITION IS ALTERED, ALL EXISTING CRYPTED VALUES ARE AUTOMATICALLY CORRUPTED AND CAN NOT BE DECRYPTED.
			///	</summary>
			private static byte[] KEY_192 = new byte[]{47, 13, 81, 126, 178, 7, 208, 29
																								,13, 177, 24, 88, 21, 241, 125, 182
																								, 1, 73, 17, 214, 129, 37, 171, 157
																								};

			/// <summary>
			///		24 random bytes to form the TripleDES Initialization Vector (IV).
			///		****** IF THIS DEFINITION IS ALTERED, ALL EXISTING CRYPTED VALUES ARE AUTOMATICALLY CORRUPTED AND CAN NOT BE DECRYPTED.
			///	</summary>
			private static byte[] IV_192 = new byte[]   {46, 16, 84, 129, 108, 9, 228, 21
																								,11, 117, 124, 188, 121, 141, 225, 82
																								, 8, 173, 117, 14, 29, 37, 71, 19
																								};

			/// <summary>We hold a single instance used as the seed randomizer for other instances.</summary>
			private static Random _randomMaster;

			/// <summary>
			///		Maximum number of fill-in characters.
			///		This value is used to distinguishe strong crypted strings from weak crypted strings on decrypt.
			///		Therefore:
			///			****** IF THE VALUE OF THIS CONST IS REDEFINED DECRYPT OF EXISTING CRYPTED VALUES MAY FAIL ******
			/// </summary>
			private const int EXTRACHARS_MAX = 30;

			/// <summary>Minimum number of fill-in characters.</summary>
			private const int EXTRACHARS_MIN = 10;

			/// <summary>
			///		A crypted string ends with a 2 digit hex value indicating number of extra characters added to the result.
			///		For DES crypting we do not add any extra characters, but use this value to indicate DES crypting.
			///	</summary>
			private const int DES_INDICATOR = 2;

			/// <summary>
			///		A crypted string ends with a 2 digit hex value indicating number of extra characters added to the result.
			///		For TripleDES crypting we do not add any extra characters, but use this value to indicate TripleDES crypting.
			///	</summary>
			private const int TRIPLEDES_INDICATOR = 4;

			/// <summary>Size of the ring buffer used during weak crypting</summary>
			private const int WEAK_RINGSIZE = 7;

			/// <summary>The optional prefix to use for crypted values</summary>
			public const string PREFIX = "crypted:";

			// ---------------------------------------------------------------------------------------------------------------------
			/// <summary>
			///		Create a Randomizer that is guaranteed to be initialized with a random seed within current process,
			///		even when two instances is created within the same tick.
			///	</summary>
			public static Random CreateRandomizer()
			{
				lock (typeof(Crypt))
				{
					if (_randomMaster == null) _randomMaster = new Random(~unchecked((int)DateTime.Now.Ticks));
				}
				return new Random(_randomMaster.Next());
			}

			// -------------------------------------------------------------------------------------------------------------------------
			/// <summary>Checks whether the supplied string if it starts with "crypted:"</summary>
			/// <param name="cryptedString">The string to verify.</param>
			/// <returns>true if the string starts with "crypted:", else false.</returns>
			public static bool IsCrypted(string cryptedString)
			{
				return Utl.SafeString(cryptedString).ToLower().StartsWith(PREFIX);
			}

			// -------------------------------------------------------------------------------------------------------------------------
			/// <summary>Crypt the supplied string using strong crypting.</summary>
			/// <param name="source">A plain string.</param>
			/// <param name="includePrefix">An indicator on whether the returned value should be prefixed by "crypted:"</param>
			/// <returns>A strong crypted string, if the supplied source is not an empty string, else an empty string is returned.</returns>
			/// <remarks>
			///		Weak crypting is less secure than Strong crypting,
			///		but the length of the resulting string from Weak crypting is approximately 1/3 of Strong crypting.
			///		Weak crypting should be used for values that should not be exposed in clear text,
			///		like e.g. secret query string parameters in hyperlinks exposed in emails etc.,
			///		where the result of potential hacking is has limited impact.
			///		Then use Strong crypting for passwords in configuration files etc.
			///
			///		Even though Weak crypting is less secure, it will take significant effort to hack it.
			/// </remarks>
			public static string CryptStrong(string source, bool includePrefix)
			{
				return (includePrefix ? PREFIX : "") + Crypt.CryptStrong(source);
			}

			// -------------------------------------------------------------------------------------------------------------------------
			/// <summary>Crypt the supplied string using strong crypting.</summary>
			/// <param name="source">A plain string.</param>
			/// <returns>A strong crypted string, if the supplied source is not an empty string, else an empty string is returned.</returns>
			/// <remarks>
			///		Weak crypting is less secure than Strong crypting,
			///		but the length of the resulting string from Weak crypting is approximately 1/3 of Strong crypting.
			///		Weak crypting should be used for values that should not be exposed in clear text,
			///		like e.g. secret query string parameters in hyperlinks exposed in emails etc.,
			///		where the result of potential hacking is has limited impact.
			///		Then use Strong crypting for passwords in configuration files etc.
			///
			///		Even though Weak crypting is less secure, it will take significant effort to hack it.
			/// </remarks>
			public static string CryptStrong(string source)
			{
				source = Utl.SafeString(source).Trim();
				if (source.Length == 0) return "";

				var csb = new CryptedStringBuilder();
				int extraChars = csb.Generator.Next(EXTRACHARS_MIN, EXTRACHARS_MAX);
				var unicode = new UnicodeEncoding();

				for (int idx = 0; idx < source.Length; idx++)
				{
					byte cryptKey = csb.RandomHex2();
					csb.Append(cryptKey);
					csb.Append();
					byte[] val = unicode.GetBytes(source.Substring(idx, 1));
					csb.Append(cryptKey ^ val[0]);
				}

				for (int idx = 0; idx < extraChars; idx++)
				{
					csb.Append();
				}
				csb.Append(extraChars);

				return csb.ToString();
			}

			// -------------------------------------------------------------------------------------------------------------------------
			/// <summary>
			///		Crypt the supplied string using weak crypting.
			///	</summary>
			/// <param name="source">A plain string.</param>
			/// <param name="includePrefix">An indicator on whether the returned value should be prefixed by "crypted:"</param>
			/// <returns>A weak crypted string, if the supplied source is not an empty string, else an empty string is returned.</returns>
			/// <remarks>
			///		Weak crypting is less secure than Strong crypting,
			///		but the length of the resulting string from Weak crypting is approximately 1/3 of Strong crypting.
			///		Weak crypting should be used for values that should not be exposed in clear text,
			///		like e.g. secret query string parameters in hyperlinks exposed in emails etc.,
			///		where the result of potential hacking is has limited impact.
			///		Then use Strong crypting for passwords in configuration files etc.
			///
			///		Even though Weak crypting is less secure, it will take significant effort to hack it.
			/// </remarks>
			public static string CryptWeak(string source, bool includePrefix)
			{
				return (includePrefix ? PREFIX : "") + Crypt.CryptWeak(source);
			}

			// -------------------------------------------------------------------------------------------------------------------------
			/// <summary>
			///		Crypt the supplied string using weak crypting.
			///	</summary>
			/// <param name="source">A plain string.</param>
			/// <returns>A weak crypted string, if the supplied source is not an empty string, else an empty string is returned.</returns>
			/// <remarks>
			///		Weak crypting is less secure than Strong crypting,
			///		but the length of the resulting string from Weak crypting is approximately 1/3 of Strong crypting.
			///		Weak crypting should be used for values that should not be exposed in clear text,
			///		like e.g. secret query string parameters in hyperlinks exposed in emails etc.,
			///		where the result of potential hacking is has limited impact.
			///		Then use Strong crypting for passwords in configuration files etc.
			///
			///		Even though Weak crypting is less secure, it will take significant effort to hack it.
			/// </remarks>
			public static string CryptWeak(string source)
			{
				source = Utl.SafeString(source).Trim();
				if (source.Length == 0) return "";

				//System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
				var unicode = new UnicodeEncoding();

				var csb = new CryptedStringBuilder(WEAK_RINGSIZE);
				int extraChars = csb.Generator.Next(EXTRACHARS_MIN / 2, EXTRACHARS_MAX / 2);

				byte cryptKey = csb.RandomHex2();
				csb.Append(cryptKey);

				int fillIn = extraChars;
				for (int idx = 0; idx < source.Length; idx++)
				{
					byte[] val = unicode.GetBytes(source.Substring(idx, 1));
					byte cryptedVal = Utl.ToByte(cryptKey ^ val[0]);
					csb.Append(cryptedVal);

					if (fillIn > 0)
					{
						fillIn--;
						cryptKey = csb.RandomHex2();
						csb.Append(cryptKey);
					}
					else
					{
						cryptKey = csb.Mask;
					}
				}

				for (int idx = 0; idx < fillIn; idx++)
				{
					csb.Append();
				}

				csb.Append(extraChars + EXTRACHARS_MAX);

				return csb.ToString();
			}

			// -------------------------------------------------------------------------------------------------------------------------
			/// <summary>Crypt the supplied string using Triple DES crypting with 192 bit key.</summary>
			/// <param name="source">A plain string.</param>
			/// <param name="includePrefix">An indicator on whether the returned value should be prefixed by "crypted:"</param>
			/// <returns>A strong crypted string, if the supplied source is not an empty string, else an empty string is returned.</returns>
			internal static string CryptDESStrong(string source, bool includePrefix)
			{
				return (includePrefix ? PREFIX : "") + Crypt.CryptDESStrong(source);
			}

			// -------------------------------------------------------------------------------------------------------------------------
			/// <summary>Crypt the supplied string using Triple DES crypting with 192 bit key.</summary>
			/// <param name="source">A plain string.</param>
			/// <returns>A weak crypted string, if the supplied source is not an empty string, else an empty string is returned.</returns>
			internal static string CryptDESStrong(string source)
			{
				source = Utl.SafeString(source).Trim();
				if (source.Length == 0) return "";

				var cryptoProvider = new TripleDESCryptoServiceProvider();
				return Crypt.CryptDESCommon(source, cryptoProvider.CreateEncryptor(KEY_192, IV_192), TRIPLEDES_INDICATOR);
			}

			// -------------------------------------------------------------------------------------------------------------------------
			/// <summary>Crypt the supplied string using DES crypting with 64 bit key.</summary>
			/// <param name="source">A plain string.</param>
			/// <param name="includePrefix">An indicator on whether the returned value should be prefixed by "crypted:"</param>
			/// <returns>A weak crypted string, if the supplied source is not an empty string, else an empty string is returned.</returns>
			internal static string CryptDESWeak(string source, bool includePrefix)
			{
				return (includePrefix ? PREFIX : "") + Crypt.CryptDESWeak(source);
			}

			// -------------------------------------------------------------------------------------------------------------------------
			/// <summary>Crypt the supplied string using DES crypting with 64 bit key.</summary>
			/// <param name="source">A plain string.</param>
			/// <returns>A weak crypted string, if the supplied source is not an empty string, else an empty string is returned.</returns>
			internal static string CryptDESWeak(string source)
			{
				source = Utl.SafeString(source).Trim();
				if (source.Length == 0) return "";

				var cryptoProvider = new DESCryptoServiceProvider();
				return Crypt.CryptDESCommon(source, cryptoProvider.CreateEncryptor(KEY_64, IV_64), DES_INDICATOR);
			}

			// -------------------------------------------------------------------------------------------------------------------------
			/// <summary>Crypt the supplied string using supplied DES transformer.</summary>
			/// <param name="source">A plain string.</param>
			/// <param name="transformer">A DES crypty transformer initialized with a key and an initialization vector.</param>
			/// <param name="indicator">The crypt method indicator to append to the result</param>
			/// <returns>A DES crypted string is returned.</returns>
			private static string CryptDESCommon(string source, ICryptoTransform transformer, int indicator)
			{
				var memoryStream = new System.IO.MemoryStream();
				var cryptoStream = new CryptoStream(memoryStream, transformer, CryptoStreamMode.Write);
				var writer = new System.IO.StreamWriter(cryptoStream);

				writer.Write(source);
				writer.Flush();
				cryptoStream.FlushFinalBlock();
				memoryStream.Flush();

				// Convert back to a string
				return Convert.ToBase64String(memoryStream.GetBuffer(), 0, Utl.ToInt(memoryStream.Length)) + indicator.ToString("x2");
			}

			// -------------------------------------------------------------------------------------------------------------------------
			/// <summary>Decrypt supplied string if it starts with "crypted:"</summary>
			/// <param name="cryptedString">The string to conditionally decrypt.</param>
			/// <returns>A decrypted string, if the input string starts with "crypted:", else the input string is returned unmodified.</returns>
			public static string DecryptStringConditional(string cryptedString)
			{
				if (Utl.SafeString(cryptedString).ToLower().StartsWith(PREFIX))
				{
					return Crypt.DecryptString(cryptedString);
				}
				else
				{
					return Utl.SafeString(cryptedString);
				}
			}

			// -------------------------------------------------------------------------------------------------------------------------
			/// <summary>
			///		Decrypt the supplied string.
			///	</summary>
			/// <param name="cryptedString">
			///		The string containing an encrypted value.
			///		Decryption automatically detects what algorithm has been used for encrypting (weak or strong).
			/// </param>
			/// <returns>A decrypted string, if the input is not an empty string, else a string of zero length.</returns>
			public static string DecryptString(string cryptedString)
			{
				string decryptedString;
				try
				{
					cryptedString = Utl.SafeString(cryptedString).Trim();
					if (cryptedString.Length == 0) return "";

					// Remove optional prefix "crypted:"
					if (cryptedString.ToLower().StartsWith(PREFIX)) cryptedString = cryptedString.Substring(PREFIX.Length);

					// The last to hex digits specify number of extra characters generated
					int x = cryptedString.Length - 2;
					if (x < 0) return "";
					int extraChars = Crypt.FromHex(cryptedString.Substring(x));

					if (extraChars <= 0)
					{
						return ""; // Corrupted value
					}
					else if (extraChars > EXTRACHARS_MAX)
					{
						if ((extraChars - EXTRACHARS_MAX) > cryptedString.Length) return ""; // Corrupted value
						decryptedString = Crypt.DecryptWeak(cryptedString, extraChars - EXTRACHARS_MAX);
					}
					else if (extraChars == DES_INDICATOR)
					{
						decryptedString = Crypt.DecryptDESWeak(cryptedString);
					}
					else if (extraChars == TRIPLEDES_INDICATOR)
					{
						decryptedString = Crypt.DecryptDESStrong(cryptedString);
					}
					else if (extraChars > cryptedString.Length) // Corrupted value
					{
						return "";
					}
					else
					{
						decryptedString = Crypt.DecryptStrong(cryptedString, extraChars);
					}
				}
				catch
				// The only plausible reason for getting an exception here is if the input value is corrupted.
				// In such case, we just ignore the corrupted value and return a zero length string.
				{
					return "";
				}

				decryptedString = decryptedString ?? string.Empty;
				return decryptedString.StartsWith("\0")
					? "" // if the decrypted string starts with the null character '\0' which normally points to the end of the string, it indicates that the encrypted string was incorrect
					: decryptedString;
			}

			// -------------------------------------------------------------------------------------------------------------------------
			/// <summary>Decrypt a strong crypted string.</summary>
			/// <param name="cryptedString">The string containing a crypted value.</param>
			/// <param name="extraChars">The number of extra characters filled in the result.</param>
			/// <returns>The decrypted result.</returns>
			private static string DecryptStrong(string cryptedString, int extraChars)
			{
				// Remove the extra characters
				int x = cryptedString.Length - ((extraChars + 1) * 2);
				if (x <= 0 || x >= cryptedString.Length) return "";
				cryptedString = cryptedString.Substring(0, x);

				// The actual value is stores as every third hex
				var sb = new StringBuilder();
				var unicode = new UnicodeEncoding();
				var val = new byte[] { 0, 0 };
				for (int idx = 0; idx <= cryptedString.Length - (3 * 2); idx += 3 * 2)
				{
					byte cryptKey = Crypt.FromHex(cryptedString.Substring(idx, 2));
					val[0] = Utl.ToByte(Crypt.FromHex(cryptedString.Substring(idx + 4, 2)) ^ cryptKey);
					sb.Append(unicode.GetString(val, 0, val.Length));
				}
				return sb.ToString();
			}

			// -------------------------------------------------------------------------------------------------------------------------
			/// <summary>Decrypt a weak crypted string.</summary>
			/// <param name="cryptedString">The string containing a crypted value.</param>
			/// <param name="extraChars">The number of extra characters filled in the result.</param>
			/// <returns>The decrypted result.</returns>
			private static string DecryptWeak(string cryptedString, int extraChars)
			{
				int resultSize = (cryptedString.Length / 2) - (extraChars + 2);
				if (resultSize < 1) return "";
				int resultCount = 0;
				var ring = new RingBuffer(WEAK_RINGSIZE);

				// The actual value is stores as every third hex
				var sb = new StringBuilder();
				var unicode = new UnicodeEncoding();
				var val = new byte[] { 0, 0 };
				int idx = 0;
				byte cryptKey = Crypt.FromHex(cryptedString.Substring(idx, 2));
				ring.Append(cryptKey);
				idx += 2;
				while (true)
				{
					val[0] = Crypt.FromHex(cryptedString.Substring(idx, 2));
					ring.Append(val[0]);
					val[0] = Utl.ToByte(val[0] ^ cryptKey);
					sb.Append(unicode.GetString(val, 0, val.Length));

					resultCount++;
					if (resultCount == resultSize) break;

					idx += 2;
					if (extraChars > 0)
					{
						extraChars--;
						cryptKey = Crypt.FromHex(cryptedString.Substring(idx, 2));
						ring.Append(cryptKey);
						idx += 2;
					}
					else
					{
						cryptKey = ring.Current;
					}

				}
				return sb.ToString();
			}

			// -------------------------------------------------------------------------------------------------------------------------
			/// <summary>Decrypt a string crypted with TripleDES 192 bit key.</summary>
			/// <param name="cryptedString">The string containing a crypted value.</param>
			/// <returns>The decrypted result.</returns>
			private static string DecryptDESStrong(string cryptedString)
			{
				var cryptoProvider = new TripleDESCryptoServiceProvider();
				return Crypt.DecryptDESCommon(cryptedString, cryptoProvider.CreateDecryptor(KEY_192, IV_192));
			}

			// -------------------------------------------------------------------------------------------------------------------------
			/// <summary>Decrypt a string crypted with DES 64 bit key.</summary>
			/// <param name="cryptedString">The string containing a crypted value.</param>
			/// <returns>The decrypted result.</returns>
			private static string DecryptDESWeak(string cryptedString)
			{
				var cryptoProvider = new DESCryptoServiceProvider();
				return Crypt.DecryptDESCommon(cryptedString, cryptoProvider.CreateDecryptor(KEY_64, IV_64));
			}

			// -------------------------------------------------------------------------------------------------------------------------
			/// <summary>Decrypt a DES crypted string using supplied DES transformer.</summary>
			/// <param name="cryptedString">The string containing a crypted value.</param>
			/// <param name="transformer">A DES crypty transformer initialized with a key and an initialization vector.</param>
			/// <returns>The decrypted result.</returns>
			private static string DecryptDESCommon(string cryptedString, ICryptoTransform transformer)
			{
				System.IO.MemoryStream memoryStream = null;
				CryptoStream cryptoStream = null;
				System.IO.StreamReader reader = null;
				string result = "";
				try
				{
					cryptedString = cryptedString.Substring(0, cryptedString.Length - 2); // Remove the indicator
					byte[] buffer = Convert.FromBase64String(cryptedString); // convert from string to byte array
					memoryStream = new System.IO.MemoryStream(buffer);
					cryptoStream = new CryptoStream(memoryStream, transformer, CryptoStreamMode.Read);
					reader = new System.IO.StreamReader(cryptoStream);

					result = reader.ReadToEnd();
				}
				finally
				{
					if (reader != null) reader.Close();
					if (cryptoStream != null) cryptoStream.Close();
					if (memoryStream != null) memoryStream.Close();
				}
				return result;
			}

			// -------------------------------------------------------------------------------------------------------------------------
			/// <summary>Parse supplied string as a hex number.</summary>
			/// <param name="hexString">The string to parse.</param>
			/// <returns>The number represented by the hexString.</returns>
			private static byte FromHex(string hexString)
			{
				int i;
				if (!int.TryParse(hexString, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out i))
				{
					return 0;
				}
				return Utl.ToByte(i);
			}

			// ========================================================================================================================
			/// <summary>Class holding the state for the crypted result being built.</summary>
			// ========================================================================================================================
			private class CryptedStringBuilder
			{
				private readonly Random _generator;
				private readonly StringBuilder _sb;
				private readonly RingBuffer _ring;

				// -------------------------------------------------------------------------------------------------------------------------
				public CryptedStringBuilder() : this(Crypt.WEAK_RINGSIZE) { }

				// -------------------------------------------------------------------------------------------------------------------------
				public CryptedStringBuilder(int ringSize)
				{
					_generator = Crypt.CreateRandomizer(); // Get an instance with a unique seed
					_ring = new RingBuffer(ringSize);
					_sb = new StringBuilder();
				}

				// -------------------------------------------------------------------------------------------------------------------------
				public Random Generator { get { return _generator; } }

				// -------------------------------------------------------------------------------------------------------------------------
				public void Append()
				{
					this.Append(this.RandomHex2());
				}

				// -------------------------------------------------------------------------------------------------------------------------
				public void Append(int val)
				{
					this.Append(Utl.ToByte(val));
				}

				// -------------------------------------------------------------------------------------------------------------------------
				public void Append(byte val)
				{
					_sb.Append(this.Hex2(val));
					_ring.Append(val);
				}

				// -------------------------------------------------------------------------------------------------------------------------
				public byte Mask { get { return _ring.Current; } }

				// -------------------------------------------------------------------------------------------------------------------------
				public override string ToString() { return _sb.ToString(); }

				// -------------------------------------------------------------------------------------------------------------------------
				/// <summary>Generate a random number that will result in two hex digits.</summary>
				/// <returns>A random number that will render as two hex digits.</returns>
				public byte RandomHex2()
				{
					return Utl.ToByte(_generator.Next(16, 254));
				}

				// -------------------------------------------------------------------------------------------------------------------------
				/// <summary>Format supplied value as a two digit hex number.</summary>
				private string Hex2(int intValue)
				{
					return _generator.Next(1, 100) > 50 ? intValue.ToString("x2") : intValue.ToString("X2");
				}
			}

			// ========================================================================================================================
			/// <summary>Class holding a ring buffer of bytes.</summary>
			// ========================================================================================================================
			private class RingBuffer
			{
				private readonly byte[] _bytes;
				private int _idx;

				// -------------------------------------------------------------------------------------------------------------------------
				public RingBuffer(int size)
				{
					_bytes = new byte[size];
					_idx = 0;
				}

				// -------------------------------------------------------------------------------------------------------------------------
				public void Append(byte val)
				{
					_bytes[_idx] = val;
					_idx++;
					if (_idx >= _bytes.Length) _idx = 0;
				}

				// -------------------------------------------------------------------------------------------------------------------------
				public byte Current { get { return _bytes[_idx]; } }
			}
		}
		#endregion

		private static class UtlHelper
		{
			public static T FuncResultOrDefault<T>(object valueToConvert, T defaultValue, Func<object, T, T> func)
			{
				try
				{
					return func(valueToConvert, defaultValue);
				}
				catch (InvalidCastException)
				{
					return defaultValue;
				}
				catch (FormatException)
				{
					return defaultValue;
				}
				catch (ArgumentNullException)
				{
					return defaultValue;
				}
			}
		}
	}
}
