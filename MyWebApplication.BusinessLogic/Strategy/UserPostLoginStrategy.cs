namespace MyWebApplication.BusinessLogic.Strategy
{
    public class UserPostLoginStrategy : IPostLoginStrategy
    {
        public LoginOutcome Execute(string userName) =>
            new("Home", "Index", $"Welcome back, {userName}!");
    }
}
