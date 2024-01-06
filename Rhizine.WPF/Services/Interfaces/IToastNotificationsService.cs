using Windows.UI.Notifications;

namespace Rhizine.WPF.Services.Interfaces;

public interface IToastNotificationsService
{
    public abstract void ShowToastNotification(ToastNotification toastNotification);

    public abstract void ShowToastNotificationSample();
}