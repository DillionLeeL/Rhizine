namespace Rhizine.Services.Interfaces;

public interface IActivationHandler
{
    bool CanHandle();

    Task HandleAsync();
}