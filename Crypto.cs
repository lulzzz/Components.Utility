﻿#region Related components
using System;
using System.Net;
using System.Text;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;
#endregion

namespace net.vieapps.Components.Utility
{
	/// <summary>
	/// Static servicing methods for working with cryptos
	/// </summary>
	public static partial class CryptoService
	{

		#region Hash array of bytes/string
		static Dictionary<string, Func<HashAlgorithm>> HashFactories = new Dictionary<string, Func<HashAlgorithm>>(StringComparer.OrdinalIgnoreCase)
		{
			{ "md5", () => MD5.Create() },
			{ "sha1", () => SHA1.Create() },
			{ "sha256", () => SHA256.Create() },
			{ "sha384", () => SHA384.Create() },
			{ "sha512", () => SHA512.Create() },
			{ "ripemd", () => RIPEMD160.Create() },
			{ "ripemd160", () => RIPEMD160.Create() },
			{ "blake", () => new HMACBlake2B(128) },
			{ "blake128", () => new HMACBlake2B(128) },
			{ "blake256", () => new HMACBlake2B(256) },
			{ "blake384", () => new HMACBlake2B(384) },
			{ "blake512", () => new HMACBlake2B(512) },
		};

		/// <summary>
		/// Gets a hashser
		/// </summary>
		/// <param name="mode">Mode of the hasher (md5, sha1, sha256, sha384, sha512, ripemd/ripemd160, blake/blake128, blake256, blake384, blake512)</param>
		/// <returns></returns>
		public static HashAlgorithm GetHasher(string mode = "SHA256")
		{
			if (!CryptoService.HashFactories.TryGetValue(mode, out Func<HashAlgorithm> func))
				func = () => SHA256.Create();
			return func();
		}

		/// <summary>
		/// Gets hash of this array of bytes
		/// </summary>
		/// <param name="bytes"></param>
		/// <param name="mode">Mode of the hasher (md5, sha1, sha256, sha384, sha512, ripemd/ripemd160, blake/blake128, blake256, blake384, blake512)</param>
		/// <returns></returns>
		public static byte[] GetHash(this byte[] bytes, string mode = "SHA256")
		{
			if (bytes == null || bytes.Length < 1)
				throw new ArgumentException("Invalid", nameof(bytes));

			using (var hasher = CryptoService.GetHasher(mode))
			{
				return hasher.ComputeHash(bytes);
			}
		}

		/// <summary>
		/// Gets hash of this string
		/// </summary>
		/// <param name="string"></param>
		/// <param name="mode">Mode of the hasher (md5, sha1, sha256, sha384, sha512, ripemd/ripemd160, blake/blake128, blake256, blake384, blake512)</param>
		/// <returns></returns>
		public static byte[] GetHash(this string @string, string mode = "SHA256")
		{
			return string.IsNullOrWhiteSpace(@string)
				? new byte[0]
				: @string.ToBytes().GetHash(mode);
		}

		/// <summary>
		/// Gets MD5 hash of this string
		/// </summary>
		/// <param name="string"></param>
		/// <returns></returns>
		public static byte[] GetMD5Hash(this string @string)
		{
			return @string.GetHash("MD5");
		}

		/// <summary>
		/// Gets MD5 hash of this string
		/// </summary>
		/// <param name="string"></param>
		/// <param name="toBase64"></param>
		/// <returns></returns>
		public static string GetMD5(this string @string, bool toBase64 = false)
		{
			return toBase64
				? @string.GetMD5Hash().ToBase64()
				: @string.GetMD5Hash().ToHexa();
		}

		/// <summary>
		/// Gets SHA1 hash of this string
		/// </summary>
		/// <param name="string"></param>
		/// <returns></returns>
		public static byte[] GetSHA1Hash(this string @string)
		{
			return @string.GetHash("SHA1");
		}

		/// <summary>
		/// Gets SHA1 hash of this string
		/// </summary>
		/// <param name="string"></param>
		/// <param name="toBase64"></param>
		/// <returns></returns>
		public static string GetSHA1(this string @string, bool toBase64 = false)
		{
			return toBase64
				? @string.GetSHA1Hash().ToBase64()
				: @string.GetSHA1Hash().ToHexa();
		}

		/// <summary>
		/// Gets SHA256 hash of this string
		/// </summary>
		/// <param name="string"></param>
		/// <returns></returns>
		public static byte[] GetSHA256Hash(this string @string)
		{
			return @string.GetHash("SHA256");
		}

		/// <summary>
		/// Gets SHA256 hash of this string
		/// </summary>
		/// <param name="string"></param>
		/// <param name="toBase64"></param>
		/// <returns></returns>
		public static string GetSHA256(this string @string, bool toBase64 = false)
		{
			return toBase64
				? @string.GetSHA256Hash().ToBase64()
				: @string.GetSHA256Hash().ToHexa();
		}

		/// <summary>
		/// Gets SHA384 hash of this string
		/// </summary>
		/// <param name="string"></param>
		/// <returns></returns>
		public static byte[] GetSHA384Hash(this string @string)
		{
			return @string.GetHash("SHA384");
		}

		/// <summary>
		/// Gets SHA384 hash of this string
		/// </summary>
		/// <param name="string"></param>
		/// <param name="toBase64"></param>
		/// <returns></returns>
		public static string GetSHA384(this string @string, bool toBase64 = false)
		{
			return toBase64
				? @string.GetSHA384Hash().ToBase64()
				: @string.GetSHA384Hash().ToHexa();
		}

		/// <summary>
		/// Gets SHA512 hash of this string
		/// </summary>
		/// <param name="string"></param>
		/// <returns></returns>
		public static byte[] GetSHA512Hash(this string @string)
		{
			return @string.GetHash("SHA512");
		}

		/// <summary>
		/// Gets SHA512 hash of this string
		/// </summary>
		/// <param name="string"></param>
		/// <param name="toBase64"></param>
		/// <returns></returns>
		public static string GetSHA512(this string @string, bool toBase64 = false)
		{
			return toBase64
				? @string.GetSHA512Hash().ToBase64()
				: @string.GetSHA512Hash().ToHexa();
		}

		/// <summary>
		/// Gets BLAKE hash of this string (128 bits)
		/// </summary>
		/// <param name="string"></param>
		/// <returns></returns>
		public static byte[] GetBLAKEHash(this string @string)
		{
			return @string.GetHash("BLAKE");
		}

		/// <summary>
		/// Gets BLAKE hash of this string (128 bits)
		/// </summary>
		/// <param name="string"></param>
		/// <param name="toBase64"></param>
		/// <returns></returns>
		public static string GetBLAKE(this string @string, bool toBase64 = false)
		{
			return toBase64
				? @string.GetBLAKEHash().ToBase64()
				: @string.GetBLAKEHash().ToHexa();
		}

		/// <summary>
		/// Gets BLAKE hash of this string (256 bits)
		/// </summary>
		/// <param name="string"></param>
		/// <returns></returns>
		public static byte[] GetBLAKE256Hash(this string @string)
		{
			return @string.GetHash("BLAKE256");
		}

		/// <summary>
		/// Gets BLAKE hash of this string (256 bits)
		/// </summary>
		/// <param name="string"></param>
		/// <param name="toBase64"></param>
		/// <returns></returns>
		public static string GetBLAKE256(this string @string, bool toBase64 = false)
		{
			return toBase64
				? @string.GetBLAKE256Hash().ToBase64()
				: @string.GetBLAKE256Hash().ToHexa();
		}

		/// <summary>
		/// Gets BLAKE hash of this string (384 bits)
		/// </summary>
		/// <param name="string"></param>
		/// <returns></returns>
		public static byte[] GetBLAKE384Hash(this string @string)
		{
			return @string.GetHash("BLAKE384");
		}

		/// <summary>
		/// Gets BLAKE hash of this string (384 bits)
		/// </summary>
		/// <param name="string"></param>
		/// <param name="toBase64"></param>
		/// <returns></returns>
		public static string GetBLAKE384(this string @string, bool toBase64 = false)
		{
			return toBase64
				? @string.GetBLAKE384Hash().ToBase64()
				: @string.GetBLAKE384Hash().ToHexa();
		}

		/// <summary>
		/// Gets BLAKE hash of this string (512 bits)
		/// </summary>
		/// <param name="string"></param>
		/// <returns></returns>
		public static byte[] GetBLAKE512Hash(this string @string)
		{
			return @string.GetHash("BLAKE512");
		}

		/// <summary>
		/// Gets BLAKE hash of this string (512 bits)
		/// </summary>
		/// <param name="string"></param>
		/// <param name="toBase64"></param>
		/// <returns></returns>
		public static string GetBLAKE512(this string @string, bool toBase64 = false)
		{
			return toBase64
				? @string.GetBLAKE512Hash().ToBase64()
				: @string.GetBLAKE512Hash().ToHexa();
		}

		/// <summary>
		/// Gets RIPEMD160 hash of this string (160 bits)
		/// </summary>
		/// <param name="string"></param>
		/// <returns></returns>
		public static byte[] GetRIPEMD160Hash(this string @string)
		{
			return @string.GetHash("RIPEMD160");
		}

		/// <summary>
		/// Gets RIPEMD160 hash of this string (160 bits)
		/// </summary>
		/// <param name="string"></param>
		/// <param name="toBase64"></param>
		/// <returns></returns>
		public static string GetRIPEMD160(this string @string, bool toBase64 = false)
		{
			return toBase64
				? @string.GetRIPEMD160Hash().ToBase64()
				: @string.GetRIPEMD160Hash().ToHexa();
		}
		#endregion

