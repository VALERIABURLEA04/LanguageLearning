namespace MyWebApplication.BusinessLogic.Strategy
{
    public class AdminPostLoginStrategy : IPostLoginStrategy
    {
        public LoginOutcome Execute(string userName) =>
            new("Admin", "Index", $"Welcome back, {userName}. Admin tools loaded.");
    }
}
