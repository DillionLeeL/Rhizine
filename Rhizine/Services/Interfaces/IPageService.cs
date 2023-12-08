using System.Windows.Controls;

namespace Rhizine.Services.Interfaces;

public interface IPageService
{
    Page GetPage(string key);
}