		#region HMAC Hash array of bytes/string
		static Dictionary<string, Func<byte[], HMAC>> HmacHashFactories = new Dictionary<string, Func<byte[], HMAC>>(StringComparer.OrdinalIgnoreCase)
		{
			{ "md5", (key) => new HMACMD5(key) },
			{ "sha1", (key) => new HMACSHA1(key) },
			{ "sha256", (key) => new HMACSHA256(key) },
			{ "sha384", (key) => new HMACSHA384(key) },
			{ "sha512", (key) => new HMACSHA512(key) },
			{ "blake", (key) => new HMACBlake2B(key, 128) },
			{ "blake128", (key) => new HMACBlake2B(key, 128) },
			{ "blake256", (key) => new HMACBlake2B(key, 256) },
			{ "blake384", (key) => new HMACBlake2B(key, 384) },
			{ "blake512", (key) => new HMACBlake2B(key, 512) },
		};

		/// <summary>
		/// Gets a HMAC hashser
		/// </summary>
		/// <param name="key"></param>
		/// <param name="mode">Mode of the hasher (md5, sha1, sha256, sha384, sha512, blake/blake128, blake256, blake384, blake512)</param>
		/// <returns></returns>
		public static HMAC GetHMACHasher(byte[] key, string mode = "SHA256")
		{
			if (!CryptoService.HmacHashFactories.TryGetValue(mode, out Func<byte[], HMAC> func))
				func = (k) => new HMACSHA256(k);
			return func(key);
		}

		/// <summary>
		/// Gets HMAC hash of this array of bytes
		/// </summary>
		/// <param name="bytes"></param>
		/// <param name="key">Keys for hashing (means salt)</param>
		/// <param name="mode">Mode of the hasher (md5, sha1, sha256, sha384, sha512, blake/blake128, blake256, blake384, blake512)</param>
		/// <returns></returns>
		public static byte[] GetHMACHash(this byte[] bytes, byte[] key, string mode = "SHA256")
		{
			if (bytes == null || bytes.Length < 1)
				throw new ArgumentException("Invalid", nameof(bytes));
			else if (key == null || key.Length < 1)
				throw new ArgumentException("Invalid", nameof(key));

			using (var hasher = CryptoService.GetHMACHasher(key, mode))
			{
				return hasher.ComputeHash(bytes);
			}
		}

		/// <summary>
		/// Gets HMAC hash of this string
		/// </summary>
		/// <param name="string"></param>
		/// <param name="key">Keys for hashing (means salt)</param>
		/// <param name="mode">Mode of the hasher (md5, sha1, sha256, sha384, sha512, blake/blake128, blake256, blake384, blake512)</param>
		/// <returns></returns>
		public static byte[] GetHMACHash(this string @string, byte[] key, string mode = "SHA256")
		{
			return string.IsNullOrWhiteSpace(@string)
				? new byte[0]
				: @string.ToBytes().GetHMACHash(key, mode);
		}

		/// <summary>
		/// Gets HMAC hash of this string
		/// </summary>
		/// <param name="string"></param>
		/// <param name="key">Keys for hashing (means salt)</param>
		/// <param name="mode">Mode of the hasher (md5, sha1, sha256, sha384, sha512, blake/blake128, blake256, blake384, blake512)</param>
		/// <returns></returns>
		public static byte[] GetHMACHash(this string @string, string key, string mode = "SHA256")
		{
			return @string.GetHMACHash((key ?? CryptoService.DefaultEncryptionKey).ToBytes(), mode);
		}

		/// <summary>
		/// Gets HMAC hash of this string
		/// </summary>
		/// <param name="string"></param>
		/// <param name="key">Keys for hashing (means salt)</param>
		/// <param name="mode">Mode of the hasher (md5, sha1, sha256, sha384, sha512, blake/blake128, blake256, blake384, blake512)</param>
		/// <param name="toHexa">true to get hexa-string, otherwise get base64-string</param>
		/// <returns></returns>
		public static string GetHMAC(this string @string, string key, string mode = null, bool toHexa = true)
		{
			var hash = @string.GetHMACHash(key, mode);
			return toHexa
				? hash.ToHexa()
				: hash.ToBase64();
		}

		/// <summary>
		/// Gets HMAC MD5 hash of this string
		/// </summary>
		/// <param name="string"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static byte[] GetHMACMD5Hash(this string @string, string key)
		{
			return @string.GetHMACHash((key ?? CryptoService.DefaultEncryptionKey).ToBytes(), "MD5");
		}

		/// <summary>
		/// Gets HMAC MD5 hash of this string
		/// </summary>
		/// <param name="string"></param>
		/// <param name="key"></param>
		/// <param name="toHexa"></param>
		/// <returns></returns>
		public static string GetHMACMD5(this string @string, string key, bool toHexa = true)
		{
			return @string.GetHMAC(key, "MD5", toHexa);
		}

		/// <summary>
		/// Gets HMAC MD5 hash of this string
		/// </summary>
		/// <param name="string"></param>
		/// <param name="toHexa"></param>
		/// <returns></returns>
		public static string GetHMACMD5(this string @string, bool toHexa = true)
		{
			return @string.GetHMACMD5(null, toHexa);
		}

		/// <summary>
		/// Gets HMAC SHA1 hash of this string
		/// </summary>
		/// <param name="string"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static byte[] GetHMACSHA1Hash(this string @string, string key)
		{
			return @string.GetHMACHash((key ?? CryptoService.DefaultEncryptionKey).ToBytes(), "SHA1");
		}

		/// <summary>
		/// Gets HMAC SHA1 hash of this string
		/// </summary>
		/// <param name="string"></param>
		/// <param name="key"></param>
		/// <param name="toHexa"></param>
		/// <returns></returns>
		public static string GetHMACSHA1(this string @string, string key, bool toHexa = true)
		{
			return @string.GetHMAC(key, "SHA1", toHexa);
		}

		/// <summary>
		/// Gets HMAC SHA1 hash of this string
		/// </summary>
		/// <param name="string"></param>
		/// <param name="toHexa"></param>
		/// <returns></returns>
		public static string GetHMACSHA1(this string @string, bool toHexa = true)
		{
			return @string.GetHMACSHA1(null, toHexa);
		}

		/// <summary>
		/// Gets HMAC SHA256 hash of this string
		/// </summary>
		/// <param name="string"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static byte[] GetHMACSHA256Hash(this string @string, string key)
		{
			return @string.GetHMACHash((key ?? CryptoService.DefaultEncryptionKey).ToBytes(), "SHA256");
		}

		/// <summary>
		/// Gets HMAC SHA256 hash of this string
		/// </summary>
		/// <param name="string"></param>
		/// <param name="key"></param>
		/// <param name="toHexa"></param>
		/// <returns></returns>
		public static string GetHMACSHA256(this string @string, string key, bool toHexa = true)
		{
			return @string.GetHMAC(key, "SHA256", toHexa);
		}

		/// <summary>
		/// Gets HMAC SHA256 hash of this string
		/// </summary>
		/// <param name="string"></param>
		/// <param name="toHexa"></param>
		/// <returns></returns>
		public static string GetHMACSHA256(this string @string, bool toHexa = true)
		{
			return @string.GetHMACSHA256(null, toHexa);
		}

		/// <summary>
		/// Gets HMAC SHA384 hash of this string
		/// </summary>
		/// <param name="string"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static byte[] GetHMACSHA384Hash(this string @string, string key)
		{
			return @string.GetHMACHash((key ?? CryptoService.DefaultEncryptionKey).ToBytes(), "SHA384");
		}

		/// <summary>
		/// Gets HMAC SHA384 hash of this string
		/// </summary>
		/// <param name="string"></param>
		/// <param name="key"></param>
		/// <param name="toHexa"></param>
		/// <returns></returns>
		public static string GetHMACSHA384(this string @string, string key, bool toHexa = true)
		{
			return @string.GetHMAC(key, "SHA384", toHexa);
		}

		/// <summary>
		/// Gets HMAC SHA384 hash of this string
		/// </summary>
		/// <param name="string"></param>
		/// <param name="toHexa"></param>
		/// <returns></returns>
		public static string GetHMACSHA384(this string @string, bool toHexa = true)
		{
			return @string.GetHMACSHA384(null, toHexa);
		}

		/// <summary>
		/// Gets HMAC SHA512 hash of this string
		/// </summary>
		/// <param name="string"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static byte[] GetHMACSHA512Hash(this string @string, string key)
		{
			return @string.GetHMACHash((key ?? CryptoService.DefaultEncryptionKey).ToBytes(), "SHA512");
		}

		/// <summary>
		/// Gets HMAC SHA512 hash of this string
		/// </summary>
		/// <param name="string"></param>
		/// <param name="key"></param>
		/// <param name="toHexa"></param>
		/// <returns></returns>
		public static string GetHMACSHA512(this string @string, string key, bool toHexa = true)
		{
			return @string.GetHMAC(key, "SHA512", toHexa);
		}

		/// <summary>
		/// Gets HMAC SHA512 hash of this string
		/// </summary>
		/// <param name="string"></param>
		/// <param name="toHexa"></param>
		/// <returns></returns>
		public static string GetHMACSHA512(this string @string, bool toHexa = true)
		{
			return @string.GetHMACSHA512(null, toHexa);
		}

		/// <summary>
		/// Gets HMAC BLAKE hash of this string (128 bits)
		/// </summary>
		/// <param name="string"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static byte[] GetHMACBLAKEHash(this string @string, string key)
		{
			return @string.GetHMACHash((key ?? CryptoService.DefaultEncryptionKey).ToBytes(), "BLAKE");
		}

		/// <summary>
		/// Gets HMAC BLAKE hash of this string (128 bits)
		/// </summary>
		/// <param name="string"></param>
		/// <param name="key"></param>
		/// <param name="toHexa"></param>
		/// <returns></returns>
		public static string GetHMACBLAKE(this string @string, string key, bool toHexa = true)
		{
			return @string.GetHMAC(key, "BLAKE", toHexa);
		}

		/// <summary>
		/// Gets HMAC BLAKE hash of this string (128 bits)
		/// </summary>
		/// <param name="string"></param>
		/// <param name="toHexa"></param>
		/// <returns></returns>
		public static string GetHMACBLAKE(this string @string, bool toHexa = true)
		{
			return @string.GetHMACBLAKE(null, toHexa);
		}

