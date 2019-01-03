using SimpleInjector;
using ViewT.Crediario.Domain.Core.Commands;
using ViewT.Crediario.Domain.Core.DomainNotification;
using ViewT.Crediario.Domain.Core.DomainNotification.Events;
using ViewT.Crediario.Domain.Core.DomainNotification.Handlers;
using ViewT.Crediario.Domain.Core.Interfaces;
using ViewT.Crediario.Domain.Main.Commands.Handlers;
using ViewT.Crediario.Domain.Main.Commands.Inputs;
using ViewT.Crediario.Domain.Main.Events;
using ViewT.Crediario.Domain.Main.Interfaces;
using ViewT.Crediario.Domain.Main.Services;
using ViewT.Crediario.Infra.Data.Context;
using ViewT.Crediario.Infra.Data.RepositoryFake;
using ViewT.Crediario.Infra.Data.Service;
using ViewT.Crediario.Infra.Data.UoW;

namespace ViewT.Crediario.Infra.CrossCutting.IoC
{
    public class BootStrapper
    {
        public static void RegisterServices(Container container)
        {
            // App
            container.Register<IDomainNotificationHandler<DomainNotification>, DomainNotificationHandler>(Lifestyle.Scoped);
            container.Register<IHandler<DomainNotification>, DomainNotificationHandler>(Lifestyle.Scoped);

            // Domain
            container.Register<IUnitOfWork, UnitOfWork>(Lifestyle.Scoped);

            container.Register<ICommandHandler<UserRegisterCommand>, UserCommandHandler>(Lifestyle.Scoped);
            container.Register<ICommandHandler<UserAuthenticateCommand>, UserCommandHandler>(Lifestyle.Scoped);
            container.Register<ICommandHandler<UserForgotPasswordCommand>, UserCommandHandler>(Lifestyle.Scoped);
            container.Register<ICommandHandler<UserChangePasswordCommand>, UserCommandHandler>(Lifestyle.Scoped);

            //Events
            container.Register<IHandler<UserForgotPasswordRequestedEvent>, UserForgotPasswordRequestedEventHandler>(Lifestyle.Scoped);

            // Infra Dados
            container.Register<CrediarioContext>(Lifestyle.Scoped);

            #region .: RealRepositories :.
            #endregion

            #region .: FakeRepositories :.
            container.Register<IVersionRepository, VersionFakeRepository>(Lifestyle.Scoped);
            container.Register<IPersonRepository, PersonFakeRepository>(Lifestyle.Scoped);
            container.Register<IEmailNotificationRepository, EmailNotificationFakeRepository>(Lifestyle.Scoped);
            #endregion

            container.Register<IValidationService, ValidationService>(Lifestyle.Scoped);
            container.Register<IEmailService, EmailService>(Lifestyle.Scoped);
            container.Register<IPasswordService, PasswordService>(Lifestyle.Scoped);
            container.Register<IPushService, PushService>(Lifestyle.Scoped);

        }
    }
}