using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using Ninject;
using System.Web.Mvc;
using Moq;
using TStore.Domain.Entities;
using TStore.Domain.Abstract;
using TStore.Domain.Concrete;
using TStore.WebUI.Infrastructure.Abstract;
using TStore.WebUI.Infrastructure.Concrete;
using System.Configuration;

namespace TStore.WebUI.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernal;

        public NinjectControllerFactory() {
            ninjectKernal = new StandardKernel();
            AddBindings();
        }
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null
                ? null
                : (IController)ninjectKernal.Get(controllerType);
        }

        private void AddBindings()
        {
            ninjectKernal.Bind<ISpidersRepository>().To<EFSpiderRepository>();

            EmailSettings emailSettings = new EmailSettings
            {
                WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")

            };

            ninjectKernal.Bind<IOrderProcessor>().To<EmailOrderProcessor>().WithConstructorArgument("settings", emailSettings);

            ninjectKernal.Bind<IAuthProvider>().To<FormsAuthProvider>();
        }
    }
}