		/// <summary>
		/// Gets HMAC BLAKE hash of this string (256 bits)
		/// </summary>
		/// <param name="string"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static byte[] GetHMACBLAKE256Hash(this string @string, string key)
		{
			return @string.GetHMACHash((key ?? CryptoService.DefaultEncryptionKey).ToBytes(), "BLAKE256");
		}

		/// <summary>
		/// Gets HMAC BLAKE hash of this string (256 bits)
		/// </summary>
		/// <param name="string"></param>
		/// <param name="key"></param>
		/// <param name="toHexa"></param>
		/// <returns></returns>
		public static string GetHMACBLAKE256(this string @string, string key, bool toHexa = true)
		{
			return @string.GetHMAC(key, "BLAKE256", toHexa);
		}

		/// <summary>
		/// Gets HMAC BLAKE hash of this string (256 bits)
		/// </summary>
		/// <param name="string"></param>
		/// <param name="toHexa"></param>
		/// <returns></returns>
		public static string GetHMACBLAKE256(this string @string, bool toHexa = true)
		{
			return @string.GetHMACBLAKE256(null, toHexa);
		}

		/// <summary>
		/// Gets HMAC BLAKE hash of this string (384 bits)
		/// </summary>
		/// <param name="string"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static byte[] GetHMACBLAKE384Hash(this string @string, string key)
		{
			return @string.GetHMACHash((key ?? CryptoService.DefaultEncryptionKey).ToBytes(), "BLAKE384");
		}

		/// <summary>
		/// Gets HMAC BLAKE hash of this string (384 bits)
		/// </summary>
		/// <param name="string"></param>
		/// <param name="key"></param>
		/// <param name="toHexa"></param>
		/// <returns></returns>
		public static string GetHMACBLAKE384(this string @string, string key, bool toHexa = true)
		{
			return @string.GetHMAC(key, "BLAKE384", toHexa);
		}

		/// <summary>
		/// Gets HMAC BLAKE hash of this string (384 bits)
		/// </summary>
		/// <param name="string"></param>
		/// <param name="toHexa"></param>
		/// <returns></returns>
		public static string GetHMACBLAKE384(this string @string, bool toHexa = true)
		{
			return @string.GetHMACBLAKE384(null, toHexa);
		}

		/// <summary>
		/// Gets HMAC BLAKE hash of this string (512 bits)
		/// </summary>
		/// <param name="string"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static byte[] GetHMACBLAKE512Hash(this string @string, string key)
		{
			return @string.GetHMACHash((key ?? CryptoService.DefaultEncryptionKey).ToBytes(), "BLAKE512");
		}

		/// <summary>
		/// Gets HMAC BLAKE hash of this string (512 bits)
		/// </summary>
		/// <param name="string"></param>
		/// <param name="key"></param>
		/// <param name="toHexa"></param>
		/// <returns></returns>
		public static string GetHMACBLAKE512(this string @string, string key, bool toHexa = true)
		{
			return @string.GetHMAC(key, "BLAKE512", toHexa);
		}

		/// <summary>
		/// Gets HMAC BLAKE hash of this string (512 bits)
		/// </summary>
		/// <param name="string"></param>
		/// <param name="toHexa"></param>
		/// <returns></returns>
		public static string GetHMACBLAKE512(this string @string, bool toHexa = true)
		{
			return @string.GetHMACBLAKE512(null, toHexa);
		}
		#endregion

		#region Encryption key & Initialize vector (for working with AES)
		/// <summary>
		/// Gets the default key for encrypting/decrypting data
		/// </summary>
		public static string DefaultEncryptionKey
		{
			get
			{
				return "C804BE43-VIEApps-0B43-Core-442B-Components-B635-Service-FD0616D11B01";
			}
		}

		/// <summary>
		/// Generates a key from this string
		/// </summary>
		/// <param name="string"></param>
		/// <param name="reverse"></param>
		/// <param name="hash"></param>
		/// <param name="keySize"></param>
		/// <returns></returns>
		public static byte[] GenerateEncryptionKey(this string @string, bool reverse, bool hash, int keySize)
		{
			var passPhrase = reverse
				? @string.Reverse()
				: @string;

			var fullKey = hash
				? passPhrase.GetMD5Hash()
				: passPhrase.ToBytes();

			var maxIndex = 0;
			if (keySize > 7)
				maxIndex = keySize / 8;

			else
			{
				var sizeOfBytes = fullKey.Length;
				var bytes = 1;
				var bits = bytes * 8;
				while (bytes <= sizeOfBytes)
				{
					bits = bytes * 8;
					if (bytes < 2)
						bytes++;
					else
						bytes = bytes * 2;
				}
				maxIndex = bits / 8;
			}

			var keys = new byte[maxIndex];
			for (var index = 0; index < maxIndex; index++)
				keys[index] = fullKey[index];

			return keys;
		}

		/// <summary>
		/// Generates a key from this string (for using with AES)
		/// </summary>
		/// <param name="string"></param>
		/// <returns></returns>
		public static byte[] GenerateEncryptionKey(this string @string)
		{
			return @string.GenerateEncryptionKey(true, false, 256);
		}

		/// <summary>
		/// Generates a key from this string (for using with AES)
		/// </summary>
		/// <param name="string"></param>
		/// <returns></returns>
		public static byte[] GenerateKey(this string @string)
		{
			return @string.GenerateEncryptionKey();
		}

		/// <summary>
		/// Generates an initialization vector from this string (for using with AES)
		/// </summary>
		/// <param name="string"></param>
		/// <returns></returns>
		public static byte[] GenerateEncryptionIV(this string @string)
		{
			return @string.GenerateEncryptionKey(false, true, 128);
		}

		/// <summary>
		/// Generates an initialization vector from this string (for using with AES)
		/// </summary>
		/// <param name="string"></param>
		/// <returns></returns>
		public static byte[] GenerateInitializeVector(this string @string)
		{
			return @string.GenerateEncryptionIV();
		}
		#endregion

		#region Encrypt/Decrypt (using AES)
		/// <summary>
		/// Encrypts by specific key and initialization vector using AES
		/// </summary>
		/// <param name="data"></param>
		/// <param name="key"></param>
		/// <param name="iv"></param>
		/// <returns></returns>
		public static byte[] Encrypt(byte[] data, byte[] key = null, byte[] iv = null)
		{
			if (data == null || data.Length < 1)
				return null;

			using (var crypto = new AesCryptoServiceProvider())
			{
				using (var encryptor = crypto.CreateEncryptor(key ?? CryptoService.DefaultEncryptionKey.GenerateKey(), iv ?? CryptoService.DefaultEncryptionKey.GenerateInitializeVector()))
				{
					return encryptor.TransformFinalBlock(data, 0, data.Length);
				}
			}
		}

		/// <summary>
		/// Encrypts this string by specific key and initialization vector using AES
		/// </summary>
		/// <param name="string"></param>
		/// <param name="key"></param>
		/// <param name="iv"></param>
		/// <param name="toHexa"></param>
		/// <returns></returns>
		public static string Encrypt(this string @string, byte[] key, byte[] iv, bool toHexa = false)
		{
			return string.IsNullOrWhiteSpace(@string)
				? ""
				: toHexa
					? CryptoService.Encrypt(@string.ToBytes(), key, iv).ToHexa()
					: CryptoService.Encrypt(@string.ToBytes(), key, iv).ToBase64();
		}

		/// <summary>
		/// Encrypts this string by specific pass-phrase using AES
		/// </summary>
		/// <param name="string"></param>
		/// <param name="passPhrase"></param>
		/// <param name="toHexa"></param>
		/// <returns></returns>
		public static string Encrypt(this string @string, string passPhrase = null, bool toHexa = false)
		{
			return @string.Encrypt(passPhrase?.GenerateEncryptionKey(), passPhrase?.GenerateEncryptionIV(), toHexa);
		}

		/// <summary>
		/// Decrypts by specific key and initialization vector using AES
		/// </summary>
		/// <param name="data"></param>
		/// <param name="key"></param>
		/// <param name="iv"></param>
		/// <returns></returns>
		public static byte[] Decrypt(byte[] data, byte[] key = null, byte[] iv = null)
		{
			if (data == null || data.Length < 1)
				return null;

			using (var crypto = new AesCryptoServiceProvider())
			{
				using (var decryptor = crypto.CreateDecryptor(key ?? CryptoService.DefaultEncryptionKey.GenerateKey(), iv ?? CryptoService.DefaultEncryptionKey.GenerateInitializeVector()))
				{
					return decryptor.TransformFinalBlock(data, 0, data.Length);
				}
			}
		}

		/// <summary>
		/// Decrypts this encrypted string by specific key and initialization vector using AES
		/// </summary>
		/// <param name="string"></param>
		/// <param name="key"></param>
		/// <param name="iv"></param>
		/// <param name="isHexa"></param>
		/// <returns></returns>
		public static string Decrypt(this string @string, byte[] key, byte[] iv, bool isHexa = false)
		{
			return string.IsNullOrWhiteSpace(@string)
				? ""
				: CryptoService.Decrypt(isHexa ? @string.HexToBytes() : @string.Base64ToBytes(), key, iv).GetString();
		}

		/// <summary>
		/// Decrypts this encrypted string by specific pass-phrase using AES
		/// </summary>
		/// <param name="string"></param>
		/// <param name="passPhrase"></param>
		/// <param name="isHexa"></param>
		/// <returns></returns>
		public static string Decrypt(this string @string, string passPhrase = null, bool isHexa = false)
		{
			return @string.Decrypt(passPhrase?.GenerateKey(), passPhrase?.GenerateInitializeVector(), isHexa);
		}
		#endregion

		#region Encrypt/Decrypt (using RSA)
		/// <summary>
		/// Encrypts the data by RSA
		/// </summary>
		/// <param name="rsa"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static byte[] RSAEncrypt(RSACryptoServiceProvider rsa, byte[] data)
		{
			return rsa.Encrypt(data, false);
		}

