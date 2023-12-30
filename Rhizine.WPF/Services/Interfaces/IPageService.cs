using System.Windows.Controls;

namespace Rhizine.WPF.Services.Interfaces;

public interface IPageService
{
    Page GetPage(string key);
}