﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Mvc.App_Start
{
    public class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            return container;
        }
        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here  
            //This is the important line to edit  
            container.RegisterType<ICompanyRepository, CompanyRepository>();


            RegisterTypes(container);
            return container;
        }
        public static void RegisterTypes(IUnityContainer container)
        {

        }
    }
}