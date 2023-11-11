using System.Windows.Controls;

namespace Rhizine.Contracts.Services;

public interface IPageService
{
    Type GetPageType(string key);

    Page GetPage(string key);
}
