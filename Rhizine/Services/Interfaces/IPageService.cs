using System.Windows.Controls;

namespace Rhizine.Services.Interfaces;

public interface IPageService
{
    Type GetPageType(string key);

    Page GetPage(string key);
}