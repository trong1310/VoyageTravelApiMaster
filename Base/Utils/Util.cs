
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace TravelMasterApi.Base.Utils
{
	public static class Util
	{
		public static string ToDescriptionString<T>(this T source)
		{
			FieldInfo fi = source.GetType().GetField(source.ToString());

			DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
				typeof(DescriptionAttribute), false);

			if (attributes != null && attributes.Length > 0) return attributes[0].Description;
			else return source.ToString();
		}

		public static T GetEnumFromInt<T>(int value) where T : Enum
		{
			if (Enum.IsDefined(typeof(T), value))
			{
				return (T)Enum.ToObject(typeof(T), value);
			}
			throw new ArgumentException($"Value {value} is not valid for enum {typeof(T).Name}");
		}

		public static string GenerateCode()
		{
			StringBuilder builder = new StringBuilder();
			Enumerable
			   .Range(65, 26)
				.Select(e => ((char)e).ToString())
				.Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
				.Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
				.OrderBy(e => Guid.NewGuid())
				.Take(15)
				.ToList().ForEach(e => builder.Append(e));
			string id = builder.ToString();
			return id;
		}
        public static string GenerateRandomNumber()
        {
            StringBuilder builder = new StringBuilder();
            Enumerable
                .Range(0, 10)
                .Select(e => e.ToString())
                .OrderBy(e => Guid.NewGuid())
                .Take(3)
                .ToList().ForEach(e => builder.Append(e));
            string id = builder.ToString();
            return id;
        }
        public static string DecimalToString(this decimal d)
		{
			return d.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);
		}

		public static string ComputeMD5Hash(string input)
		{
			using (MD5 md5 = MD5.Create())
			{
				byte[] inputBytes = Encoding.UTF8.GetBytes(input);
				byte[] hashBytes = md5.ComputeHash(inputBytes);
				StringBuilder sb = new StringBuilder();
				foreach (byte b in hashBytes)
				{
					sb.Append(b.ToString("x2"));
				}
				return sb.ToString();
			}
		}
        public static string GenerateSlug(string name)
        {
            // chuẩn hóa để tách dấu
            string normalizedString = name.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(item) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(item);
                }
            }

            string diacritics = stringBuilder.ToString().Normalize(NormalizationForm.FormD);

            // xử lý riêng chữ đ/Đ
            diacritics = diacritics.Replace('đ', 'd').Replace('Đ', 'd');

            // bỏ ký tự đặc biệt, chỉ giữ a-z A-Z 0-9, khoảng trắng, dấu -
            string specialCharacters = Regex.Replace(diacritics, @"[^a-zA-Z0-9\s-]", string.Empty);

            // thay khoảng trắng bằng dấu -
            string slug = Regex.Replace(specialCharacters, @"\s+", "-").Trim();

            return slug.ToLowerInvariant();
        }
    }
}
