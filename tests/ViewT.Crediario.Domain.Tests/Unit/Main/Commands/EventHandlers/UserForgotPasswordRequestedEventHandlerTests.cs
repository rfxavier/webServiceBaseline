using AutoMoq;
using ViewT.Crediario.Domain.Core.DomainNotification.Events;
using ViewT.Crediario.Domain.Main.Events;

namespace ViewT.Crediario.Domain.Tests.Unit.Main.Commands.EventHandlers
{
    public class UserForgotPasswordRequestedEventHandlerTests
    {
        public readonly AutoMoqer _mocker;

        private UserForgotPasswordRequestedEvent _eventUserForgotPasswordRequested;

        private const string ValidUserName = "login@domain.com";
        private const string ValidPassword = "1234";

        public UserForgotPasswordRequestedEventHandlerTests()
        {
            DomainEvent.ClearCallbacks();

            _mocker = new AutoMoqer();
        }

    }
}