using AutoMoq;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using ViewT.Crediario.Domain.Core.Commands;
using ViewT.Crediario.Domain.Core.DomainNotification.Events;
using ViewT.Crediario.Domain.Main.Commands.Handlers;
using ViewT.Crediario.Domain.Main.Commands.Inputs;
using ViewT.Crediario.Domain.Main.Commands.Results;
using ViewT.Crediario.Domain.Main.Entities;
using ViewT.Crediario.Domain.Main.Enums;
using ViewT.Crediario.Domain.Main.Interfaces;
using ViewT.Crediario.Domain.Tests.Unit.Main.Commands.Builders;
using ViewT.Crediario.Domain.Tests.Unit.Main.Entities.Builders;
using Xunit;

namespace ViewT.Crediario.Domain.Tests.Unit.Main.Commands.Handlers
{
    public class UserAuthenticateCommandHandlerTests
    {
        private readonly AutoMoqer _mocker;

        private readonly IList<DomainNotification> _notifications = new List<DomainNotification>();

        private const string ValidUserName = "login@domain.com";
        private const string ValidPassword = "1234";

        public UserAuthenticateCommandHandlerTests()
        {
            DomainEvent.ClearCallbacks();

            _mocker = new AutoMoqer();

        }

        [Fact(DisplayName = "UserAuthenticateCommandHandler Handle 0 invalid required")]
        [Trait("Category", "UserAuthenticateCommandHandler")]
        public void
            UserAuthenticateCommand_WithRequiredPropertiesNotInformed_Handle_ShouldReturnAllErrors()
        {
            //Arrange
            DomainEvent.Register<DomainNotification>(dn => _notifications.Add(dn));

            _mocker.GetMock<IValidationService>()
                .Setup(v => v.Validate(It.IsAny<ICommand>()))
                .Returns(() => true);

            var invalidCommand = new UserAuthenticateCommandBuilder();

            var handler = _mocker.Resolve<UserCommandHandler>();

            //Act
            handler.Handle(invalidCommand);

            //Assert
            _notifications.Should()
                .NotBeEmpty()
                .And.Contain(n => n.Value == Domain.Main.Resources.Messages.UserAuthenticateUserRequired)
                .And.Contain(n => n.Value == Domain.Main.Resources.Messages.UserAuthenticatePasswordRequired)
                .And.HaveCount(2);

            _mocker.GetMock<IValidationService>().Verify(x => x.Validate(It.IsAny<ICommand>()), Times.Once());
        }