		/// <summary>
		/// Encrypts the data by RSA
		/// </summary>
		/// <param name="rsa"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static string RSAEncrypt(RSACryptoServiceProvider rsa, string data)
		{
			return Convert.ToBase64String(CryptoService.RSAEncrypt(rsa, data.ToBytes()));
		}

		/// <summary>
		/// Encrypts the data by RSA
		/// </summary>
		/// <param name="key"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static byte[] RSAEncrypt(string key, byte[] data)
		{
			using (var rsa = CryptoService.CreateRSAInstance(key))
			{
				return CryptoService.RSAEncrypt(rsa, data);
			}
		}

		/// <summary>
		/// Encrypts the data by RSA
		/// </summary>
		/// <param name="key"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static string RSAEncrypt(string key, string data)
		{
			return Convert.ToBase64String(CryptoService.RSAEncrypt(key, data.ToBytes()));
		}

		/// <summary>
		/// Decrypts the data by RSA
		/// </summary>
		/// <param name="rsa"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static byte[] RSADecrypt(RSACryptoServiceProvider rsa, byte[] data)
		{
			return rsa.Decrypt(data, false);
		}

		/// <summary>
		/// Decrypts the data by RSA
		/// </summary>
		/// <param name="rsa"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static string RSADecrypt(RSACryptoServiceProvider rsa, string data)
		{
			return CryptoService.RSADecrypt(rsa, Convert.FromBase64String(data)).GetString();
		}

		/// <summary>
		/// Decrypts the data by RSA
		/// </summary>
		/// <param name="key"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static byte[] RSADecrypt(string key, byte[] data)
		{
			using (var rsa = CryptoService.CreateRSAInstance(key))
			{
				return CryptoService.RSADecrypt(rsa, data);
			}
		}

		/// <summary>
		/// Decrypts the data by RSA
		/// </summary>
		/// <param name="key"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static string RSADecrypt(string key, string data)
		{
			using (var rsa = CryptoService.CreateRSAInstance(key))
			{
				return CryptoService.RSADecrypt(rsa, data);
			}
		}
		#endregion

		#region Encrypt/Decrypt (extensions - using RSA)
		/// <summary>
		/// Encrypts the data by RSA
		/// </summary>
		/// <param name="rsa"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static byte[] Encrypt(this RSACryptoServiceProvider rsa, byte[] data)
		{
			return data == null || data.Length < 1
				? new byte[0]
				: rsa.Encrypt(data, false);
		}

		/// <summary>
		/// Encrypts the data by RSA
		/// </summary>
		/// <param name="rsa"></param>
		/// <param name="data"></param>
		/// <param name="toHexa"></param>
		/// <returns></returns>
		public static string Encrypt(this RSACryptoServiceProvider rsa, string data, bool toHexa = false)
		{
			return string.IsNullOrWhiteSpace(data)
				? ""
				: toHexa
					? rsa.Encrypt(data.ToBytes()).ToHexa()
					: rsa.Encrypt(data.ToBytes()).ToBase64();
		}

		/// <summary>
		/// Decrypts the data by RSA
		/// </summary>
		/// <param name="rsa"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static byte[] Decrypt(this RSACryptoServiceProvider rsa, byte[] data)
		{
			return data == null || data.Length < 1
				? new byte[0]
				: rsa.Decrypt(data, false);
		}

		/// <summary>
		/// Decrypts the data by RSA
		/// </summary>
		/// <param name="rsa"></param>
		/// <param name="data"></param>
		/// <param name="isHexa"></param>
		/// <returns></returns>
		public static string Decrypt(this RSACryptoServiceProvider rsa, string data, bool isHexa = false)
		{
			return string.IsNullOrWhiteSpace(data)
				? ""
				: isHexa
					? rsa.Decrypt(data.HexToBytes()).GetString()
					: rsa.Decrypt(data.Base64ToBytes()).GetString();
		}
		#endregion

		#region Create new instance of RSA from a specific key
		const string PEM_PRIVATE_KEY_BEGIN = "-----BEGIN RSA PRIVATE KEY-----";
		const string PEM_PRIVATE_KEY_END = "-----END RSA PRIVATE KEY-----";
		const string PEM_PUBLIC_KEY_BEGIN = "-----BEGIN PUBLIC KEY-----";
		const string PEM_PUBLIC_KEY_END = "-----END PUBLIC KEY-----";

		/// <summary>
		/// Creates an instance of RSA Algorithm
		/// </summary>
		/// <param name="key">Key for the RSA instance, might be or public key, and must be formated in XML or PEM</param>
		/// <param name="size">The size of key, default is 2048 bit length</param>
		/// <param name="name">Additional for name of key-container. Default name of key-container is 'VIEAppsRSAContainer'.
		/// Ex: if the additional name is 'Passport', then the name of the key-container will be 'VIEAppsPassportRSAContainer'.</param>
		/// <returns>An instance of RSA</returns>
		public static RSACryptoServiceProvider CreateRSAInstance(string key, int size = 2048, string name = null)
		{
			// prepare
			if (string.IsNullOrWhiteSpace(key) || (!key.StartsWith("<RSAKeyValue>") && !key.StartsWith(CryptoService.PEM_PRIVATE_KEY_BEGIN) && !key.StartsWith(CryptoService.PEM_PUBLIC_KEY_BEGIN)))
				throw new InvalidDataException("Invalid key to create new instance of RSA. Key must be formated in XML or PEM.");

			RSACryptoServiceProvider rsa = null;
			var containerName = $"VIEApps{name ?? ""}RSAContainer";

			// create RSACryptoServiceProvider instance and initialize with XML key
			if (key.StartsWith("<RSAKeyValue>"))
				try
				{
					// create new instance of RSA and store in the machine key-store (C:\ProgramData\Microsoft\Crypto\RSA\MachineKeys)
					rsa = new RSACryptoServiceProvider(size, new CspParameters(1, "Microsoft Strong Cryptographic Provider")
					{
						Flags = CspProviderFlags.UseMachineKeyStore,
						KeyContainerName = containerName
					});
					rsa.FromXmlString(key);
					rsa.PersistKeyInCsp = true;
				}
				catch (CryptographicException ex)
				{
					/* Object already exists:
					- Update settings of IIS and impersonate user: https://pwnedcode.wordpress.com/2008/11/10/fixing-cryptographicexception-%E2%80%9Cobject-already-exists%E2%80%9D/
					- Allow account of processes can modify folder C:\ProgramData\Microsoft\Crypto\RSA\MachineKeys
					 */
					if (ex.Message.Contains("Object already exists"))
						throw new CryptographicException($"Cannot create new instance of RSA. Please config your machine to allow to access to container '{containerName}' of machine key store (FAST & DANGEROUS method: change security of folder " + @"'C:\ProgramData\Microsoft\Crypto\RSA\MachineKeys' to allow account of processes can ascess and modify).", ex);
					else
						throw new CryptographicException($"Cannot create new instance of RSA in container '{containerName}' of the machine key store.", ex);
				}
				catch (Exception)
				{
					throw;
				}

			// create RSACryptoServiceProvider instance and initialize with PEM key
			else if (key.StartsWith(CryptoService.PEM_PRIVATE_KEY_BEGIN))
			{
				// prepare key
				var stringBuilder = new StringBuilder(key.Trim());
				stringBuilder.Replace(CryptoService.PEM_PRIVATE_KEY_BEGIN, "");
				stringBuilder.Replace(CryptoService.PEM_PRIVATE_KEY_END, "");
				byte[] binKey = null;
				try
				{
					binKey = Convert.FromBase64String(stringBuilder.ToString().Trim());
				}
				catch (Exception ex)
				{
					throw new InvalidDataException("Invalid PEM key to create new instance of RSA (not Base64 string).", ex);
				}

				// create new instance of RSA with key
				try
				{
					rsa = CryptoService.CreateRSAInstanceWithPrivateKey(binKey);
				}
				catch (Exception)
				{
					throw;
				}
			}

			// create RSACryptoServiceProvider instance and initialize with PEM public key
			else if (key.StartsWith(CryptoService.PEM_PUBLIC_KEY_BEGIN))
			{
				// prepare key
				var stringBuilder = new StringBuilder(key.Trim());
				stringBuilder.Replace(CryptoService.PEM_PUBLIC_KEY_BEGIN, "");
				stringBuilder.Replace(CryptoService.PEM_PUBLIC_KEY_END, "");
				byte[] binKey = null;
				try
				{
					binKey = Convert.FromBase64String(stringBuilder.ToString().Trim());
				}
				catch (Exception ex)
				{
					throw new InvalidDataException("Invalid public PEM key to create new instance of RSA (not Base64 string).", ex);
				}

				// create new instance of RSA with key
				try
				{
					rsa = CryptoService.CreateRSAInstanceWithPublicKey(binKey);
				}
				catch (Exception)
				{
					throw;
				}
			}

			// return the RSA instance
			return rsa;
		}

