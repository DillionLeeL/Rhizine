
// TODO
namespace Rhizine.WPF.Services.Interfaces;

public interface IUserDataService
{
    event EventHandler<UserViewModel> UserDataUpdated;

    void Initialize();

    UserViewModel GetUser();
}
