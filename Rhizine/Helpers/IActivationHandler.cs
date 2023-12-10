namespace Rhizine.Helpers;

public interface IActivationHandler
{
    bool CanHandle();

    Task HandleAsync();
}