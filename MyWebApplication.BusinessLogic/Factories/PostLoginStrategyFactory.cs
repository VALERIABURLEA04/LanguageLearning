using MyWebApplication.BusinessLogic.Strategy;

namespace MyWebApplication.BusinessLogic.Factories
{
    public class PostLoginStrategyFactory
    {
        public IPostLoginStrategy Create(bool isAdmin) =>
            isAdmin ? new AdminPostLoginStrategy() : new UserPostLoginStrategy();
    }
}
