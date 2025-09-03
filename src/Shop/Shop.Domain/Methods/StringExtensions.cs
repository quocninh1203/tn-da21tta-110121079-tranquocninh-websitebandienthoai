using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Shop.Domain.Methods
{
    public static class StringExtensions
    {
        public static string ToSlug(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            // Chuyển Unicode có dấu thành không dấu
            string normalized = input.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (char c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }
            string noDiacritics = sb.ToString().Normalize(NormalizationForm.FormC);

            // Loại bỏ ký tự không hợp lệ và thay khoảng trắng bằng dấu gạch ngang
            string slug = Regex.Replace(noDiacritics, @"[^a-zA-Z0-9\s-]", "");
            slug = Regex.Replace(slug, @"\s+", "-").Trim('-').ToLower();

            return slug;
        }
    }
}
