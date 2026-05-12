namespace MyWebApplication.BusinessLogic.Strategy
{
    public interface IPostLoginStrategy
    {
        LoginOutcome Execute(string userName);
    }
}
