using Autofac;
using KitSchmidt.Common.DAL;
using KitSchmidt.DAL;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KitSchmidt.App_Start
{
    public class KitModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register services
            builder
                .RegisterType<DbContext>()
                .Keyed<KitContext>(FiberModule.Key_DoNotSerialize)
                .As<KitContext>()
                .InstancePerLifetimeScope();

            //builder
            //    .RegisterType<MessageDataService>()
            //    .Keyed<IMessageDataService>(FiberModule.Key_DoNotSerialize)
            //    .As<IMessageDataService>()
            //    .InstancePerLifetimeScope();
        }
    }
}