using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.Owin;
using Data;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using Data.Repository;
using Services.Classes;
using Services.Interfaces;

namespace WebUI.App_Start
{
    public class AutofacConfig
    {
                // метод для конфигурации MVC контроллера
        public static void Configure(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            #region регистрация контроллеров

            // для MVC
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            #endregion

            #region регистрация сервисов

            builder.RegisterType<TestService>().As<ITestService>();
            builder.RegisterType<GroupService>().As<IGroupService>();

            #endregion

            #region регистрация репозиториев

            builder.RegisterType<TestRepository>().As<ITestRepository>();
            builder.RegisterType<GroupRepository>().As<IGroupRepository>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();

            #endregion

            // регестрируем контекст данных и указываем его процесс создания
            builder.RegisterType<ApplicationDbContext>().InstancePerRequest();
            

            var container = builder.Build();
            // метод для конфигурации MVC контроллера
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();

        }
    }

}
