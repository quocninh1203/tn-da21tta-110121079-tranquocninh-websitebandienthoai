namespace Shop.Domain.Methods
{
    public static class GenerateOtpCode
    {
        public static string GenerateOtp()
        {
            const int length = 6;
            const string chars = "0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                                        .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
