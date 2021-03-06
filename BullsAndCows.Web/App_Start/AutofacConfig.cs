﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using BullsAndCows.Core;
using BullsAndCows.Data;
using BullsAndCows.Data.Repositories;

namespace BullsAndCows.Web
{
    public class AutofacConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();

            // Register your MVC controllers. (MvcApplication is the name of
            // the class in Global.asax.)

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterAssemblyTypes(typeof(MvcApplication).Assembly)
                .AsImplementedInterfaces();

            builder.RegisterModule(new AutofacWebTypesModule());

            builder.RegisterType<BullsCowsDbContext>().InstancePerRequest();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));

            builder.RegisterType<GameManager>().As<IGameManager>().InstancePerRequest();
            builder.RegisterType<RankingManager>().As<IRankingManager>().InstancePerRequest();


            // OPTIONAL: Register model binders that require DI.
            builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            builder.RegisterModelBinderProvider();

            // OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            // OPTIONAL: Enable property injection in view pages.
            builder.RegisterSource(new ViewRegistrationSource());

            // OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();

            // OPTIONAL: Enable action method parameter injection (RARE).
            //builder.InjectActionInvoker();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}