using Xunit;
using NSubstitute;
using Rhizine.WPF.Services;
using Rhizine.Core.Services.Interfaces;
using Rhizine.WPF.ViewModels.Flyouts;

namespace Rhizine.Tests.UnitTests.WPF
{
    public class FlyoutServiceTests
    {
        private readonly ILoggingService _loggingService;
        private readonly FlyoutService _flyoutService;

        public FlyoutServiceTests()
        {
            _loggingService = Substitute.For<ILoggingService>();
            _flyoutService = new FlyoutService(_loggingService);
        }

        [Fact]
        public void RegisterFlyout_ShouldAddFlyout()
        {
            // Arrange
            var flyoutName = "TestFlyout";

            // Act
            _flyoutService.RegisterFlyout<SettingsFlyoutViewModel>(flyoutName);

            // Assert
            Assert.True(_flyoutService.TryGetFlyout(flyoutName, out var flyout));
            Assert.IsType<SettingsFlyoutViewModel>(flyout);
        }

        [Fact]
        public void OpenFlyout_ShouldOpenFlyout()
        {
            // Arrange
            var flyoutName = "TestFlyout";
            _flyoutService.RegisterFlyout<SettingsFlyoutViewModel>(flyoutName);

            // Act
            _flyoutService.OpenFlyout(flyoutName);

            // Assert
            Assert.True(_flyoutService.TryGetFlyout(flyoutName, out var flyout));
            Assert.True(flyout.IsOpen);
        }

        [Fact]
        public void CloseFlyout_ShouldCloseFlyout()
        {
            // Arrange
            var flyoutName = "TestFlyout";
            _flyoutService.RegisterFlyout<SettingsFlyoutViewModel>(flyoutName);
            _flyoutService.OpenFlyout(flyoutName);

            // Act
            _flyoutService.CloseFlyout(flyoutName);

            // Assert
            Assert.True(_flyoutService.TryGetFlyout(flyoutName, out var flyout));
            Assert.False(flyout.IsOpen);
        }

        [Fact]
        public void RegisterFlyout_WithViewModel_ShouldAddFlyout()
        {
            // Arrange
            var flyoutName = "TestFlyoutWithViewModel";
            var viewModel = new SettingsFlyoutViewModel();

            // Act
            _flyoutService.RegisterFlyout(flyoutName, viewModel);

            // Assert
            Assert.True(_flyoutService.TryGetFlyout(flyoutName, out var flyout));
            Assert.Same(viewModel, flyout);
        }

        [Fact]
        public void RegisterFlyout_WithParameters_ShouldAddFlyout()
        {
            // Arrange
            var flyoutName = "TestFlyoutWithParameters";
            var parameters = new object[] { /* parameters for the constructor of SettingsFlyoutViewModel */ };

            // Act
            _flyoutService.RegisterFlyout<SettingsFlyoutViewModel>(flyoutName, parameters);

            // Assert
            Assert.True(_flyoutService.TryGetFlyout(flyoutName, out var flyout));
            Assert.IsType<SettingsFlyoutViewModel>(flyout);
        }

        [Fact]
        public void TryGetFlyout_NonExistingFlyout_ShouldReturnFalse()
        {
            // Arrange
            var flyoutName = "NonExistingFlyout";

            // Act
            var result = _flyoutService.TryGetFlyout(flyoutName, out var flyout);

            // Assert
            Assert.False(result);
            Assert.Null(flyout);
        }
    }
}