		// from http://www.jensign.com/opensslkey/
		static RSACryptoServiceProvider CreateRSAInstanceWithPrivateKey(byte[] key)
		{
			// set up stream to decode the asn.1 encoded RSA key
			using (var stream = new MemoryStream(key))
			{
				// wrap Memory Stream with BinaryReader for easy reading
				using (var reader = new BinaryReader(stream))
				{
					byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

					byte @byte = 0;
					ushort twoBytes = 0;
					int elems = 0;
					try
					{
						twoBytes = reader.ReadUInt16();
						if (twoBytes == 0x8130)                      // data read as little endian order (actual data order for Sequence is 30 81)
							reader.ReadByte();                           // advance 1 byte
						else if (twoBytes == 0x8230)
							reader.ReadInt16();                          // advance 2 bytes
						else
							return null;

						twoBytes = reader.ReadUInt16();
						if (twoBytes != 0x0102)                           // version number
							return null;
						@byte = reader.ReadByte();
						if (@byte != 0x00)
							return null;

						// all key components are Integer sequences
						elems = CryptoService.GetIntegerSize(reader);
						MODULUS = reader.ReadBytes(elems);

						elems = CryptoService.GetIntegerSize(reader);
						E = reader.ReadBytes(elems);

						elems = CryptoService.GetIntegerSize(reader);
						D = reader.ReadBytes(elems);

						elems = CryptoService.GetIntegerSize(reader);
						P = reader.ReadBytes(elems);

						elems = CryptoService.GetIntegerSize(reader);
						Q = reader.ReadBytes(elems);

						elems = CryptoService.GetIntegerSize(reader);
						DP = reader.ReadBytes(elems);

						elems = CryptoService.GetIntegerSize(reader);
						DQ = reader.ReadBytes(elems);

						elems = CryptoService.GetIntegerSize(reader);
						IQ = reader.ReadBytes(elems);

						// ------- create RSACryptoServiceProvider instance and initialize with public key -----
						var rsa = new RSACryptoServiceProvider()
						{
							PersistKeyInCsp = false
						};

						rsa.ImportParameters(new RSAParameters()
						{
							Modulus = MODULUS,
							Exponent = E,
							D = D,
							P = P,
							Q = Q,
							DP = DP,
							DQ = DQ,
							InverseQ = IQ
						});

						return rsa;
					}
					catch (Exception)
					{
						return null;
					}
				}
			}
		}

		static int GetIntegerSize(BinaryReader reader)
		{
			byte @byte = 0;
			byte lowByte = 0x00;
			byte highByte = 0x00;
			int count = 0;
			@byte = reader.ReadByte();
			if (@byte != 0x02)     //expect integer
				return 0;
			@byte = reader.ReadByte();

			if (@byte == 0x81)
				count = reader.ReadByte();    // data size in next byte

			else if (@byte == 0x82)
			{
				highByte = reader.ReadByte(); // data size in next 2 bytes
				lowByte = reader.ReadByte();
				byte[] modint = { lowByte, highByte, 0x00, 0x00 };
				count = BitConverter.ToInt32(modint, 0);
			}

			// we already have the data size
			else
				count = @byte;

			//remove high order zeros in data
			while (reader.ReadByte() == 0x00)
				count -= 1;

			//last ReadByte wasn't a removed zero, so back up a byte
			reader.BaseStream.Seek(-1, SeekOrigin.Current);

			return count;
		}

