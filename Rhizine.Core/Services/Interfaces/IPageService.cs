using System.Windows.Controls;

namespace Rhizine.Core.Services.Interfaces;

public interface IPageService
{
    Page GetPage(string key);
}