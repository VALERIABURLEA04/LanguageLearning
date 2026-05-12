namespace MyWebApplication.BusinessLogic.Strategy
{
    public record LoginOutcome(string RedirectController, string RedirectAction, string WelcomeMessage);
}
