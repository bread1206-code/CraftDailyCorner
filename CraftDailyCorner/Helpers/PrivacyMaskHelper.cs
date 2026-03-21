namespace CraftDailyCorner.Helpers
{
    public static class PrivacyMaskHelper
    {
        public static string MaskEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                return "（未提供）";

            var parts = email.Split('@');
            var name = parts[0];
            var domain = parts[1];

            if (string.IsNullOrWhiteSpace(name))
                return $"***@{domain}";

            if (name.Length <= 2)
                return $"{name[0]}***@{domain}";

            return $"{name.Substring(0, 2)}***@{domain}";
        }

        public static string MaskPhone(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return "（未提供）";

            var trimmed = phone.Trim();

            if (trimmed.Length <= 4)
                return "***";

            return $"{trimmed.Substring(0, 2)}***{trimmed.Substring(trimmed.Length - 2)}";
        }
    }
}
