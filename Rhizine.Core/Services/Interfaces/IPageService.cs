using CommunityToolkit.Mvvm.ComponentModel;

namespace Rhizine.Core.Services.Interfaces;

public interface IPageService<TPage>
{
    TPage GetPage(string key);

    public void Register<VM, V>()
        where VM : ObservableObject
        where V : TPage;
}