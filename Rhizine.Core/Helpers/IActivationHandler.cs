namespace Rhizine.Core.Helpers;

public interface IActivationHandler
{
    bool CanHandle(object args);

    Task HandleAsync(object args);
}