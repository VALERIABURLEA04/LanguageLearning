using MyWebApplication.BusinessLogic.Core.Dtos;
using MyWebApplication.BusinessLogic.Factories;
using MyWebApplication.BusinessLogic.Interfaces;

namespace MyWebApplication.BusinessLogic.Facade
{
    /// <summary>
    /// Facade hiding the multi-step auth flow:
    /// store lookup, password verify/hash, JWT issuance, post-login strategy.
    /// </summary>
    public class AuthFacade
    {
        private readonly IUserStore _users;
        private readonly IPasswordHasher _hasher;
        private readonly ITokenIssuer _tokens;
        private readonly PostLoginStrategyFactory _postLogin;

        public AuthFacade(
            IUserStore users,
            IPasswordHasher hasher,
            ITokenIssuer tokens,
            PostLoginStrategyFactory postLogin)
        {
            _users = users;
            _hasher = hasher;
            _tokens = tokens;
            _postLogin = postLogin;
        }

        public AuthResult Login(string email, string password)
        {
            var user = _users.FindByEmail(email ?? "");
            if (user == null || !_hasher.Verify(password ?? "", user.PasswordHash))
                return new AuthResult(false, null, null, null, "Invalid email or password.");

            return BuildSuccess(user);
        }

        public AuthResult Register(string name, string email, string password, bool isAdmin = false)
        {
            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
                return new AuthResult(false, null, null, null, "All fields are required.");

            if (_users.ExistsByEmail(email))
                return new AuthResult(false, null, null, null, "An account with this email already exists.");

            var user = _users.Create(name.Trim(), email.Trim(), _hasher.Hash(password), isAdmin);
            return BuildSuccess(user);
        }

        private AuthResult BuildSuccess(UserDto user)
        {
            var token = _tokens.Issue(user);
            var outcome = _postLogin.Create(user.IsAdmin).Execute(user.Name);
            return new AuthResult(true, user, token, outcome, null);
        }
    }
}
