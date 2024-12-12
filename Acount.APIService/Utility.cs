using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Text;
using System.Security.Cryptography;


namespace Acount.APIService
{
	public class CommonUtility
	{
		public static DateTime GetDateTime(DateTime date, string TimeZone)
		{
			return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(date, TimeZone);
		}
		public static DateTime ConvertUtcToTimeZone(DateTime utcDateTime, string timeZoneId)
		{
			TimeZoneInfo targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
			return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, targetTimeZone);
		}
		public static string ConvertEnumToString(string resultCode)
		{
			return System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(resultCode.ToString().Replace("_", " ").ToLower().Substring(1));
		}
		public static string GetSHA1Hash(string input)
		{
			using var sha1 = SHA1.Create();
			byte[] inputBytes = Encoding.UTF8.GetBytes((string)input);
			return Convert.ToHexString(sha1.ComputeHash(inputBytes)).ToLower();
		}
		public static long ToUnixTime(DateTime date, string timezone = "UTC")
		{
			TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezone);
			DateTime convertedDateTime = TimeZoneInfo.ConvertTimeToUtc(date, timeZoneInfo);
			DateTime UNIXTIME_ZERO_POINT = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return (long)convertedDateTime.Subtract(UNIXTIME_ZERO_POINT).TotalSeconds;
		}

		#region Conversion Json Objects

		public static dynamic ConvertJsonToObject(dynamic data)
		{
			return JsonConvert.DeserializeObject(data);
		}

		public static T ConvertFromDynamicObjectToJson<T>(dynamic data)
		{
			return JsonConvert.SerializeObject(data);
		}

		public static T ConvertFromDynamicObject<T>(dynamic data)
		{
			return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Newtonsoft.Json.JsonConvert.SerializeObject(data));
		}

		public static T SpecificConvertFromDynamicObject<T>(dynamic data)
		{
			return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Newtonsoft.Json.JsonConvert.SerializeObject(data));
		}

		public static object ConvertJsonToDynamicObject(object requestObject)
		{
			switch (requestObject)
			{
				case JObject jObject: // objects become Dictionary<string,object>
					return ((IEnumerable<KeyValuePair<string, JToken>>)jObject).ToDictionary(j => j.Key, j => ConvertJsonToDynamicObject(j.Value));
				case JArray jArray: // arrays become List<object>
					return jArray.Select(ConvertJsonToDynamicObject).ToList();
				case JValue jValue: // values just become the value
					return jValue.Value;
				default: // don't know what to do here
					throw new Exception($"Unsupported type: {requestObject.GetType()}");
			}
		}
		public static object ConvertJsonStringtoObject(object requestObject)
		{
			return ConvertJsonToDynamicObject(ConvertJsonToObject(Convert.ToString(requestObject)));

		}

		#endregion

		#region Conversion Hex values
		public static string ConvertStringToHex(String strInput)
		{
			Byte[] stringBytes = Encoding.Unicode.GetBytes(strInput);
			StringBuilder sbBytes = new(stringBytes.Length * 2);
			foreach (byte b in stringBytes)
				sbBytes.AppendFormat("{0:X2}", b);
			return sbBytes.ToString();
		}
		public static string ConvertHexToString(String hexInput)
		{
			int numberChars = hexInput.Length;
			byte[] bytes = new byte[numberChars / 2];
			for (int i = 0; i < numberChars; i += 2)
				bytes[i / 2] = Convert.ToByte(hexInput.Substring(i, 2), 16);
			return Encoding.Unicode.GetString(bytes);
		}
		#endregion

		#region Base64 Encode/Decode
		public static string Base64Encode(string plainText)
		{
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			return System.Convert.ToBase64String(plainTextBytes);
		}

		public static string Base64Decode(string base64EncodedData)
		{
			var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
			return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
		}

		public static Byte[] ConvertBase64toByteArray(string basestring)
		{
			return System.Convert.FromBase64String(basestring);
		}
		public static string ConvertBytetobase64(byte[] bytea)
		{
			if (bytea != null)
				return Convert.ToBase64String(bytea, 0, bytea.Length);
			else return null;
		}
		#endregion

		//Generate OTP
		public static string GenerateOTP()
		{
			Random generator = new Random();
			return generator.Next(0, 1000000).ToString("D6");
		}
	}

	public static class EnumExtensions
	{
		public static string GetDescription<T>(this T enumValue)
			where T : struct, IConvertible
		{
			if (!typeof(T).IsEnum)
				return null;

			var description = enumValue.ToString();
			var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

			if (fieldInfo != null)
			{
				var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
				if (attrs != null && attrs.Length > 0)
				{
					description = ((DescriptionAttribute)attrs[0]).Description;
				}
			}

			return description.ToUpper();
		}
	}

}