        [Fact(DisplayName = "UserAuthenticateCommandHandler Handle 1 invalid from repository - unauthorized Person")]
        [Trait("Category", "UserAuthenticateCommandHandler")]
        public void
            UserAuthenticateCommand_FromRepositoriesUnauthorizedPerson_Handle_ShouldReturnAllErrors()
        {
            //Arrange
            DomainEvent.Register<DomainNotification>(dn => _notifications.Add(dn));

            _mocker.GetMock<IValidationService>()
                .Setup(v => v.Validate(It.IsAny<ICommand>()))
                .Returns(() => true);

            var command = new UserAuthenticateCommandBuilder()
                .WithUser(ValidUserName)
                .WithPassword(ValidPassword);

            _mocker.GetMock<IPersonRepository>()
                .Setup(u => u.GetByEmailAndPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => null);

            var handler = _mocker.Resolve<UserCommandHandler>();

            //Act
            handler.Handle(command);

            //Assert
            _notifications.Should()
                .NotBeEmpty()
                .And.Contain(n => n.Value == Domain.Main.Resources.Messages.UserAuthenticateLoginFailed)
                .And.HaveCount(1);

            _mocker.GetMock<IPersonRepository>().Verify(x => x.GetByEmailAndPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
            _mocker.GetMock<IPasswordService>().Verify(x => x.Encrypt(It.IsAny<string>()), Times.Once());
        }


        [Fact(DisplayName = "UserAuthenticateCommandHandler Handle 2 invalid from repository - existing and inactive Person")]
        [Trait("Category", "UserAuthenticateCommandHandler")]
        public void
            UserAuthenticateCommand_FromRepositoriesExistingInactivePerson_Handle_ShouldReturnAllErrors()
        {
            //Arrange
            DomainEvent.Register<DomainNotification>(dn => _notifications.Add(dn));

            _mocker.GetMock<IValidationService>()
                .Setup(v => v.Validate(It.IsAny<ICommand>()))
                .Returns(() => true);

            var command = new UserAuthenticateCommandBuilder()
                .WithUser(ValidUserName)
                .WithPassword(ValidPassword);

            _mocker.GetMock<IPersonRepository>()
                .Setup(u => u.GetByEmailAndPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => new PersonBuilder().WithPersonUserStatus(PersonUserStatus.Inactive));

            var handler = _mocker.Resolve<UserCommandHandler>();

            //Act
            handler.Handle(command);

            //Assert
            _notifications.Should()
                .NotBeEmpty()
                .And.HaveCount(1)
                .And.Contain(n => n.Value == Domain.Main.Resources.Messages.UserAuthenticateUserIsInactive);
        }

        #region .: Comment :.
        //[Fact(DisplayName = "UserAuthenticateCommandHandler Handle new Device")]
        //[Trait("Category", "UserAuthenticateCommandHandler")]
        //public void
        //    UserAuthenticateCommand_WithNewDevice_Handle_ShouldAddNewDevice()
        //{
        //    //Arrange
        //    DomainEvent.Register<DomainNotification>(dn => _notifications.Add(dn));

        //    var command = new UserAuthenticateCommandBuilder()
        //        .WithUser(ValidUserName)
        //        .WithPassword(ValidPassword);

        //    Person person = null;
        //    Device device = null;

        //    _mocker.GetMock<IPersonRepository>()
        //        .Setup(u => u.GetByEmailAndPassword(It.IsAny<string>(), It.IsAny<string>()))
        //        .Callback(() =>
        //        {
        //            person = new PersonBuilder()
        //                .WithPersonId(Guid.NewGuid());
        //        })
        //        .Returns(() => person);

        //    _mocker.GetMock<IDeviceRepository>()
        //        .Setup(d => d.GetDeviceByPersonIncludingPerson(It.IsAny<Person>()))
        //        .Returns(() => null);


        //    _mocker.GetMock<IDeviceRepository>()
        //        .Setup(u => u.Add(It.IsAny<Device>()))
        //        .Callback((Device d) =>
        //        {
        //            device = new DeviceBuilder()
        //                .WithDeviceId(d.DeviceId)
        //                .WithPerson(d.Person);
        //        })
        //        .Returns(() => device);


        //    var handler = _mocker.Resolve<UserCommandHandler>();

        //    //Act
        //    handler.Handle(command);

        //    //Assert
        //    _notifications.Should()
        //        .BeEmpty();

        //    device.Should().NotBeNull();
        //    device.DeviceId.Should().NotBe(Guid.Empty);
        //    device.Person.Should().Be(person);

        //    _mocker.GetMock<IPersonRepository>().Verify(x => x.GetByEmailAndPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        //    _mocker.GetMock<IDeviceRepository>().Verify(x => x.Add(It.IsAny<Device>()), Times.Once());
        //}


        //[Fact(DisplayName = "UserAuthenticateCommandHandler Handle existing Device disable old")]
        //[Trait("Category", "UserAuthenticateCommandHandler")]
        //public void
        //    UserAuthenticateCommand_WithExistingDevice_RequestComingFromNewDevice_Handle_ShouldAddNewDevice_AndDisableOldDevice()
        //{
        //    //Arrange
        //    DomainEvent.Register<DomainNotification>(dn => _notifications.Add(dn));

        //    var command = new UserAuthenticateCommandBuilder()
        //        .WithUser(ValidUserName)
        //        .WithPassword(ValidPassword)
        //        .WithIdentification(Guid.NewGuid().ToString());

        //    Person person = null;
        //    Device oldDevice = null;
        //    Device newDevice = null;

        //    _mocker.GetMock<IPersonRepository>()
        //        .Setup(u => u.GetByEmailAndPassword(It.IsAny<string>(), It.IsAny<string>()))
        //        .Callback(() =>
        //        {
        //            person = new PersonBuilder()
        //                .WithPersonId(Guid.NewGuid());
        //        })
        //        .Returns(() => person);

        //    _mocker.GetMock<IDeviceRepository>()
        //        .Setup(d => d.GetDeviceByPersonIncludingPerson(It.IsAny<Person>()))
        //        .Returns(() => new DeviceBuilder().WithIdentification(Guid.NewGuid().ToString()));

        //    _mocker.GetMock<IDeviceRepository>()
        //        .Setup(u => u.Update(It.IsAny<Device>()))
        //        .Callback((Device d) =>
        //        {
        //            oldDevice = new DeviceBuilder()
        //                .WithDeviceStatus(d.DeviceStatus)
        //                .WithActive(d.Active);
        //        })
        //        .Returns(() => oldDevice);

        //    _mocker.GetMock<IDeviceRepository>()
        //        .Setup(u => u.Add(It.IsAny<Device>()))
        //        .Callback((Device d) =>
        //        {
        //            newDevice = new DeviceBuilder()
        //                .WithDeviceId(d.DeviceId)
        //                .WithPerson(d.Person);
        //        })
        //        .Returns(() => newDevice);


        //    var handler = _mocker.Resolve<UserCommandHandler>();

        //    //Act
        //    handler.Handle(command);

        //    //Assert
        //    _notifications.Should()
        //        .BeEmpty();

        //    oldDevice.DeviceStatus.Should().Be(DeviceStatus.Inactive);
        //    oldDevice.Active.Should().BeFalse();

        //    newDevice.Should().NotBeNull();
        //    newDevice.DeviceId.Should().NotBe(Guid.Empty);
        //    newDevice.Person.Should().Be(person);

        //    _mocker.GetMock<IPersonRepository>().Verify(x => x.GetByEmailAndPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        //    _mocker.GetMock<IDeviceRepository>().Verify(x => x.Update(It.IsAny<Device>()), Times.Once());
        //    _mocker.GetMock<IDeviceRepository>().Verify(x => x.Add(It.IsAny<Device>()), Times.Once());
        //}


        //[Fact(DisplayName = "UserAuthenticateCommandHandler Handle existing and same Device")]
        //[Trait("Category", "UserAuthenticateCommandHandler")]
        //public void
        //    UserAuthenticateCommand_WithExistingDevice_RequestComingFromSameDevice_Handle_ShouldNotAddNewDevice()
        //{
        //    //Arrange
        //    DomainEvent.Register<DomainNotification>(dn => _notifications.Add(dn));

        //    var identification = Guid.NewGuid().ToString();

        //    var command = new UserAuthenticateCommandBuilder()
        //        .WithUser(ValidUserName)
        //        .WithPassword(ValidPassword)
        //        .WithIdentification(identification);

        //    Person person = null;

        //    _mocker.GetMock<IPersonRepository>()
        //        .Setup(u => u.GetByEmailAndPassword(It.IsAny<string>(), It.IsAny<string>()))
        //        .Callback(() =>
        //        {
        //            person = new PersonBuilder()
        //                .WithPersonId(Guid.NewGuid());
        //        })
        //        .Returns(() => person);

        //    _mocker.GetMock<IDeviceRepository>()
        //        .Setup(d => d.GetDeviceByPersonIncludingPerson(It.IsAny<Person>()))
        //        .Returns(() => new DeviceBuilder().WithIdentification(identification));


        //    var handler = _mocker.Resolve<UserCommandHandler>();

        //    //Act
        //    handler.Handle(command);

        //    //Assert
        //    _notifications.Should()
        //        .BeEmpty();

        //    _mocker.GetMock<IPersonRepository>().Verify(x => x.GetByEmailAndPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        //    _mocker.GetMock<IDeviceRepository>().Verify(x => x.Update(It.IsAny<Device>()), Times.Never());
        //    _mocker.GetMock<IDeviceRepository>().Verify(x => x.Add(It.IsAny<Device>()), Times.Never());
        //}


        //[Fact(DisplayName = "UserAuthenticateCommandHandler Handle user not having token")]
        //[Trait("Category", "UserAuthenticateCommandHandler")]
        //public void
        //    UserAuthenticateCommand_WithUserNotHavingToken_Handle_ShouldAddNewTokenAndAttachToUser()
        //{
        //    //Arrange
        //    DomainEvent.Register<DomainNotification>(dn => _notifications.Add(dn));

        //    var command = new UserAuthenticateCommandBuilder()
        //        .WithUser(ValidUserName)
        //        .WithPassword(ValidPassword);

        //    Person userAuthenticated = null;

        //    _mocker.GetMock<IPersonRepository>()
        //        .Setup(u => u.GetByEmailAndPassword(It.IsAny<string>(), It.IsAny<string>()))
        //        .Callback(() =>
        //        {
        //            userAuthenticated = new PersonBuilder()
        //                    .WithToken(null);
        //        })
        //        .Returns(() => userAuthenticated);


        //    Token tokenAdded = null;

        //    _mocker.GetMock<ITokenRepository>()
        //        .Setup(t => t.Add(It.IsAny<Token>()))
        //        .Callback((Token t) =>
        //        {
        //            tokenAdded = new TokenBuilder()
        //                .WithTokenId(t.TokenId)
        //                .WithUserToken(t.UserToken);
        //        })
        //        .Returns(() => tokenAdded);


        //    Person userUpdated = null;

        //    _mocker.GetMock<IPersonRepository>()
        //        .Setup(u => u.Update(It.IsAny<Person>()))
        //        .Callback((Person u) =>
        //        {
        //            userUpdated = new PersonBuilder()
        //                        .WithToken(u.Token);
        //        })
        //        .Returns(() => userUpdated);


        //    var handler = _mocker.Resolve<UserCommandHandler>();

        //    //Act
        //    handler.Handle(command);

        //    //Assert
        //    _notifications.Should()
        //        .BeEmpty();

        //    tokenAdded.Should()
        //        .NotBeNull();
        //    tokenAdded.TokenId.Should()
        //        .NotBeEmpty();
        //    userUpdated.Token.TokenId.Should()
        //        .Be(tokenAdded.TokenId);

        //    _mocker.GetMock<IPersonRepository>().Verify(x => x.GetByEmailAndPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        //    _mocker.GetMock<ITokenRepository>().Verify(x => x.Add(It.IsAny<Token>()), Times.Once());
        //    _mocker.GetMock<IPersonRepository>().Verify(x => x.Update(It.IsAny<Person>()), Times.Once());

        //}

        //[Fact(DisplayName = "UserAuthenticateCommandHandler Handle user having existing token")]
        //[Trait("Category", "UserAuthenticateCommandHandler")]
        //public void
        //    UserAuthenticateCommand_WithUserHavingExistingToken_Handle_ShouldDeactivateTokenAndAttachNewTokenToUser()
        //{
        //    //Arrange
        //    DomainEvent.Register<DomainNotification>(dn => _notifications.Add(dn));

        //    var command = new UserAuthenticateCommandBuilder()
        //        .WithUser(ValidUserName)
        //        .WithPassword(ValidPassword);

        //    Person userAuthenticated = null;

        //    _mocker.GetMock<IPersonRepository>()
        //        .Setup(u => u.GetByEmailAndPassword(It.IsAny<string>(), It.IsAny<string>()))
        //        .Callback(() =>
        //        {
        //            userAuthenticated = new PersonBuilder()
        //                .WithToken(new TokenBuilder()
        //                            .WithTokenId(Guid.NewGuid()));
        //        })
        //        .Returns(() => userAuthenticated);


        //    Token tokenUpdated = null;
        //    _mocker.GetMock<ITokenRepository>()
        //        .Setup(t => t.Update(It.IsAny<Token>()))
        //        .Callback((Token t) =>
        //        {
        //            tokenUpdated = new TokenBuilder()
        //                .WithTokenId(t.TokenId)
        //                .WithActive(t.Active);
        //        })
        //        .Returns(() => tokenUpdated);


        //    Token tokenAdded = null;

        //    _mocker.GetMock<ITokenRepository>()
        //        .Setup(t => t.Add(It.IsAny<Token>()))
        //        .Callback((Token t) =>
        //        {
        //            tokenAdded = new TokenBuilder()
        //                .WithTokenId(t.TokenId)
        //                .WithUserToken(t.UserToken);
        //        })
        //        .Returns(() => tokenAdded);


        //    Person userUpdated = null;

        //    _mocker.GetMock<IPersonRepository>()
        //        .Setup(u => u.Update(It.IsAny<Person>()))
        //        .Callback((Person u) =>
        //        {
        //            userUpdated = new PersonBuilder()
        //                .WithToken(u.Token);
        //        })
        //        .Returns(() => userUpdated);

        //    var handler = _mocker.Resolve<UserCommandHandler>();

        //    //Act
        //    handler.Handle(command);

        //    //Assert
        //    _notifications.Should()
        //        .BeEmpty();

        //    tokenUpdated.Should()
        //        .NotBeNull();
        //    tokenUpdated.TokenId.Should()
        //        .NotBeEmpty();
        //    tokenUpdated.Active.Should()
        //        .BeFalse();

        //    userUpdated.Token.TokenId.Should()
        //        .NotBe(tokenUpdated.TokenId);
        //    userUpdated.Token.TokenId.Should()
        //        .Be(tokenAdded.TokenId);

        //    _mocker.GetMock<IPersonRepository>().Verify(x => x.GetByEmailAndPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        //    _mocker.GetMock<ITokenRepository>().Verify(x => x.Update(It.IsAny<Token>()), Times.Once());
        //    _mocker.GetMock<ITokenRepository>().Verify(x => x.Add(It.IsAny<Token>()), Times.Once());
        //    _mocker.GetMock<IPersonRepository>().Verify(x => x.Update(It.IsAny<Person>()), Times.Once());
        //}
        #endregion

        [Fact(DisplayName = "UserAuthenticateCommandHandler Handle 4 - valid new SerialKey")]
        [Trait("Category", "UserAuthenticateCommandHandler")]
        public void
            UserAuthenticateCommand_Valid_Handle_ShouldReturnSuccessAndChangeUserSerialKey()
        {
            //Arrange
            DomainEvent.Register<DomainNotification>(dn => _notifications.Add(dn));

            _mocker.GetMock<IValidationService>()
                .Setup(v => v.Validate(It.IsAny<ICommand>()))
                .Returns(() => true);

            var serialKey = Guid.NewGuid().ToString();

            var command = new UserAuthenticateCommandBuilder()
                .WithUser(ValidUserName)
                .WithPassword(ValidPassword);

            Person person = null;

            _mocker.GetMock<IPersonRepository>()
                .Setup(u => u.GetByEmailAndPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Callback(() =>
                {
                    person = new PersonBuilder()
                        .WithSerialKey(serialKey);
                })
                .Returns(() => person);

            _mocker.GetMock<IPersonRepository>()
                .Setup(u => u.Update(It.IsAny<Person>()))
                .Callback((Person u) =>
                {
                    person.SetSerialKey(u.SerialKey);
                })
                .Returns(() => person);

            var handler = _mocker.Resolve<UserCommandHandler>();

            //Act
            handler.Handle(command);

            //Assert
            _notifications.Should()
                .BeEmpty();

            person.SerialKey.Should()
                .NotBe(serialKey);

            _mocker.GetMock<IPersonRepository>().Verify(x => x.Update(It.IsAny<Person>()), Times.Once());
        }

        [Fact(DisplayName = "UserAuthenticateCommandHandler Handle 5 - valid PushToken set")]
        [Trait("Category", "UserAuthenticateCommandHandler")]
        public void
            UserAuthenticateCommand_Valid_Handle_ShouldReturnSuccessAndSetPushTokenCorrectly()
        {
            //Arrange
            DomainEvent.Register<DomainNotification>(dn => _notifications.Add(dn));

            _mocker.GetMock<IValidationService>()
                .Setup(v => v.Validate(It.IsAny<ICommand>()))
                .Returns(() => true);

            UserAuthenticateCommand command = new UserAuthenticateCommandBuilder()
                .WithUser(ValidUserName)
                .WithPassword(ValidPassword)
                .WithPushToken(Guid.NewGuid().ToString());

            Person person = new PersonBuilder();

            _mocker.GetMock<IPersonRepository>()
                .Setup(u => u.GetByEmailAndPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => person);

            _mocker.GetMock<IPersonRepository>()
                .Setup(u => u.Update(It.IsAny<Person>()))
                .Callback((Person u) =>
                {
                    person.SetPushToken(u.PushToken);
                })
                .Returns(() => person);

            var handler = _mocker.Resolve<UserCommandHandler>();

            //Act
            handler.Handle(command);

            //Assert
            _notifications.Should()
                .BeEmpty();

            person.PushToken.Should()
                .Be(command.PushToken);

            _mocker.GetMock<IPersonRepository>().Verify(x => x.Update(It.IsAny<Person>()), Times.Once());
        }
    }
}
