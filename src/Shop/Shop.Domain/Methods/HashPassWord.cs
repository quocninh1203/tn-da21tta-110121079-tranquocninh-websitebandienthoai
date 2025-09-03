using Microsoft.AspNetCore.Identity;

namespace Shop.Domain.Methods
{
    public class HashPassWord
    {
        private readonly PasswordHasher<object> _passwordHasher;
        public HashPassWord()
        {
            _passwordHasher = new PasswordHasher<object>();
        }

        // Băm mật khẩu
        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null, password);
        }

        // Xác thực mật khẩu
        public bool VerifyPassword(string hashedPassword, string password)
        {
            var result = _passwordHasher.VerifyHashedPassword(null, hashedPassword, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
