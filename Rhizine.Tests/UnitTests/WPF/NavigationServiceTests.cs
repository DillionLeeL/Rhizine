using Xunit;
using NSubstitute;
using Rhizine.WPF.Services;
using Rhizine.Core.Services.Interfaces;
using System.Windows.Controls;

namespace Rhizine.Tests.UnitTests.WPF
{
    public class NavigationServiceTests
    {
        private readonly IPageService<Page> _pageService;
        private readonly ILoggingService _loggingService;
        private readonly NavigationService _navigationService;

        public NavigationServiceTests()
        {
            _pageService = Substitute.For<IPageService<Page>>();
            _loggingService = Substitute.For<ILoggingService>();
            _navigationService = new NavigationService(_pageService, _loggingService);
        }

        [WpfFact]
        public void Initialize_ShouldSetNavigationSource()
        {
            // Arrange
            var frame = new Frame();

            // Act
            _navigationService.Initialize(frame);

            // Assert
            Assert.Equal(frame, _navigationService.NavigationSource);
        }

        [WpfFact]
        public void UnsubscribeNavigation_ShouldClearNavigationSource()
        {
            // Arrange
            var frame = new Frame();
            _navigationService.Initialize(frame);

            // Act
            _navigationService.UnsubscribeNavigation();

            // Assert
            Assert.Null(_navigationService.NavigationSource);
        }

        [WpfFact]
        public void CanGoBack_WhenFrameIsNull_ShouldReturnFalse()
        {
            // Assert
            Assert.False(_navigationService.CanGoBack);
        }

        [WpfFact]
        public void GoBack_WhenCanGoBackIsFalse_ShouldReturnFalse()
        {
            // Arrange
            var frame = new Frame();
            _navigationService.Initialize(frame);

            // Act
            var result = _navigationService.GoBack();

            // Assert
            Assert.False(result);
        }

        [WpfFact]
        public void NavigateTo_WhenPageExists_ShouldReturnTrue()
        {
            // Arrange
            var frame = new Frame();
            _navigationService.Initialize(frame);
            _pageService.GetPage("TestPage").Returns(new Page());

            // Act
            var result = _navigationService.NavigateTo("TestPage");

            // Assert
            Assert.True(result);
        }

        [WpfFact]
        public void NavigateTo_WhenPageDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            var frame = new Frame();
            _navigationService.Initialize(frame);
            _pageService.GetPage("TestPage").Returns(x => null);

            // Act
            var result = _navigationService.NavigateTo("TestPage");

            // Assert
            Assert.False(result);
        }

    }
}
