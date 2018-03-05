using Autofac;
using BaseDatabase.Services.ShareLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDatabase.BaseDbInstanceMangers
{
    public class BaseDbInstanceManger
    {
        private static IContainer container;

        public static void RegisterType()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ShareLogInfoService>().As<IShareLogInfoService>().InstancePerLifetimeScope();

            container = builder.Build();
        }

        public static IShareLogInfoService GetShareLogInfoService()
        {
            return container.Resolve<IShareLogInfoService>();
        }

    }
}