		// from http://www.jensign.com/opensslkey/
		static RSACryptoServiceProvider CreateRSAInstanceWithPublicKey(byte[] key)
		{
			// set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob
			using (var stream = new MemoryStream(key))
			{
				// wrap Memory Stream with BinaryReader for easy reading
				using (var reader = new BinaryReader(stream))
				{
					// encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
					byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
					byte[] seq = new byte[15];

					byte @byte = 0;
					ushort twoBytes = 0;

					try
					{
						twoBytes = reader.ReadUInt16();
						if (twoBytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
							reader.ReadByte();   //advance 1 byte
						else if (twoBytes == 0x8230)
							reader.ReadInt16();  //advance 2 bytes
						else
							return null;

						seq = reader.ReadBytes(15);      //read the Sequence OID
						if (!CryptoService.CompareByteArrays(seq, SeqOID))  //make sure Sequence for OID is correct
							return null;

						twoBytes = reader.ReadUInt16();
						if (twoBytes == 0x8103) //data read as little endian order (actual data order for Bit String is 03 81)
							reader.ReadByte();   //advance 1 byte
						else if (twoBytes == 0x8203)
							reader.ReadInt16();  //advance 2 bytes
						else
							return null;

						@byte = reader.ReadByte();
						if (@byte != 0x00)     //expect null byte next
							return null;

						twoBytes = reader.ReadUInt16();
						if (twoBytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
							reader.ReadByte();   //advance 1 byte
						else if (twoBytes == 0x8230)
							reader.ReadInt16();  //advance 2 bytes
						else
							return null;

						twoBytes = reader.ReadUInt16();
						byte lowbyte = 0x00;
						byte highbyte = 0x00;

						if (twoBytes == 0x8102) //data read as little endian order (actual data order for Integer is 02 81)
							lowbyte = reader.ReadByte(); // read next bytes which is bytes in modulus
						else if (twoBytes == 0x8202)
						{
							highbyte = reader.ReadByte();    //advance 2 bytes
							lowbyte = reader.ReadByte();
						}
						else
							return null;

						byte[] modInt = { lowbyte, highbyte, 0x00, 0x00 };   //reverse byte order since asn.1 key uses big endian order
						var modSize = BitConverter.ToInt32(modInt, 0);

						var firstByte = reader.ReadByte();
						reader.BaseStream.Seek(-1, SeekOrigin.Current);

						//if first byte (highest order) of modulus is zero, don't include it
						if (firstByte == 0x00)
						{
							reader.ReadByte();   //skip this null byte
							modSize -= 1;   //reduce modulus buffer size by 1
						}

						var modulus = reader.ReadBytes(modSize);  //read the modulus bytes
						if (reader.ReadByte() != 0x02)           //expect an Integer for the exponent data
							return null;

						var expbytes = (int)reader.ReadByte();       // should only need one byte for actual exponent data (for all useful values)
						var exponent = reader.ReadBytes(expbytes);

						// ------- create RSACryptoServiceProvider instance and initialize with public key -----
						var rsa = new RSACryptoServiceProvider()
						{
							PersistKeyInCsp = false
						};

						rsa.ImportParameters(new RSAParameters()
						{
							Modulus = modulus,
							Exponent = exponent
						});

						return rsa;
					}
					catch (Exception)
					{
						return null;
					}
				}
			}
		}

		static bool CompareByteArrays(byte[] a, byte[] b)
		{
			if (a.Length != b.Length)
				return false;

			int i = 0;
			foreach (byte c in a)
			{
				if (c != b[i])
					return false;
				i++;
			}
			return true;
		}
		#endregion

		#region Generate key pair of RSA
		/// <summary>
		/// Generates the RSA key-pairs (with 2048 bits length).
		/// </summary>
		/// <returns>
		/// Collection of strings that presents key-pairs, indexes is:
		/// - 0: key in XML format,
		/// - 1: key in XML format (encrypted by default AES encryption), 
		/// - 2: public key in XML format, 
		/// - 3: public key in XML format (encrypted by default AES encryption), 
		/// - 4: key in PEM format,
		/// - 5: key in PEM format (encrypted by default AES encryption),
		/// - 6: public key in PEM format,
		/// - 7: public key in PEM format (encrypted by default AES encryption),
		/// - 8: exponent of public key in HEX format
		/// - 9: modulus of public key in HEX format,
		/// </returns>
		public static List<string> GenerateRSAKeyPairs()
		{
			using (var rsa = new RSACryptoServiceProvider(
				2048,
				new CspParameters(1, "Microsoft Strong Cryptographic Provider")
				{
					Flags = CspProviderFlags.UseArchivableKey,
					KeyContainerName = "VIEAppsRSAContainer-" + UtilityService.NewUUID
				})
			{
				PersistKeyInCsp = false
			}
			)
			{
				// create collection of keys
				var keyPairs = new List<string>();

				// add key in XML format
				var key = rsa.ToXmlString(true);
				keyPairs.Append(new List<string>() { key, key.Encrypt() });

				// add public key in XML format
				var publicKey = rsa.ToXmlString(false);
				keyPairs.Append(new List<string>() { publicKey, publicKey.Encrypt() });

				// add key in PEM format
				key = CryptoService.ExportPrivateKeyToPEMFormat(rsa);
				keyPairs.Append(new List<string>() { key, key.Encrypt() });

				// add public key in PEM format
				key = CryptoService.ExportPublicKeyToPEMFormat(rsa);
				keyPairs.Append(new List<string>() { key, key.Encrypt() });

				// add modulus and exponent of public key in HEX format
				var xmlDoc = new System.Xml.XmlDocument();
				xmlDoc.LoadXml(publicKey);
				keyPairs.Append(new List<string>()
				{
					xmlDoc.DocumentElement.ChildNodes[0].InnerText.ToHexa(true),
					xmlDoc.DocumentElement.ChildNodes[1].InnerText.ToHexa(true)
				});

				// return the collection of keys
				return keyPairs;
			}
		}

		// -------------------------------------------------------
		// Methods to export key to PEM format - http://stackoverflow.com/questions/28406888/c-sharp-rsa-public-key-output-not-correct/28407693#28407693
		/// <summary>
		/// Exports the key of RSA to PEM format
		/// </summary>
		/// <param name="rsa">Object to export</param>
		/// <returns></returns>
		public static string ExportPrivateKeyToPEMFormat(RSACryptoServiceProvider rsa)
		{
			// check
			if (rsa.PublicOnly)
				throw new ArgumentException("CSP does not contain a key", "csp");

			using (var results = new StringWriter())
			{
				using (var stream = new MemoryStream())
				{
					var writer = new BinaryWriter(stream);
					writer.Write((byte)0x30); // SEQUENCE
					using (var innerStream = new MemoryStream())
					{
						var parameters = rsa.ExportParameters(true);
						var innerWriter = new BinaryWriter(innerStream);
						CryptoService.EncodeIntegerBigEndian(innerWriter, new byte[] { 0x00 }); // Version
						CryptoService.EncodeIntegerBigEndian(innerWriter, parameters.Modulus);
						CryptoService.EncodeIntegerBigEndian(innerWriter, parameters.Exponent);
						CryptoService.EncodeIntegerBigEndian(innerWriter, parameters.D);
						CryptoService.EncodeIntegerBigEndian(innerWriter, parameters.P);
						CryptoService.EncodeIntegerBigEndian(innerWriter, parameters.Q);
						CryptoService.EncodeIntegerBigEndian(innerWriter, parameters.DP);
						CryptoService.EncodeIntegerBigEndian(innerWriter, parameters.DQ);
						CryptoService.EncodeIntegerBigEndian(innerWriter, parameters.InverseQ);
						var length = (int)innerStream.Length;
						CryptoService.EncodeLength(writer, length);
						writer.Write(innerStream.GetBuffer(), 0, length);
					}

					// begin
					results.WriteLine(CryptoService.PEM_PRIVATE_KEY_BEGIN);

					// output as Base64 with lines chopped at 64 characters
					var base64 = Convert.ToBase64String(stream.GetBuffer(), 0, (int)stream.Length).ToCharArray();
					for (var index = 0; index < base64.Length; index += 64)
						results.WriteLine(base64, index, Math.Min(64, base64.Length - index));

					// end
					results.Write(CryptoService.PEM_PRIVATE_KEY_END);
				}

				return results.ToString();
			}
		}

		// -------------------------------------------------------
		// Methods to export public key to PEM format - http://stackoverflow.com/questions/23734792/c-sharp-export-private-public-rsa-key-from-rsacryptoserviceprovider-to-pem-strin

		/// <summary>
		/// Exports the public key of RSA to PEM format
		/// </summary>
		/// <param name="rsa">Object to export</param>
		/// <returns></returns>
		public static String ExportPublicKeyToPEMFormat(RSACryptoServiceProvider rsa)
		{
			using (var results = new StringWriter())
			{
				using (var stream = new MemoryStream())
				{
					var writer = new BinaryWriter(stream);
					writer.Write((byte)0x30); // SEQUENCE
					using (var innerStream = new MemoryStream())
					{
						var innerWriter = new BinaryWriter(innerStream);
						innerWriter.Write((byte)0x30); // SEQUENCE

						CryptoService.EncodeLength(innerWriter, 13);
						innerWriter.Write((byte)0x06); // OBJECT IDENTIFIER

						var rsaEncryptionOid = new byte[] { 0x2a, 0x86, 0x48, 0x86, 0xf7, 0x0d, 0x01, 0x01, 0x01 };
						CryptoService.EncodeLength(innerWriter, rsaEncryptionOid.Length);
						innerWriter.Write(rsaEncryptionOid);
						innerWriter.Write((byte)0x05); // NULL

						CryptoService.EncodeLength(innerWriter, 0);
						innerWriter.Write((byte)0x03); // BIT STRING

						using (var bitStringStream = new MemoryStream())
						{
							var bitStringWriter = new BinaryWriter(bitStringStream);
							bitStringWriter.Write((byte)0x00); // # of unused bits
							bitStringWriter.Write((byte)0x30); // SEQUENCE
							using (var paramsStream = new MemoryStream())
							{
								var parameters = rsa.ExportParameters(false);
								var paramsWriter = new BinaryWriter(paramsStream);
								CryptoService.EncodeIntegerBigEndian(paramsWriter, parameters.Modulus); // Modulus
								CryptoService.EncodeIntegerBigEndian(paramsWriter, parameters.Exponent); // Exponent
								var paramsLength = (int)paramsStream.Length;
								CryptoService.EncodeLength(bitStringWriter, paramsLength);
								bitStringWriter.Write(paramsStream.GetBuffer(), 0, paramsLength);
							}
							var bitStringLength = (int)bitStringStream.Length;
							CryptoService.EncodeLength(innerWriter, bitStringLength);
							innerWriter.Write(bitStringStream.GetBuffer(), 0, bitStringLength);
						}

						var length = (int)innerStream.Length;
						CryptoService.EncodeLength(writer, length);
						writer.Write(innerStream.GetBuffer(), 0, length);
					}

					// begin
					results.WriteLine(CryptoService.PEM_PUBLIC_KEY_BEGIN);

					// output as Base64 with lines chopped at 64 characters
					var base64 = Convert.ToBase64String(stream.GetBuffer(), 0, (int)stream.Length).ToCharArray();
					for (var index = 0; index < base64.Length; index += 64)
						results.WriteLine(base64, index, Math.Min(64, base64.Length - index));

					// end
					results.Write(CryptoService.PEM_PUBLIC_KEY_END);
				}

				return results.ToString();
			}
		}

		static void EncodeLength(BinaryWriter writer, int length)
		{
			// check
			if (length < 0)
				throw new ArgumentOutOfRangeException(nameof(length), "Length must be non-negative");

			// short form
			if (length < 0x80)
				writer.Write((byte)length);

			// long form
			else
			{
				var temp = length;
				var bytesRequired = 0;
				while (temp > 0)
				{
					temp >>= 8;
					bytesRequired++;
				}
				writer.Write((byte)(bytesRequired | 0x80));
				for (var index = bytesRequired - 1; index >= 0; index--)
					writer.Write((byte)(length >> (8 * index) & 0xff));
			}
		}

		static void EncodeIntegerBigEndian(BinaryWriter writer, byte[] value, bool forceUnsigned = true)
		{
			writer.Write((byte)0x02); // INTEGER
			var prefixZeros = 0;
			for (var index = 0; index < value.Length; index++)
			{
				if (value[index] != 0) break;
				prefixZeros++;
			}
			if (value.Length - prefixZeros == 0)
			{
				CryptoService.EncodeLength(writer, 1);
				writer.Write((byte)0);
			}
			else
			{
				if (forceUnsigned && value[prefixZeros] > 0x7f)
				{
					// Add a prefix zero to force unsigned if the MSB is 1
					CryptoService.EncodeLength(writer, value.Length - prefixZeros + 1);
					writer.Write((byte)0);
				}
				else
					CryptoService.EncodeLength(writer, value.Length - prefixZeros);

				for (var index = prefixZeros; index < value.Length; index++)
					writer.Write(value[index]);
			}
		}
		#endregion

	}

	#region Replacement of System.Security.Cryptography.RIPEMD160
	// darrenstarr - https://github.com/darrenstarr/RIPEMD160.net
	public class RIPEMD160 : HashAlgorithm
	{
		public new static RIPEMD160 Create()
		{
			return new RIPEMD160();
		}

		public new static RIPEMD160 Create(string hashname)
		{
			return new RIPEMD160();
		}

		static public UInt32 ReadUInt32(byte[] buffer, long offset)
		{
			return
				(Convert.ToUInt32(buffer[3 + offset]) << 24) |
				(Convert.ToUInt32(buffer[2 + offset]) << 16) |
				(Convert.ToUInt32(buffer[1 + offset]) << 8) |
				(Convert.ToUInt32(buffer[0 + offset]));
		}

		static UInt32 RotateLeft(UInt32 value, int bits)
		{
			return (value << bits) | (value >> (32 - bits));
		}

		/* the five basic functions F(), G() and H() */
		static UInt32 F(UInt32 x, UInt32 y, UInt32 z)
		{
			return x ^ y ^ z;
		}

		static UInt32 G(UInt32 x, UInt32 y, UInt32 z)
		{
			return (x & y) | (~x & z);
		}

		static UInt32 H(UInt32 x, UInt32 y, UInt32 z)
		{
			return (x | ~y) ^ z;
		}

		static UInt32 I(UInt32 x, UInt32 y, UInt32 z)
		{
			return (x & z) | (y & ~z);
		}

		static UInt32 J(UInt32 x, UInt32 y, UInt32 z)
		{
			return x ^ (y | ~z);
		}

		/* the ten basic operations FF() through III() */

		static void FF(ref UInt32 a, UInt32 b, ref UInt32 c, UInt32 d, UInt32 e, UInt32 x, int s)
		{
			a += F(b, c, d) + x;
			a = RotateLeft(a, s) + e;
			c = RotateLeft(c, 10);
		}


		static void GG(ref UInt32 a, UInt32 b, ref UInt32 c, UInt32 d, UInt32 e, UInt32 x, int s)
		{
			a += G(b, c, d) + x + (UInt32)0x5a827999;
			a = RotateLeft(a, s) + e;
			c = RotateLeft(c, 10);
		}


		static void HH(ref UInt32 a, UInt32 b, ref UInt32 c, UInt32 d, UInt32 e, UInt32 x, int s)
		{
			a += H(b, c, d) + x + (UInt32)0x6ed9eba1;
			a = RotateLeft(a, s) + e;
			c = RotateLeft(c, 10);
		}

		static void II(ref UInt32 a, UInt32 b, ref UInt32 c, UInt32 d, UInt32 e, UInt32 x, int s)
		{
			a += I(b, c, d) + x + (UInt32)0x8f1bbcdc;
			a = RotateLeft(a, s) + e;
			c = RotateLeft(c, 10);
		}

		static void JJ(ref UInt32 a, UInt32 b, ref UInt32 c, UInt32 d, UInt32 e, UInt32 x, int s)
		{
			a += J(b, c, d) + x + (UInt32)0xa953fd4e;
			a = RotateLeft(a, s) + e;
			c = RotateLeft(c, 10);
		}

		static void FFF(ref UInt32 a, UInt32 b, ref UInt32 c, UInt32 d, UInt32 e, UInt32 x, int s)
		{
			a += F(b, c, d) + x;
			a = RotateLeft(a, s) + e;
			c = RotateLeft(c, 10);
		}

		static void GGG(ref UInt32 a, UInt32 b, ref UInt32 c, UInt32 d, UInt32 e, UInt32 x, int s)
		{
			a += G(b, c, d) + x + (UInt32)0x7a6d76e9;
			a = RotateLeft(a, s) + e;
			c = RotateLeft(c, 10);
		}

		static void HHH(ref UInt32 a, UInt32 b, ref UInt32 c, UInt32 d, UInt32 e, UInt32 x, int s)
		{
			a += H(b, c, d) + x + (UInt32)0x6d703ef3;
			a = RotateLeft(a, s) + e;
			c = RotateLeft(c, 10);
		}

		static void III(ref UInt32 a, UInt32 b, ref UInt32 c, UInt32 d, UInt32 e, UInt32 x, int s)
		{
			a += I(b, c, d) + x + (UInt32)0x5c4dd124;
			a = RotateLeft(a, s) + e;
			c = RotateLeft(c, 10);
		}

		static void JJJ(ref UInt32 a, UInt32 b, ref UInt32 c, UInt32 d, UInt32 e, UInt32 x, int s)
		{
			a += J(b, c, d) + x + (UInt32)0x50a28be6;
			a = RotateLeft(a, s) + e;
			c = RotateLeft(c, 10);
		}

		/// initializes MDbuffer to "magic constants"
		static public void MDinit(ref UInt32[] MDbuf)
		{
			MDbuf[0] = (UInt32)0x67452301;
			MDbuf[1] = (UInt32)0xefcdab89;
			MDbuf[2] = (UInt32)0x98badcfe;
			MDbuf[3] = (UInt32)0x10325476;
			MDbuf[4] = (UInt32)0xc3d2e1f0;
		}

		///  the compression function.
		///  transforms MDbuf using message bytes X[0] through X[15]
		static public void compress(ref UInt32[] MDbuf, UInt32[] X)
		{
			UInt32 aa = MDbuf[0];
			UInt32 bb = MDbuf[1];
			UInt32 cc = MDbuf[2];
			UInt32 dd = MDbuf[3];
			UInt32 ee = MDbuf[4];
			UInt32 aaa = MDbuf[0];
			UInt32 bbb = MDbuf[1];
			UInt32 ccc = MDbuf[2];
			UInt32 ddd = MDbuf[3];
			UInt32 eee = MDbuf[4];

			/* round 1 */
			FF(ref aa, bb, ref cc, dd, ee, X[0], 11);
			FF(ref ee, aa, ref bb, cc, dd, X[1], 14);
			FF(ref dd, ee, ref aa, bb, cc, X[2], 15);
			FF(ref cc, dd, ref ee, aa, bb, X[3], 12);
			FF(ref bb, cc, ref dd, ee, aa, X[4], 5);
			FF(ref aa, bb, ref cc, dd, ee, X[5], 8);
			FF(ref ee, aa, ref bb, cc, dd, X[6], 7);
			FF(ref dd, ee, ref aa, bb, cc, X[7], 9);
			FF(ref cc, dd, ref ee, aa, bb, X[8], 11);
			FF(ref bb, cc, ref dd, ee, aa, X[9], 13);
			FF(ref aa, bb, ref cc, dd, ee, X[10], 14);
			FF(ref ee, aa, ref bb, cc, dd, X[11], 15);
			FF(ref dd, ee, ref aa, bb, cc, X[12], 6);
			FF(ref cc, dd, ref ee, aa, bb, X[13], 7);
			FF(ref bb, cc, ref dd, ee, aa, X[14], 9);
			FF(ref aa, bb, ref cc, dd, ee, X[15], 8);

			/* round 2 */
			GG(ref ee, aa, ref bb, cc, dd, X[7], 7);
			GG(ref dd, ee, ref aa, bb, cc, X[4], 6);
			GG(ref cc, dd, ref ee, aa, bb, X[13], 8);
			GG(ref bb, cc, ref dd, ee, aa, X[1], 13);
			GG(ref aa, bb, ref cc, dd, ee, X[10], 11);
			GG(ref ee, aa, ref bb, cc, dd, X[6], 9);
			GG(ref dd, ee, ref aa, bb, cc, X[15], 7);
			GG(ref cc, dd, ref ee, aa, bb, X[3], 15);
			GG(ref bb, cc, ref dd, ee, aa, X[12], 7);
			GG(ref aa, bb, ref cc, dd, ee, X[0], 12);
			GG(ref ee, aa, ref bb, cc, dd, X[9], 15);
			GG(ref dd, ee, ref aa, bb, cc, X[5], 9);
			GG(ref cc, dd, ref ee, aa, bb, X[2], 11);
			GG(ref bb, cc, ref dd, ee, aa, X[14], 7);
			GG(ref aa, bb, ref cc, dd, ee, X[11], 13);
			GG(ref ee, aa, ref bb, cc, dd, X[8], 12);

			/* round 3 */
			HH(ref dd, ee, ref aa, bb, cc, X[3], 11);
			HH(ref cc, dd, ref ee, aa, bb, X[10], 13);
			HH(ref bb, cc, ref dd, ee, aa, X[14], 6);
			HH(ref aa, bb, ref cc, dd, ee, X[4], 7);
			HH(ref ee, aa, ref bb, cc, dd, X[9], 14);
			HH(ref dd, ee, ref aa, bb, cc, X[15], 9);
			HH(ref cc, dd, ref ee, aa, bb, X[8], 13);
			HH(ref bb, cc, ref dd, ee, aa, X[1], 15);
			HH(ref aa, bb, ref cc, dd, ee, X[2], 14);
			HH(ref ee, aa, ref bb, cc, dd, X[7], 8);
			HH(ref dd, ee, ref aa, bb, cc, X[0], 13);
			HH(ref cc, dd, ref ee, aa, bb, X[6], 6);
			HH(ref bb, cc, ref dd, ee, aa, X[13], 5);
			HH(ref aa, bb, ref cc, dd, ee, X[11], 12);
			HH(ref ee, aa, ref bb, cc, dd, X[5], 7);
			HH(ref dd, ee, ref aa, bb, cc, X[12], 5);

			/* round 4 */
			II(ref cc, dd, ref ee, aa, bb, X[1], 11);
			II(ref bb, cc, ref dd, ee, aa, X[9], 12);
			II(ref aa, bb, ref cc, dd, ee, X[11], 14);
			II(ref ee, aa, ref bb, cc, dd, X[10], 15);
			II(ref dd, ee, ref aa, bb, cc, X[0], 14);
			II(ref cc, dd, ref ee, aa, bb, X[8], 15);
			II(ref bb, cc, ref dd, ee, aa, X[12], 9);
			II(ref aa, bb, ref cc, dd, ee, X[4], 8);
			II(ref ee, aa, ref bb, cc, dd, X[13], 9);
			II(ref dd, ee, ref aa, bb, cc, X[3], 14);
			II(ref cc, dd, ref ee, aa, bb, X[7], 5);
			II(ref bb, cc, ref dd, ee, aa, X[15], 6);
			II(ref aa, bb, ref cc, dd, ee, X[14], 8);
			II(ref ee, aa, ref bb, cc, dd, X[5], 6);
			II(ref dd, ee, ref aa, bb, cc, X[6], 5);
			II(ref cc, dd, ref ee, aa, bb, X[2], 12);

			/* round 5 */
			JJ(ref bb, cc, ref dd, ee, aa, X[4], 9);
			JJ(ref aa, bb, ref cc, dd, ee, X[0], 15);
			JJ(ref ee, aa, ref bb, cc, dd, X[5], 5);
			JJ(ref dd, ee, ref aa, bb, cc, X[9], 11);
			JJ(ref cc, dd, ref ee, aa, bb, X[7], 6);
			JJ(ref bb, cc, ref dd, ee, aa, X[12], 8);
			JJ(ref aa, bb, ref cc, dd, ee, X[2], 13);
			JJ(ref ee, aa, ref bb, cc, dd, X[10], 12);
			JJ(ref dd, ee, ref aa, bb, cc, X[14], 5);
			JJ(ref cc, dd, ref ee, aa, bb, X[1], 12);
			JJ(ref bb, cc, ref dd, ee, aa, X[3], 13);
			JJ(ref aa, bb, ref cc, dd, ee, X[8], 14);
			JJ(ref ee, aa, ref bb, cc, dd, X[11], 11);
			JJ(ref dd, ee, ref aa, bb, cc, X[6], 8);
			JJ(ref cc, dd, ref ee, aa, bb, X[15], 5);
			JJ(ref bb, cc, ref dd, ee, aa, X[13], 6);

			/* parallel round 1 */
			JJJ(ref aaa, bbb, ref ccc, ddd, eee, X[5], 8);
			JJJ(ref eee, aaa, ref bbb, ccc, ddd, X[14], 9);
			JJJ(ref ddd, eee, ref aaa, bbb, ccc, X[7], 9);
			JJJ(ref ccc, ddd, ref eee, aaa, bbb, X[0], 11);
			JJJ(ref bbb, ccc, ref ddd, eee, aaa, X[9], 13);
			JJJ(ref aaa, bbb, ref ccc, ddd, eee, X[2], 15);
			JJJ(ref eee, aaa, ref bbb, ccc, ddd, X[11], 15);
			JJJ(ref ddd, eee, ref aaa, bbb, ccc, X[4], 5);
			JJJ(ref ccc, ddd, ref eee, aaa, bbb, X[13], 7);
			JJJ(ref bbb, ccc, ref ddd, eee, aaa, X[6], 7);
			JJJ(ref aaa, bbb, ref ccc, ddd, eee, X[15], 8);
			JJJ(ref eee, aaa, ref bbb, ccc, ddd, X[8], 11);
			JJJ(ref ddd, eee, ref aaa, bbb, ccc, X[1], 14);
			JJJ(ref ccc, ddd, ref eee, aaa, bbb, X[10], 14);
			JJJ(ref bbb, ccc, ref ddd, eee, aaa, X[3], 12);
			JJJ(ref aaa, bbb, ref ccc, ddd, eee, X[12], 6);

			/* parallel round 2 */
			III(ref eee, aaa, ref bbb, ccc, ddd, X[6], 9);
			III(ref ddd, eee, ref aaa, bbb, ccc, X[11], 13);
			III(ref ccc, ddd, ref eee, aaa, bbb, X[3], 15);
			III(ref bbb, ccc, ref ddd, eee, aaa, X[7], 7);
			III(ref aaa, bbb, ref ccc, ddd, eee, X[0], 12);
			III(ref eee, aaa, ref bbb, ccc, ddd, X[13], 8);
			III(ref ddd, eee, ref aaa, bbb, ccc, X[5], 9);
			III(ref ccc, ddd, ref eee, aaa, bbb, X[10], 11);
			III(ref bbb, ccc, ref ddd, eee, aaa, X[14], 7);
			III(ref aaa, bbb, ref ccc, ddd, eee, X[15], 7);
			III(ref eee, aaa, ref bbb, ccc, ddd, X[8], 12);
			III(ref ddd, eee, ref aaa, bbb, ccc, X[12], 7);
			III(ref ccc, ddd, ref eee, aaa, bbb, X[4], 6);
			III(ref bbb, ccc, ref ddd, eee, aaa, X[9], 15);
			III(ref aaa, bbb, ref ccc, ddd, eee, X[1], 13);
			III(ref eee, aaa, ref bbb, ccc, ddd, X[2], 11);

			/* parallel round 3 */
			HHH(ref ddd, eee, ref aaa, bbb, ccc, X[15], 9);
			HHH(ref ccc, ddd, ref eee, aaa, bbb, X[5], 7);
			HHH(ref bbb, ccc, ref ddd, eee, aaa, X[1], 15);
			HHH(ref aaa, bbb, ref ccc, ddd, eee, X[3], 11);
			HHH(ref eee, aaa, ref bbb, ccc, ddd, X[7], 8);
			HHH(ref ddd, eee, ref aaa, bbb, ccc, X[14], 6);
			HHH(ref ccc, ddd, ref eee, aaa, bbb, X[6], 6);
			HHH(ref bbb, ccc, ref ddd, eee, aaa, X[9], 14);
			HHH(ref aaa, bbb, ref ccc, ddd, eee, X[11], 12);
			HHH(ref eee, aaa, ref bbb, ccc, ddd, X[8], 13);
			HHH(ref ddd, eee, ref aaa, bbb, ccc, X[12], 5);
			HHH(ref ccc, ddd, ref eee, aaa, bbb, X[2], 14);
			HHH(ref bbb, ccc, ref ddd, eee, aaa, X[10], 13);
			HHH(ref aaa, bbb, ref ccc, ddd, eee, X[0], 13);
			HHH(ref eee, aaa, ref bbb, ccc, ddd, X[4], 7);
			HHH(ref ddd, eee, ref aaa, bbb, ccc, X[13], 5);

			/* parallel round 4 */
			GGG(ref ccc, ddd, ref eee, aaa, bbb, X[8], 15);
			GGG(ref bbb, ccc, ref ddd, eee, aaa, X[6], 5);
			GGG(ref aaa, bbb, ref ccc, ddd, eee, X[4], 8);
			GGG(ref eee, aaa, ref bbb, ccc, ddd, X[1], 11);
			GGG(ref ddd, eee, ref aaa, bbb, ccc, X[3], 14);
			GGG(ref ccc, ddd, ref eee, aaa, bbb, X[11], 14);
			GGG(ref bbb, ccc, ref ddd, eee, aaa, X[15], 6);
			GGG(ref aaa, bbb, ref ccc, ddd, eee, X[0], 14);
			GGG(ref eee, aaa, ref bbb, ccc, ddd, X[5], 6);
			GGG(ref ddd, eee, ref aaa, bbb, ccc, X[12], 9);
			GGG(ref ccc, ddd, ref eee, aaa, bbb, X[2], 12);
			GGG(ref bbb, ccc, ref ddd, eee, aaa, X[13], 9);
			GGG(ref aaa, bbb, ref ccc, ddd, eee, X[9], 12);
			GGG(ref eee, aaa, ref bbb, ccc, ddd, X[7], 5);
			GGG(ref ddd, eee, ref aaa, bbb, ccc, X[10], 15);
			GGG(ref ccc, ddd, ref eee, aaa, bbb, X[14], 8);

			/* parallel round 5 */
			FFF(ref bbb, ccc, ref ddd, eee, aaa, X[12], 8);
			FFF(ref aaa, bbb, ref ccc, ddd, eee, X[15], 5);
			FFF(ref eee, aaa, ref bbb, ccc, ddd, X[10], 12);
			FFF(ref ddd, eee, ref aaa, bbb, ccc, X[4], 9);
			FFF(ref ccc, ddd, ref eee, aaa, bbb, X[1], 12);
			FFF(ref bbb, ccc, ref ddd, eee, aaa, X[5], 5);
			FFF(ref aaa, bbb, ref ccc, ddd, eee, X[8], 14);
			FFF(ref eee, aaa, ref bbb, ccc, ddd, X[7], 6);
			FFF(ref ddd, eee, ref aaa, bbb, ccc, X[6], 8);
			FFF(ref ccc, ddd, ref eee, aaa, bbb, X[2], 13);
			FFF(ref bbb, ccc, ref ddd, eee, aaa, X[13], 6);
			FFF(ref aaa, bbb, ref ccc, ddd, eee, X[14], 5);
			FFF(ref eee, aaa, ref bbb, ccc, ddd, X[0], 15);
			FFF(ref ddd, eee, ref aaa, bbb, ccc, X[3], 13);
			FFF(ref ccc, ddd, ref eee, aaa, bbb, X[9], 11);
			FFF(ref bbb, ccc, ref ddd, eee, aaa, X[11], 11);

			// combine results */
			ddd += cc + MDbuf[1];               /* final result for MDbuf[0] */
			MDbuf[1] = MDbuf[2] + dd + eee;
			MDbuf[2] = MDbuf[3] + ee + aaa;
			MDbuf[3] = MDbuf[4] + aa + bbb;
			MDbuf[4] = MDbuf[0] + bb + ccc;
			MDbuf[0] = ddd;
		}

		///  puts bytes from strptr into X and pad out; appends length 
		///  and finally, compresses the last block(s)
		///  note: length in bits == 8 * (lswlen + 2^32 mswlen).
		///  note: there are (lswlen mod 64) bytes left in strptr.
		static public void MDfinish(ref UInt32[] MDbuf, byte[] strptr, long index, UInt32 lswlen, UInt32 mswlen)
		{
			//UInt32 i;                                 /* counter       */
			var X = Enumerable.Repeat((UInt32)0, 16).ToArray();                             /* message words */


			/* put bytes from strptr into X */
			for (var i = 0; i < (lswlen & 63); i++)
			{
				/* byte i goes into word X[i div 4] at pos.  8*(i mod 4)  */
				X[i >> 2] ^= Convert.ToUInt32(strptr[i + index]) << (8 * (i & 3));
			}

			/* append the bit m_n == 1 */
			X[(lswlen >> 2) & 15] ^= (UInt32)1 << Convert.ToInt32(8 * (lswlen & 3) + 7);

			if ((lswlen & 63) > 55)
			{
				/* length goes to next block */
				compress(ref MDbuf, X);
				X = Enumerable.Repeat((UInt32)0, 16).ToArray();
			}

			/* append length in bits*/
			X[14] = lswlen << 3;
			X[15] = (lswlen >> 29) | (mswlen << 3);
			compress(ref MDbuf, X);
		}
		static int RMDsize = 160;
		UInt32[] MDbuf = new UInt32[RMDsize / 32];
		UInt32[] X = new UInt32[16];               /* current 16-word chunk        */
		byte[] UnhashedBuffer = new byte[64];
		int UnhashedBufferLength = 0;
		long HashedLength = 0;

		public RIPEMD160()
		{
			Initialize();
		}

		protected override void HashCore(byte[] array, int ibStart, int cbSize)
		{
			var index = 0;
			while (index < cbSize)
			{
				var bytesRemaining = cbSize - index;
				if (UnhashedBufferLength > 0)
				{
					if ((bytesRemaining + UnhashedBufferLength) >= (UnhashedBuffer.Length))
					{
						Array.Copy(array, ibStart + index, UnhashedBuffer, UnhashedBufferLength, (UnhashedBuffer.Length) - UnhashedBufferLength);
						index += (UnhashedBuffer.Length) - UnhashedBufferLength;
						UnhashedBufferLength = UnhashedBuffer.Length;

						for (var i = 0; i < 16; i++)
							X[i] = ReadUInt32(UnhashedBuffer, i * 4);

						compress(ref MDbuf, X);
						UnhashedBufferLength = 0;
					}
					else
					{
						Array.Copy(array, ibStart + index, UnhashedBuffer, UnhashedBufferLength, bytesRemaining);
						UnhashedBufferLength += bytesRemaining;
						index += bytesRemaining;
					}
				}
				else
				{
					if (bytesRemaining >= (UnhashedBuffer.Length))
					{
						for (var i = 0; i < 16; i++)
							X[i] = ReadUInt32(array, index + (i * 4));
						index += UnhashedBuffer.Length;

						compress(ref MDbuf, X);
					}
					else
					{
						Array.Copy(array, ibStart + index, UnhashedBuffer, 0, bytesRemaining);
						UnhashedBufferLength = bytesRemaining;
						index += bytesRemaining;
					}
				}
			}

			HashedLength += cbSize;
		}

		protected override byte[] HashFinal()
		{
			MDfinish(ref MDbuf, UnhashedBuffer, 0, Convert.ToUInt32(HashedLength), 0);

			var result = new byte[RMDsize / 8];

			for (var i = 0; i < RMDsize / 8; i += 4)
			{
				result[i] = Convert.ToByte(MDbuf[i >> 2] & 0xFF);         /* implicit cast to byte  */
				result[i + 1] = Convert.ToByte((MDbuf[i >> 2] >> 8) & 0xFF);  /*  extracts the 8 least  */
				result[i + 2] = Convert.ToByte((MDbuf[i >> 2] >> 16) & 0xFF);  /*  significant bits.     */
				result[i + 3] = Convert.ToByte((MDbuf[i >> 2] >> 24) & 0xFF);
			}

			return result;
		}

		public override void Initialize()
		{
			MDinit(ref MDbuf);
			X = Enumerable.Repeat((UInt32)0, 16).ToArray();
			HashedLength = 0;
			UnhashedBufferLength = 0;
		}
	}
	#endregion

}