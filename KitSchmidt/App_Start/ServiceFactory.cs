using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KitSchmidt.App_Start
{
    public static class ServiceFactory
    {
        public static IContainer Container { get; set; }

        public static T GetFromContainer<T>()
        {
            using (var scope = Container.BeginLifetimeScope())
                return scope.Resolve<T>();
        }
    }
}